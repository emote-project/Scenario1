using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Renci.SshNet;
using System.IO;

namespace EmoteNaoInstaller
{
    class EmotePackageManager
    {
        public class LogEventArgs : EventArgs
        {
            public string Message { get; private set; }
            public bool IsFileTransfer { get; private set; }
            public double FileTransferCompletition { get; private set; }
            public LogEventArgs(string message, bool isFileTransfer = false, double fileTransferCompletition = 0 )
            {
                Message = message;
                IsFileTransfer = isFileTransfer;
                FileTransferCompletition = fileTransferCompletition;
            }
        
        }
        static public event EventHandler<LogEventArgs> LogEvent;
        static public event EventHandler UploadCompleted;



        static private double uploadingFileSize = 0;


        static public Task<bool> InstallAsync(RemoteNAO remote, EmoteNaoPackage package)
        {
            return Task.Run<bool>(() => Install(remote, package));
        }

        static async public Task<bool> Install(RemoteNAO remote, EmoteNaoPackage package)
        {
            Log("Starting installation");

            Log("Setting working directory to " + package.WorkingDirectory);
            remote.ExecuteCommand("cd " + package.WorkingDirectory);

            Log("Executing preinstallation commands: ");
            foreach (string command in package.PreInstallCommands)
            {
                Log(command);
                var res = await ExecuteCommandAsync(remote, command);
                if (res.Error != null)
                {
                    Log("Error: " + res.Error);
                }
                else
                {
                    Log(res.Result);
                }
            }

            Log("Uploading data file. Please wait...");
            bool result = await UploadFileAsync(remote, package.DataFilePath, UploadStatus);
            Log("Upload completed");

            Log("Executing preinstallation commands: ");
            foreach (string command in package.PostInstallCommands)
            {
                Log(command);
                var res = await ExecuteCommandAsync(remote, command);
                if (res.Error != "")
                {
                    Log("Error: " + res.Error);
                }
                if (res.Result != "")
                {
                    Log(res.Result);
                }
            }

            Log(Environment.NewLine+"Installation completed");
            return result;
        }

        static private void UploadStatus(ulong status)
        {
            Log("Uplaod status: ",true,status/uploadingFileSize);
        }

        static private Task<SshCommand> ExecuteCommandAsync(RemoteNAO remote, string command)
        {
            return Task.Run<SshCommand>(() => ExecuteCommand(remote, command));
        }

        static private SshCommand ExecuteCommand(RemoteNAO remote, string command)
        {
            try
            {
                SshCommand res = remote.ExecuteCommand(command);
                return res;
            }
            catch (System.Exception e)
            {
                Log("An exception occurred while executing command" + command);
                Log("Exception: " + e.Message);
                return null;
            }
        }

        static private Task<bool> UploadFileAsync(RemoteNAO remote, string path, Action<ulong> uploadCallback)
        {
            return Task.Run<bool>(() => UploadFile( remote,  path,  uploadCallback));
        }

        static private bool UploadFile(RemoteNAO remote, string path, Action<ulong> uploadCallback)
        {
            if (File.Exists(path))
            {
                uploadingFileSize = new FileInfo(path).Length;
            }
            else
            {
                return false;
            }
            try
            {
                var res = remote.UploadFile(path, uploadCallback);
                uploadingFileSize = 0;
                UploadCompleted(null,null);
                return res;
            }
            catch (Renci.SshNet.Common.SftpPermissionDeniedException e)
            {
                Log("An exception occurred while uploading the file to " + path);
                Log("Exception: " + e.Message);
                return false;
            }
        }

        static private void Log(string message, bool isFileTransfer = false, double fileTransferCompletition = 0)
        {
            Console.WriteLine(message+(isFileTransfer?" - "+fileTransferCompletition:""));
            LogEvent(null,new LogEventArgs(message,isFileTransfer,fileTransferCompletition) );
        }

    }
}
