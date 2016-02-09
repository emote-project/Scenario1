using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using System.Threading.Tasks;

namespace EmoteNaoInstaller
{
    public class EmoteNaoPackage
    {
        const string INSTALL_FILE = @"\install.epkg";
        const string DATA_FILE = @"\data.tar.gz";

        public string Name { get; set; }
        public string WorkingDirectory { get; set; }
        public List<string> PreInstallCommands { get; set; }
        public List<string> PostInstallCommands { get; set; }
        public string DataFilePath {get; set; }

        public EmoteNaoPackage() 
        {
            PreInstallCommands = new List<string>();
            PostInstallCommands = new List<string>();
        }
        public EmoteNaoPackage(string workingDirectory, List<string> preInstallationCommands, List<string>postInstallationCommands) 
        {
            WorkingDirectory = workingDirectory;
            PreInstallCommands = preInstallationCommands;
            PostInstallCommands = postInstallationCommands;
        }

        public async void Create(string dataFolderPath, string destinationPath)
        {
            await CreateTarGZAsync(destinationPath, dataFolderPath);

            using (XmlTextWriter myWriter = new XmlTextWriter(destinationPath+INSTALL_FILE,Encoding.UTF8))
            {
                myWriter.Formatting = Formatting.Indented;
                myWriter.Indentation = 5;
                XmlSerializer mySerializer = new XmlSerializer(typeof(EmoteNaoPackage));
                mySerializer.Serialize(myWriter, this);
            }
        }


        public static EmoteNaoPackage Load(string path)
        {
            string installFilePath = path + INSTALL_FILE;
            if (File.Exists(installFilePath))
            {
                using (StreamReader myWriter = new StreamReader(installFilePath, false))
                {
                    XmlSerializer mySerializer = new XmlSerializer(typeof(EmoteNaoPackage));
                    EmoteNaoPackage package = (EmoteNaoPackage)mySerializer.Deserialize(myWriter);
                    package.DataFilePath = path + DATA_FILE;
                    package.Name = Path.GetFileName(Path.GetDirectoryName(path + DATA_FILE)); 
                    return package;
                }
            }
            else
            {
                Console.Error.WriteLine("No install file found in the package folder");
                return null;
            }
        }

        private Task<bool> CreateTarGZAsync(string tgzPath, string sourceDirectory)
        {
            return Task.Run<bool>(() => CreateTarGZ(tgzPath, sourceDirectory));
        }

        #region Compressing stuff
        //https://github.com/icsharpcode/SharpZipLib/wiki/GZip-and-Tar-Samples#createTGZ

        private bool CreateTarGZ(string tgzPath, string sourceDirectory)
        {
            if (!Directory.Exists(tgzPath))
                Directory.CreateDirectory(tgzPath);
            Stream outStream = File.Create(tgzPath+DATA_FILE);
            Stream gzoStream = new GZipOutputStream(outStream);
            TarArchive tarArchive = TarArchive.CreateOutputTarArchive(gzoStream);

            // Note that the RootPath is currently case sensitive and must be forward slashes e.g. "c:/temp"
            // and must not end with a slash, otherwise cuts off first char of filename
            // This is scheduled for fix in next release
            tarArchive.RootPath = sourceDirectory.Replace('\\', '/');
            if (tarArchive.RootPath.EndsWith("/"))
                tarArchive.RootPath = tarArchive.RootPath.Remove(tarArchive.RootPath.Length - 1);

            AddDirectoryFilesToTar(tarArchive, sourceDirectory, true);

            tarArchive.Close();
            return true;
        }

        private void AddDirectoryFilesToTar(TarArchive tarArchive, string sourceDirectory, bool recurse)
        {
            // Optionally, write an entry for the directory itself.
            // Specify false for recursion here if we will add the directory's files individually.
            //
            TarEntry tarEntry = TarEntry.CreateEntryFromFile(sourceDirectory);
            tarArchive.WriteEntry(tarEntry, false);

            // Write each file to the tar.
            //
            string[] filenames = Directory.GetFiles(sourceDirectory);
            foreach (string filename in filenames)
            {
                tarEntry = TarEntry.CreateEntryFromFile(filename);
                tarArchive.WriteEntry(tarEntry, true);
            }

            if (recurse)
            {
                string[] directories = Directory.GetDirectories(sourceDirectory);
                foreach (string directory in directories)
                    AddDirectoryFilesToTar(tarArchive, directory, recurse);
            }
        }

        
        #endregion
    }
}
