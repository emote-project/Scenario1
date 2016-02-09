using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmoteEnercitiesMessages;
using System.Data;
using EmoteEvents;

namespace CollaborativeStrategySelector
{
    public class StrategySelector
    {
        Dictionary<string, DecisionTree.Categorization> categoriesMap = new Dictionary<string, DecisionTree.Categorization>();
        DataTable jogo;
        DecisionTree.TreeNode finalTree;
        DecisionTree.StrategyDecision strategySelector; 
        DataTable gameScores = new DataTable();

        public StrategySelector(string dtDataExcelFile)
        {
            Dictionary<string, System.Data.DataTable> database;
            try
            {
                database = DecisionTree.ExcelLoader.ImportExcelXLS(dtDataExcelFile, false);
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

            //Categorize
            DataTable tabelasTodasEnergy = ProcessTables(tabelas, "Energy");
            DataTable orderedtableEnergy = DecisionTree.ExcelLoader.OrderDataTable(DecisionTree.ExcelLoader.CountDistValues2(tabelasTodasEnergy, "Energy"));

            DataTable tabelasTodasMoney = ProcessTables(tabelas, "Money");
            DataTable orderedtableMoney = DecisionTree.ExcelLoader.OrderDataTable(DecisionTree.ExcelLoader.CountDistValues2(tabelasTodasMoney, "Money"));

            DataTable tabelasTodasOil = ProcessTables(tabelas, "Oil");
            DataTable orderedtableOil = DecisionTree.ExcelLoader.OrderDataTable(DecisionTree.ExcelLoader.CountDistValues2(tabelasTodasOil, "Oil"));

            DataTable tabelasTodasScoreEco = ProcessTables(tabelas, "ScoreEco");
            DataTable orderedtableScoreEco = DecisionTree.ExcelLoader.OrderDataTable(DecisionTree.ExcelLoader.CountDistValues2(tabelasTodasScoreEco, "ScoreEco"));

            DataTable tabelasTodasScoreEnv = ProcessTables(tabelas, "ScoreEnv");
            DataTable orderedtableScoreEnv = DecisionTree.ExcelLoader.OrderDataTable(DecisionTree.ExcelLoader.CountDistValues2(tabelasTodasScoreEnv, "ScoreEnv"));

            DataTable tabelasTodasScoreMay = ProcessTables(tabelas, "ScoreMay");
            DataTable orderedtableScoreMay = DecisionTree.ExcelLoader.OrderDataTable(DecisionTree.ExcelLoader.CountDistValues2(tabelasTodasScoreMay, "ScoreMay"));

            DataTable tabelasTodasPopulation = ProcessTables(tabelas, "Population");
            DataTable orderedtablePopulation = DecisionTree.ExcelLoader.OrderDataTable(DecisionTree.ExcelLoader.CountDistValues2(tabelasTodasPopulation, "Population"));

            DecisionTree.Categorization energycolumn = new DecisionTree.Categorization(orderedtableEnergy, "Energy", 6);
            DecisionTree.Categorization moneycolumn = new DecisionTree.Categorization(orderedtableMoney, "Money", 5);
            DecisionTree.Categorization oilcolumn = new DecisionTree.Categorization(orderedtableOil, "Oil", 6);
            DecisionTree.Categorization ScoreEcocolumn = new DecisionTree.Categorization(orderedtableScoreEco, "ScoreEco", 4);
            DecisionTree.Categorization ScoreEnvcolumn = new DecisionTree.Categorization(orderedtableScoreEnv, "ScoreEnv", 6);
            DecisionTree.Categorization ScoreMaycolumn = new DecisionTree.Categorization(orderedtableScoreMay, "ScoreMay", 4);
            DecisionTree.Categorization populationcolumn = new DecisionTree.Categorization(orderedtablePopulation, "Population", 6);

            List<List<int>> levelManualCat = new List<List<int>>(){ {new List<int>(){1,1}}, {new List<int>(){2,2}}, 
                                                                    {new List<int>(){3,3}}, {new List<int>(){4,4}} };
            DecisionTree.Categorization levelcolumn = new DecisionTree.Categorization(levelManualCat, "Level");

            categoriesMap.Add("Energy", energycolumn);
            categoriesMap.Add("Money", moneycolumn);
            categoriesMap.Add("Oil", oilcolumn);
            categoriesMap.Add("ScoreEco", ScoreEcocolumn);
            categoriesMap.Add("ScoreEnv", ScoreEnvcolumn);
            categoriesMap.Add("ScoreMay", ScoreMaycolumn);
            categoriesMap.Add("Level", levelcolumn);
            categoriesMap.Add("Population", populationcolumn);

            jogo = ProcessColumns(tabelas);

            foreach (DataRow jogada in jogo.Rows)
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

            DecisionTree.DecisionTreeImplementation sam = new DecisionTree.DecisionTreeImplementation();

            finalTree = sam.GetTree(jogo);
            strategySelector = new DecisionTree.StrategyDecision(finalTree, categoriesMap);

            gameScores.Columns.Add("Energy", typeof(int));
            gameScores.Columns.Add("Money", typeof(int));
            gameScores.Columns.Add("Oil", typeof(int));
            gameScores.Columns.Add("ScoreEco", typeof(int));
            gameScores.Columns.Add("ScoreEnv", typeof(int));
            gameScores.Columns.Add("ScoreMay", typeof(int));
            gameScores.Columns.Add("Level", typeof(int));
            gameScores.Columns.Add("Population", typeof(int));
                                                                                        
        }

        public EmoteEnercitiesMessages.EnercitiesStrategy GetStrategy(EnercitiesGameInfo gameState)
        {
            int energy = Convert.ToInt32(gameState.PowerProduction - gameState.PowerConsumption);
            gameScores.Rows.Add(Convert.ToInt32(energy), Convert.ToInt32(gameState.Money), Convert.ToInt32(gameState.Oil), 
                                Convert.ToInt32(gameState.EconomyScore), Convert.ToInt32(gameState.EnvironmentScore), 
                                Convert.ToInt32(gameState.CurrentRole), Convert.ToInt32(gameState.Level), Convert.ToInt32(gameState.Population));
            
            DataRow currentState = gameScores.Rows[gameScores.Rows.Count - 1];

            string strategy = strategySelector.getStrategy(currentState);
            Console.WriteLine("strategy: " + strategy);

            if (strategy.Equals("S1"))
                return EnercitiesStrategy.Economist;
            if (strategy.Equals("S2"))
                return EnercitiesStrategy.Environmentalist;
            if (strategy.Equals("S3"))
                return EnercitiesStrategy.Mayor;
            else
                return EnercitiesStrategy.Global;
        }

        private static DataTable ProcessTables(List<System.Data.DataTable> tabelas, string namecolumn)
        {
            DataTable todas = new DataTable();
            foreach (DataTable tabela in tabelas)
            {
                System.Data.DataTable test = DecisionTree.ExcelLoader.bla(tabela, namecolumn);
                todas.Merge(test);

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
                foreach (var q in test)
                {
                    table.Rows.Add(Convert.ToInt32(q["Energy"]), Convert.ToInt32(q["Money"]), Convert.ToInt32(q["Oil"]), Convert.ToInt32(q["ScoreEco"]),
                                    Convert.ToInt32(q["ScoreEnv"]), Convert.ToInt32(q["ScoreMay"]), Convert.ToInt32(q["Level"]), Convert.ToInt32(q["Population"]),
                                    q["StrategyMay"]);
                }

            }
            return table;
        }
    }
}
