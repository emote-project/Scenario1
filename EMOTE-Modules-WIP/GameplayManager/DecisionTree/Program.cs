using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DecisionTree
{
    class Program
    {
        
        static DataTable TestTable = new DataTable();

        static void Main(string[] args)
        {
            Dictionary<string, System.Data.DataTable> database;
            try
            {
                database = ExcelLoader.ImportExcelXLS(@"..\..\Tabela.xlsx", false);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.ToString());
                Console.ReadLine();
                return;
            }

            List<System.Data.DataTable> tabelas = new List<System.Data.DataTable>();
            tabelas.Add(database["A$"]);
            tabelas.Add(database["B$"]);
            tabelas.Add(database["C$"]);
            tabelas.Add(database["E$"]);
            tabelas.Add(database["G$"]);
            tabelas.Add(database["H$"]);
            tabelas.Add(database["I$"]);
            tabelas.Add(database["J$"]);
            tabelas.Add(database["K$"]);
             
            DataTable tabelasTodasEnergy = ProcessTables(tabelas, "Energy");
            DataTable orderedtableEnergy = ExcelLoader.OrderDataTable(ExcelLoader.CountDistValues2(tabelasTodasEnergy, "Energy"));
            
            DataTable tabelasTodasMoney = ProcessTables(tabelas, "Money");
            DataTable orderedtableMoney = ExcelLoader.OrderDataTable(ExcelLoader.CountDistValues2(tabelasTodasMoney, "Money"));

            DataTable tabelasTodasOil = ProcessTables(tabelas, "Oil");
            DataTable orderedtableOil = ExcelLoader.OrderDataTable(ExcelLoader.CountDistValues2(tabelasTodasOil, "Oil"));

            DataTable tabelasTodasScoreEco = ProcessTables(tabelas, "ScoreEco");
            DataTable orderedtableScoreEco = ExcelLoader.OrderDataTable(ExcelLoader.CountDistValues2(tabelasTodasScoreEco, "ScoreEco"));

            DataTable tabelasTodasScoreEnv = ProcessTables(tabelas, "ScoreEnv");
            DataTable orderedtableScoreEnv = ExcelLoader.OrderDataTable(ExcelLoader.CountDistValues2(tabelasTodasScoreEnv, "ScoreEnv"));

            DataTable tabelasTodasScoreMay = ProcessTables(tabelas, "ScoreMay");
            DataTable orderedtableScoreMay = ExcelLoader.OrderDataTable(ExcelLoader.CountDistValues2(tabelasTodasScoreMay, "ScoreMay"));

            DataTable tabelasTodasPopulation = ProcessTables(tabelas, "Population");
            DataTable orderedtablePopulation = ExcelLoader.OrderDataTable(ExcelLoader.CountDistValues2(tabelasTodasPopulation, "Population"));

            Categorization energycolumn = new Categorization(orderedtableEnergy, "Energy", 6); //melhorou
            Categorization moneycolumn = new Categorization(orderedtableMoney, "Money", 5);  //nao melhorou - 5
            Categorization oilcolumn = new Categorization(orderedtableOil, "Oil", 6); //nao melhorou - 6
            Categorization ScoreEcocolumn = new Categorization(orderedtableScoreEco, "ScoreEco", 4); // 4 (menor ou igual a 4)
            Categorization ScoreEnvcolumn = new Categorization(orderedtableScoreEnv, "ScoreEnv", 6); //não melhorou - 6
            Categorization ScoreMaycolumn = new Categorization(orderedtableScoreMay, "ScoreMay", 4); //nao melhorou - 5
            Categorization populationcolumn = new Categorization(orderedtablePopulation, "Population", 6);

            List<List<int>> levelManualCat = new List<List<int>>(){ {new List<int>(){1,1}}, {new List<int>(){2,2}}, 
                                                                    {new List<int>(){3,3}}, {new List<int>(){4,4}} };
            Categorization levelcolumn = new Categorization(levelManualCat, "Level");

            Dictionary<string, Categorization> categoriesMap = new Dictionary<string,Categorization>(){{"Energy", energycolumn},
                                                                                                       {"Money", moneycolumn},
                                                                                                       {"Oil", oilcolumn},
                                                                                                       {"ScoreEco", ScoreEcocolumn},
                                                                                                       {"ScoreEnv", ScoreEnvcolumn},
                                                                                                       {"ScoreMay", ScoreMaycolumn},
                                                                                                       {"Level", levelcolumn},
                                                                                                       {"Population", populationcolumn}};

            DataTable jogo = ProcessColumns(tabelas);

            foreach(DataRow jogada in jogo.Rows)
            {
                int newenergy = energycolumn.NewValue(Convert.ToInt32(jogada["Energy"]));
                jogada.SetField("Energy", newenergy);

                int newmoney = moneycolumn.NewValue(Convert.ToInt32(jogada["Money"]));
                jogada.SetField("Money", newmoney);

                int newoil = oilcolumn.NewValue(Convert.ToInt32(jogada["Oil"]));
                jogada.SetField("Oil", newoil);

                int newScoreEco = ScoreEcocolumn.NewValue(Convert.ToInt32(jogada["ScoreEco"]));
                jogada.SetField("ScoreEco", newScoreEco);

                int newScoreEnv = ScoreEnvcolumn.NewValue(Convert.ToInt32(jogada["ScoreEnv"]));
                jogada.SetField("ScoreEnv", newScoreEnv);

                int newScoreMay = ScoreMaycolumn.NewValue(Convert.ToInt32(jogada["ScoreMay"]));
                jogada.SetField("ScoreMay", newScoreMay);

                int newpopulation = populationcolumn.NewValue(Convert.ToInt32(jogada["Population"]));
                jogada.SetField("Population", newpopulation);
            }

            System.IO.StreamWriter file1 = new System.IO.StreamWriter("Tabela.txt");
            string line1;
           // for (int i = 0; i < jogo.Rows.Count; i++)
            //{
                for (int j = 0; j < jogo.Columns.Count; j++)
                {
                    line1 = jogo.Columns[j].ColumnName;
                    file1.WriteLine(line1);
                }
            //}
            file1.Close();

            DecisionTreeImplementation sam = new DecisionTreeImplementation();

           TreeNode decisionTree = sam.GetTree(jogo);

           StrategyDecision chooseStrategy = new StrategyDecision(decisionTree, categoriesMap);

           int correct = 0;
           int incorrect = 0;
           int S1_S1 = 0;
           int S1_S2 = 0;
           int S1_S3 = 0;
           int S1_SG = 0;

           int S2_S1 = 0;
           int S2_S2 = 0;
           int S2_S3 = 0;
           int S2_SG = 0;

           int S3_S1 = 0;
           int S3_S2 = 0;
           int S3_S3 = 0;
           int S3_SG = 0;

           int SG_S1 = 0;
           int SG_S2 = 0;
           int SG_S3 = 0;
           int SG_SG = 0;

           foreach (DataRow t in TestTable.Rows)
           {
               Console.WriteLine(t["Energy"].ToString() + "  " + t["Money"].ToString() + "  " + t["Oil"].ToString() + " " +
                   t["ScoreEco"].ToString() + " " + t["ScoreEnv"].ToString() + " " + t["ScoreMay"].ToString() + " " +
                   t["Level"].ToString() + " " + t["Population"].ToString() + " " + t["StrategyMay"].ToString());

               string predStrategy = chooseStrategy.getStrategy(t);
               string realStrategy = t["StrategyMay"].ToString();

               if (predStrategy.Equals(realStrategy))
               {
                   correct++;
                   if (predStrategy.Equals("S1"))
                       S1_S1++;
                   if (predStrategy.Equals("S2"))
                       S2_S2++;
                   if (predStrategy.Equals("S3"))
                       S3_S3++;
                   if (predStrategy.Equals("SG"))
                       SG_SG++;
               }
               else
               {
                   incorrect++;
                   if (realStrategy.Equals("S1"))
                   {
                       if (predStrategy.Equals("S2"))
                           S1_S2++;
                       if (predStrategy.Equals("S3"))
                           S1_S3++;
                       if (predStrategy.Equals("SG"))
                           S1_SG++;
                   }
                   if (realStrategy.Equals("S2"))
                   {
                       if (predStrategy.Equals("S1"))
                           S2_S1++;
                       if (predStrategy.Equals("S3"))
                           S2_S3++;
                       if (predStrategy.Equals("SG"))
                           S2_SG++;
                   }
                   if (realStrategy.Equals("S3"))
                   {
                       if (predStrategy.Equals("S1"))
                           S3_S1++;
                       if (predStrategy.Equals("S2"))
                           S3_S2++;
                       if (predStrategy.Equals("SG"))
                           S3_SG++;
                   }
                   if (realStrategy.Equals("SG"))
                   {
                       if (predStrategy.Equals("S1"))
                           SG_S1++;
                       if (predStrategy.Equals("S2"))
                           SG_S2++;
                       if (predStrategy.Equals("S3"))
                           SG_S3++;
                   }
               }
           }
           Console.WriteLine("Correctos: " + correct + " incorrectos: " + incorrect);
           Console.WriteLine("S1_S1: " + S1_S1 + " S1_S2: " + S1_S2 + " S1_S3: " + S1_S3 + " S1_SG: " + S1_SG);
           Console.WriteLine("S2_S1: " + S2_S1 + " S2_S2: " + S2_S2 + " S2_S3: " + S2_S3 + " S2_SG: " + S2_SG);
           Console.WriteLine("S3_S1: " + S3_S1 + " S3_S2: " + S3_S2 + " S3_S3: " + S3_S3 + " S3_SG: " + S3_SG);
           Console.WriteLine("SG_S1: " + SG_S1 + " SG_S2: " + SG_S2 + " SG_S3: " + SG_S3 + " SG_SG: " + SG_SG);
           // Console.WriteLine("Final strategy: " + strategy);

            System.IO.StreamWriter file = new System.IO.StreamWriter("testtt.txt");

            string firstline = "Energy;Money;Oil;ScoreEco;ScoreEnv;ScoreMay;Level;Populatio;result";
            file.WriteLine(firstline);
            foreach (DataRow t in jogo.Rows)
            {
                string line = t["Energy"].ToString() + ";" + t["Money"].ToString() + ";" + t["Oil"].ToString() + ";" +
                    t["ScoreEco"].ToString() + ";" + t["ScoreEnv"].ToString() + ";" + t["ScoreMay"].ToString() + ";" +
                    t["Level"].ToString() + ";" + t["Population"].ToString() + ";" + t["StrategyMay"].ToString() ;
                
                file.WriteLine(line);
            }
            file.Close();
                /*
                foreach (List<int> l in dividing)
                {
                    Console.Write("category: ");
                    List<int> li = l;
                    for (int i = 0; i < l.Count; i++)
                        Console.Write(", {0}",li[i]);

                    Console.WriteLine(" end");
                }

                List<int[]> categories = newcolumn.FinalCategories(dividing);

            */

           /* foreach (DataRow t in tabelasTodas.Rows)
                Console.WriteLine(t["Energy"].ToString());
            */

           /* foreach (DataRow row in tabelasTodas.Rows)
            {
                foreach (DataColumn col in tabelasTodas.Columns)
                {
                    Console.Write("\t " + row[col].ToString());
                }
                Console.WriteLine();
            }*/
            
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }

        private static DataTable ProcessTables(List<System.Data.DataTable> tabelas, string namecolumn)
        {
            DataTable todas = new DataTable();
          /*  DataTable G = ExcelLoader.bla(tabelas[0], "Energy");
            DataTable H = ExcelLoader.bla(tabelas[1], "Energy");
            DataTable I = ExcelLoader.bla(tabelas[2], "Energy");
            DataTable J = ExcelLoader.bla(tabelas[3], "Energy");
            DataTable K = ExcelLoader.bla(tabelas[4], "Energy");
            todas.Merge(J);
            todas.Merge(I);
            todas.Merge(G);
            todas.Merge(H);
            todas.Merge(K);*/
            foreach (DataTable tabela in tabelas)
            {
             
                //int[] energy = ExcelLoader.GetColumnInt(tabela, "Energy");
                //foreach (int i in energy) Console.WriteLine(i);
                System.Data.DataTable test = ExcelLoader.bla(tabela, namecolumn);
               // ExcelLoader.CountDistValues2(test, "Energy");
                todas.Merge(test);

                /***********************************/
                /*System.Data.DataTable orderedtable = ExcelLoader.OrderDataTable(test);

                Categorization newcolumn = new Categorization(orderedtable);
                List<List<int>> dividing = newcolumn.DividingCat(4);

                foreach (List<int> l in dividing)
                {
                    Console.Write("category: ");
                    List<int> li = l;
                    for (int i = 0; i < l.Count; i++)
                        Console.Write(", {0}",li[i]);

                    Console.WriteLine(" end");
                }*/
                /**********************************************/
               // List<Int32> energy = ExcelLoader.GetColumnInt(tabela, "Energy");
               /* foreach (int i in energy)
                {
                    //if (!String.IsNullOrEmpty(i))
                        Console.WriteLine(i);
                }

                Console.WriteLine("DISCTINT");
                Dictionary<string, int> query = ExcelLoader.CountDistValues(tabela, "Energy");
                string max = ExcelLoader.MaxFrequency(query);

                Console.WriteLine("max value is: {0}", query[max]);
                foreach(var i in query)
                Console.WriteLine(i);
                //string[] ble = ExcelLoader.bla(tabela, "Energy");
                //foreach (string i in ble)
                //{
                    //if (!String.IsNullOrEmpty(i))
                 //   Console.WriteLine(i);
               // } */
                
            }
            
            return todas;
        }

        private static DataTable ProcessColumns(List<System.Data.DataTable> tabelas)
        {
            //DataTable todas = new DataTable();
            DataTable table = new DataTable();

            table.Columns.Add("Energy", typeof(int));
            table.Columns.Add("Money", typeof(int));
            table.Columns.Add("Oil", typeof(int));
            table.Columns.Add("ScoreEco", typeof(int));
            table.Columns.Add("ScoreEnv", typeof(int));
            table.Columns.Add("ScoreMay", typeof(int));
            table.Columns.Add("Level", typeof(int));
            table.Columns.Add("Population", typeof(int));
            table.Columns.Add("StrategyMay", typeof(string));

            TestTable.Columns.Add("Energy", typeof(int));
            TestTable.Columns.Add("Money", typeof(int));
            TestTable.Columns.Add("Oil", typeof(int));
            TestTable.Columns.Add("ScoreEco", typeof(int));
            TestTable.Columns.Add("ScoreEnv", typeof(int));
            TestTable.Columns.Add("ScoreMay", typeof(int));
            TestTable.Columns.Add("Level", typeof(int));
            TestTable.Columns.Add("Population", typeof(int));
            TestTable.Columns.Add("StrategyMay", typeof(string));

            
            foreach (DataTable tabela in tabelas)
            {
                IEnumerable<DataRow> test = from e in tabela.AsEnumerable()
                                            where !e.IsNull("Energy")
                                            where !e.IsNull("Money")
                                            where !e.IsNull("Oil")
                                            where !e.IsNull("ScoreEco")
                                            where !e.IsNull("ScoreEnv")
                                            where !e.IsNull("ScoreMay")
                                            where !e.IsNull("Level")
                                            where !e.IsNull("Population")
                                            where !e.IsNull("StrategyMay")
                                            select e;
                int numbRows = 5;
                int auxRows = numbRows;
                foreach (var q in test)
                {
                    auxRows--;
                    if (auxRows == 0)
                    {
                        TestTable.Rows.Add(Convert.ToInt32(q["Energy"]), Convert.ToInt32(q["Money"]), Convert.ToInt32(q["Oil"]), Convert.ToInt32(q["ScoreEco"]),
                        Convert.ToInt32(q["ScoreEnv"]), Convert.ToInt32(q["ScoreMay"]), Convert.ToInt32(q["Level"]), Convert.ToInt32(q["Population"]),
                        q["StrategyMay"]);

                        auxRows = numbRows;
                    }

                    else
                        table.Rows.Add(Convert.ToInt32(q["Energy"]), Convert.ToInt32(q["Money"]), Convert.ToInt32(q["Oil"]), Convert.ToInt32(q["ScoreEco"]),
                            Convert.ToInt32(q["ScoreEnv"]), Convert.ToInt32(q["ScoreMay"]), Convert.ToInt32(q["Level"]), Convert.ToInt32(q["Population"]),
                            q["StrategyMay"]);
                   // Console.WriteLine(q["Energy"].ToString() + "  " + q["Money"].ToString());
                }
               
            }
            

           /* foreach (DataRow t in table.Rows)
                Console.WriteLine(t["Energy"].ToString() + "  " + t["Money"].ToString() + "  " + t["Oil"].ToString() + " " +
                    t["ScooreEco"].ToString() + " " + t["ScooreEnv"].ToString() + " " + t["ScooreMay"].ToString() + " " +
                    t["Level"].ToString() + " " + t["Population"].ToString() + " " + t["StrategyMay"].ToString());
            */
            return table;
        }
    }
}
