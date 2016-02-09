using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;

namespace DecisionTree
{
    public class ExcelLoader
    {
        /*Returns a datatable without any null cells*/
        public static System.Data.DataTable bla(System.Data.DataTable dataTable, string columnName)
        {
            DataTable table = new DataTable();

            IEnumerable<DataRow> test = from x in dataTable.AsEnumerable()
                                        where !x.IsNull(columnName)
                                        select x;
           
            table.Columns.Add(columnName, typeof(int));

            foreach (var q in test){
                //Console.WriteLine(q["Energy"].ToString());
                table.Rows.Add(Convert.ToInt32(q[columnName]));
            }

            /*foreach (DataRow t in table.Rows)
               Console.WriteLine(t["Energy"].ToString());
           */

            return table;
        }

        public static List<Int32> GetColumnInt(System.Data.DataTable dataTable, string columnName)
        {
            /*var query = from item in dataTable.AsEnumerable()
                        from f in item.ItemArray
                        where f != null
                        select item;
            

            
            foreach(var i in query)
            Console.WriteLine(i);*/

            string[] all = dataTable.AsEnumerable().Where(x => !x.IsNull(columnName)) //!String.IsNullOrEmpty(x.ToString()))
                .Select(x => x[columnName].ToString()).ToArray();

            foreach (string i in all)
                Console.WriteLine(i);

            //string[] all = dataTable.AsEnumerable().Where(x => !String.IsNullOrEmpty(x.ToString()))
              //  .Select(x => x[columnName].ToString()).ToArray();

            List<Int32> tableInt = new List<Int32>();

            foreach (string i in all)
            {
                if (!String.IsNullOrEmpty(i))
                    tableInt.Add(Convert.ToInt32(i));

            }
            return tableInt;
        }

        public static List<String> GetColumnString(System.Data.DataTable dataTable, string columnName)
        {
            string[] all = dataTable.AsEnumerable().Where(x => !String.IsNullOrEmpty(x.ToString()))
                .Select(x => x[columnName].ToString()).ToArray();

            List<String> tableStr = new List<string>();

            foreach (string i in all)
            {
                if (!String.IsNullOrEmpty(i))
                    tableStr.Add(i);

            }
            return tableStr;
        }

        /*Ruterns a DataTable with distinct values and its scores*/
        public static System.Data.DataTable CountDistValues2(System.Data.DataTable dataTable, string columnName)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            // Returns the score of each distinct value in a give column
            var query = dataTable.AsEnumerable().GroupBy(l => l[columnName]).Select(g => new
            {
                Value = g.Key,
                Ct = g.Count()
            });

            table.Columns.Add("Value", typeof(int));
            table.Columns.Add("Ct", typeof(int));

            foreach (var q in query)
                table.Rows.Add( Convert.ToInt32(q.Value), Convert.ToInt32(q.Ct));

            /*foreach (DataRow r in table.Rows)
                Console.WriteLine("Value: {0}, Ct: {1}",r["Value"].ToString(), r["Ct"].ToString());
            */

            return table;
        }

        /*Returns a Dictionary with the distinct values and its scores*/
        public static Dictionary<string, int> CountDistValues(System.Data.DataTable dataTable, string columnName)
        {
            Dictionary<string, int> distValues = new Dictionary<string, int>();
            // Returns the score of each distinct value in a give column
            var query = dataTable.AsEnumerable().GroupBy(l => l[columnName]).Select(g => new
            {
                Value = g.Key,
                Ct = g.Count()
            });

            foreach (var q in query)
                distValues.Add(q.Value.ToString(), Convert.ToInt32(q.Ct));

            return distValues;
        }

        /*order a datatable*/
        public static System.Data.DataTable OrderDataTable(System.Data.DataTable table)
        {
            var ordered = from order in table.AsEnumerable()
                          orderby order.Field<int>("Value")
                          select order;

            DataTable ordertable = ordered.CopyToDataTable();

           /* foreach (DataRow r in ordertable.Rows)
                Console.WriteLine("Value: {0}, Ct: {1}", r["Value"].ToString(), r["Ct"].ToString());
            */
            return ordertable;
        }

        /*Returns which distinct value has the higher frequency, given a DataTable*/
        public static string MaxFrequency2(System.Data.DataTable table)
        {
            int value = 0;
            string max = "";

            foreach (DataRow r in table.Rows)
            {
                if (value < Convert.ToInt32(r["Ct"]))
                {
                    value = Convert.ToInt32(r["Ct"]);
                    max = r["Value"].ToString();
                }
            }
            return max;
        }
        /*Returns which distinct value has the higher frequency, given a Dictionary*/
        public static string MaxFrequency(Dictionary<string, int> query)
        {
            int value = 0;
            string max = "";
            foreach (var q in query)
            {
                if (value < q.Value)
                {
                    value = q.Value;
                    max = q.Key;
                }
            }
            return max;
        }

        



        /****************************************/

        public static Dictionary<string, System.Data.DataTable> ImportExcelXLS(string FileName, bool hasHeaders)
        {
            
            string HDR = hasHeaders ? "Yes" : "No";
            string strConn;
            if (FileName.Substring(FileName.LastIndexOf('.')).ToLower() == ".xlsx")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=0\";Mode=\"Share Deny None\"";
            else
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=0\";Mode=\"Share Deny None\"";

            Dictionary<string, System.Data.DataTable> output = new Dictionary<string, System.Data.DataTable>();

            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                conn.Open();
                
                System.Data.DataTable schemaTable = conn.GetOleDbSchemaTable(
                    OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                foreach (DataRow schemaRow in schemaTable.Rows)
                {
                    string sheet = schemaRow["TABLE_NAME"].ToString();
                    try
                    {
                        OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sheet + "]", conn);
                        cmd.CommandType = CommandType.Text;

                        System.Data.DataTable table = new System.Data.DataTable(sheet);
                        new OleDbDataAdapter(cmd).Fill(table);
                        output[sheet] = table;
                        Console.WriteLine(string.Format("Loaded table {0}.", sheet));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message + string.Format("Sheet:{0}.File:F{1}", sheet, FileName), ex);
                    }
                }
                conn.Close();
            }
            return output;
        }
    }
}