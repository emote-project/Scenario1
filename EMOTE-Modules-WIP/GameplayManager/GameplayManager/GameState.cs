using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EmoteEnercitiesMessages;
using EmoteEvents;

namespace GameplayManager
{
    public class EnercitiesGamestate
    {
        private GamePlayManager GPM;

        public Player Player1;
        public Player Player2;
        public Player PlayerAI = new Player("Nao", EnercitiesRole.Mayor);
        public Player CurrentPlayer = new Player("", EnercitiesRole.Mayor);

        public Player Economist;
        public Player Environmentalist;

        public PolicyType LastPolicy;
        public string LastPolicyTranslation = "";
        public StructureType LastStructure;
        public string LastStructureTranslation = "";
        public StructureCategory LastStructureCategory;
        public string LastStructureCategoryTranslation = "";
        public UpgradeType LastUpgrade;
        public string LastUpgradeTranslation = "";

        // Used to control the game flow
        public bool IsDoingGameStartPresentation = false; // true when doing the game start presentation
        public bool GameStartPresentationEnded = false; // true after Nao ends all the presentation at the beginning of the game
        public int NumberOfUpgradesDoneThisTurn = 0;    // each turn it's possible to do 3 upgrades. When the AI starts to do upgrades, it should do 3 of them. This counter helps to track how many upgrades were done.

        public override string ToString()
        {
            return string.Format("CurrentPlayer:{0}, LastPolicy:{1}, LastStructure:{2}, LastStructureCategory:{3}, LastUpgrade:{4}, EnercitiesInfo:{5}", CurrentPlayer.ToString(), LastPolicy, LastStructure, LastStructureCategory, LastUpgrade, EnercitiesInfo.ToString());
        }

        public EnercitiesGameInfo EnercitiesInfo { get; private set; }

        private Dictionary<EnercitiesRole, Player> PlayerRoles = new Dictionary<EnercitiesRole, Player>();

        public EnercitiesGamestate(GamePlayManager gpm, string player1Name, EnercitiesRole player1Role, string player2Name, EnercitiesRole player2Role)
        {
            GPM = gpm;
            Player1 = new Player(player1Name, player1Role);
            Player2 = new Player(player2Name, player2Role);
            if (player1Role == EnercitiesRole.Economist) Economist = Player1;
            else Environmentalist = Player1;
            if (player2Role == EnercitiesRole.Economist) Economist = Player2;
            else Environmentalist = Player2;

            PlayerRoles[player1Role] = Player1;
            PlayerRoles[player2Role] = Player2;
            List<EnercitiesRole> roles = new List<EnercitiesRole>() { EnercitiesRole.Economist, EnercitiesRole.Environmentalist, EnercitiesRole.Mayor };
            roles.Remove(player1Role);
            roles.Remove(player2Role);
            foreach (EnercitiesRole r in roles) PlayerRoles[r] = PlayerAI;
            EnercitiesInfo = new EnercitiesGameInfo();
        }

        public void Update(EnercitiesGameInfo gameState)
        {
            EnercitiesInfo = gameState;
            CurrentPlayer = PlayerRoles[gameState.CurrentRole];
            Update(gameState.Level, gameState.Population, gameState.TargetPopulation, gameState.Money,
                gameState.Oil, gameState.PowerConsumption, gameState.PowerProduction, gameState.EnvironmentScore, gameState.EconomyScore,
                gameState.WellbeingScore, gameState.GlobalScore, gameState.CurrentRole);
        }
        public void Update(int level, int population, int targetPopulation,
            double money, double oil, double powerConsumption, double powerProduction,
            double environmentScore, double economyScore, double wellbeingScore, double globalScore, EnercitiesRole currentRole)
        {
            if (PlayerRoles.ContainsKey(currentRole))
            {
                EnercitiesInfo.Level = level+1;
                EnercitiesInfo.Population = population;
                EnercitiesInfo.TargetPopulation = targetPopulation;
                EnercitiesInfo.Money = money;
                EnercitiesInfo.Oil = oil;
                EnercitiesInfo.PowerConsumption = powerConsumption;
                EnercitiesInfo.PowerProduction = powerProduction;
                EnercitiesInfo.EnvironmentScore = environmentScore;
                EnercitiesInfo.EconomyScore = economyScore;
                EnercitiesInfo.WellbeingScore = wellbeingScore;
                EnercitiesInfo.GlobalScore = globalScore;
                EnercitiesInfo.CurrentRole = currentRole;
            }
            CurrentPlayer = PlayerRoles[currentRole];
            switch (currentRole)
            {
                case EnercitiesRole.Economist:
                    GPM.GamePublisher.TargetLink("currentPlayerGUI", "ecoGUI");
                    break;
                case EnercitiesRole.Environmentalist:
                    GPM.GamePublisher.TargetLink("currentPlayerGUI", "envGUI");
                    break;
                case EnercitiesRole.Mayor:
                    GPM.GamePublisher.TargetLink("currentPlayerGUI", "mayGUI");
                    break;
            }
        }

        

        public string GetCurrentPlayerSide()
        {
            if (CurrentPlayer != null)
            {
                if (CurrentPlayer == Player1) return "Left";
                else return "Right";
            }
            else
                return "";
        }

    }
}
