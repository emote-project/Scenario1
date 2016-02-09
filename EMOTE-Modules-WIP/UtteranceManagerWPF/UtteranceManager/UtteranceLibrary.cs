using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;

namespace UtteranceManager
{
    public class UtteranceLibrary
    {
        public delegate void LibraryModifiedHandler();
        public event LibraryModifiedHandler LibraryModified;
        private void NotifyLibraryModified()
        {
            if (LibraryModified != null) LibraryModified();
        }

        private Dictionary<string, List<string>> categories = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> Categories
        {
            get { return categories; }
        }

        private System.Data.DataTable utterances;
        public System.Data.DataTable Utterances
        {
            get { return utterances; }
        }

        FileSystemWatcher watcher;
        string libraryFilename = "";
        Random rand;
        public bool CaseSensitiveCategories { get; private set; }

        public void LoadLibrary(string fileName, bool caseSensitiveCategories = false)
        {
            if (fileName == "" || !File.Exists(fileName))
            {
                throw new Exception(string.Format("No such file '{0}'!", fileName));
            }
            CaseSensitiveCategories = caseSensitiveCategories;
            utterances = ImportExcelXLS(fileName, true);
            libraryFilename = fileName;
            Console.WriteLine("Loaded utterance library from '{0}'.", fileName);
            CreateFileWatcher(fileName);
            rand = new Random(Environment.TickCount);
        }

        public List<string> FilterUtterances(string category, string subcategory = "-")
        {
            string fullCategory = category + ((subcategory == "-") ? "" : ":" + subcategory);
            if (!CaseSensitiveCategories)
            {
                fullCategory = fullCategory.ToLower();
                return Utterances.AsEnumerable()
                             .Where(r => r.Field<string>("CATEGORY").ToLower() == fullCategory)
                             .Select(x => x[0].ToString()).ToList();
            }
            else
            {
                return Utterances.AsEnumerable()
                             .Where(r => r.Field<string>("CATEGORY") == fullCategory)
                             .Select(x => x[0].ToString()).ToList();
            }
            
            
        }

        public List<string> FilterSubcategories(string category)
        {
            if (!CaseSensitiveCategories)
            {
                category = category.ToLower();
            }
            if (categories.ContainsKey(category)) return categories[category];
            else return new List<string>();
        }

        public string GetUtterance(string category, string subcategory = "-")
        {
            if (!CaseSensitiveCategories)
            {
                category = category.ToLower();
                subcategory = subcategory.ToLower();
            }
            if (categories.ContainsKey(category))
            {
                if (categories[category].Contains(subcategory))
                {
                    List<string> utt = FilterUtterances(category, subcategory);
                    return utt[rand.Next(utt.Count)];
                }
                else return "";
            }
            else return "";
        }
        private void CreateFileWatcher(string filename)
        {
            watcher = new FileSystemWatcher();
            watcher.Path = Path.GetDirectoryName(filename);
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
               | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = Path.GetFileName(filename); ;
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);
            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("Utterances database modified.");
            NotifyLibraryModified();
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            if (e.FullPath == libraryFilename)
            {
                Console.WriteLine("Utterances database modified.");
                NotifyLibraryModified();
                
            }
        }

        private System.Data.DataTable ImportExcelXLS(string FileName, bool hasHeaders)
        {
            string HDR = hasHeaders ? "Yes" : "No";
            string strConn;
            if (FileName.Substring(FileName.LastIndexOf('.')).ToLower() == ".xlsx")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\";Mode=\"Share Deny None\"";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\";Mode=\"Share Deny None\"";

            System.Data.DataTable output = new System.Data.DataTable();

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                categories = new Dictionary<string, List<string>>();

                conn.Open();

                System.Data.DataTable schemaTable = conn.GetOleDbSchemaTable(
                    OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                foreach (DataRow schemaRow in schemaTable.Rows)
                {
                    string sheet = schemaRow["TABLE_NAME"].ToString();
                    if (sheet.ToLower() == "utterances$")
                    {
                        try
                        {
                            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sheet + "] WHERE TEXT<>'' AND CATEGORY<>''", conn);
                            cmd.CommandType = CommandType.Text;
                            output = new System.Data.DataTable(sheet);
                            new OleDbDataAdapter(cmd).Fill(output);
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message + string.Format("Sheet:{0}.File:F{1}", sheet, FileName), ex);
                        }
                    }
                    else if (sheet.ToLower() == "categories$")
                    {
                        System.Data.DataTable categoriesTable;
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sheet + "]", conn);
                        cmd.CommandType = CommandType.Text;

                        categoriesTable = new System.Data.DataTable(sheet);
                        new OleDbDataAdapter(cmd).Fill(categoriesTable);
                        List<string> categoriesList = categoriesTable.AsEnumerable().Select(x => x[0].ToString()).ToList();
                        foreach (string c in categoriesList)
                        {
                            if (c.Trim().Length == 0 || c.StartsWith(":")) continue;
                            string[] split = c.Trim().Split(':');
                            if (!CaseSensitiveCategories) split[0] = split[0].ToLower();
                            if (!categories.ContainsKey(split[0])) categories[split[0]] = new List<string>();
                            if (split.Length > 1)
                            {
                                if (!CaseSensitiveCategories) categories[split[0]].Add(split[1].ToLower());
                                else categories[split[0]].Add(split[1]);
                            }
                            else if (!categories[split[0]].Contains("-")) categories[split[0]].Add("-");
                        }
                    }
                }
                conn.Close();
            }
            return output;
        } 
    }
}
