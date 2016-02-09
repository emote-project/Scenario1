using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;

namespace WOZInterface.GameStatus
{
    

    class GameStatusClient
    {
        static GameStatusClient instance;

        GameStatus statusWindow;
        Scores scoresWindow;

        private GameStatusClient()
        {
            WOZSettingsWindow.SettingsChanged += settingsWindow_SettingsChanged;
            WoZOptimusPrimeMegaClient.GetInstance().TurnChangedEvent += GameStatusClient_TurnChangedEvent;
            WoZOptimusPrimeMegaClient.GetInstance().SkipTurnEvent += GameStatusClient_SkipTurnEvent;
            WoZOptimusPrimeMegaClient.GetInstance().ConfirmConstructionEvent += GameStatusClient_ConfirmConstructionEvent;
            WoZOptimusPrimeMegaClient.GetInstance().ExamineCellEvent += GameStatusClient_ExamineCellEvent;
            WoZOptimusPrimeMegaClient.GetInstance().ImplementPolicyEvent += GameStatusClient_ImplementPolicyEvent;
            WoZOptimusPrimeMegaClient.GetInstance().PerformUpgradeEvent += GameStatusClient_PerformUpgradeEvent;
            WoZOptimusPrimeMegaClient.GetInstance().GameStartedEvent += GameStatusClient_GameStartedEvent;
        }

        void GameStatusClient_GameStartedEvent(object sender, WoZOptimusPrimeMegaClient.GenericGameEventArgs e)
        {
            statusWindow.Reset();
        }

        void GameStatusClient_PerformUpgradeEvent(object sender, WoZOptimusPrimeMegaClient.GameActionEventArgs e)
        {
            if (statusWindow != null) statusWindow.AddAction("Upgraded: " + ((EmoteEnercitiesMessages.UpgradeType)e.ActionTypeEnum).ToString());
        }

        void GameStatusClient_ImplementPolicyEvent(object sender, WoZOptimusPrimeMegaClient.GameActionEventArgs e)
        {
            if (statusWindow != null) statusWindow.AddAction("Implement: " + ((EmoteEnercitiesMessages.StructureType)e.ActionTypeEnum).ToString());
        }

        void GameStatusClient_ExamineCellEvent(object sender, WoZOptimusPrimeMegaClient.ExamineCellEventArgs e)
        {
            if (statusWindow != null) statusWindow.AddAction("Examined a cell for building a <" + e.StructureType_structure + "> structure");
        }

        void GameStatusClient_ConfirmConstructionEvent(object sender, WoZOptimusPrimeMegaClient.GameActionEventArgs e)
        {
            if (statusWindow != null) statusWindow.AddAction("Built: " + (EmoteEnercitiesMessages.StructureType)e.ActionTypeEnum);
        }

        void GameStatusClient_SkipTurnEvent(object sender, EventArgs e)
        {
            if (statusWindow != null) statusWindow.AddAction("Skipped this turn");
        }

        void GameStatusClient_TurnChangedEvent(object sender, WoZOptimusPrimeMegaClient.GenericGameEventArgs e)
        {
            if (statusWindow != null) statusWindow.StatusUpdate(EmoteEvents.EnercitiesGameInfo.DeserializeFromJson(e.SerializedGameState));
            if (scoresWindow != null) scoresWindow.StatusUpdate(EmoteEvents.EnercitiesGameInfo.DeserializeFromJson(e.SerializedGameState));
        }

        void settingsWindow_SettingsChanged(object sender, WOZSettingsEventArg e)
        {
            WoZOptimusPrimeMegaClient.GetInstance().WOZPublisher.PlayersGender(
                e.Settings.EnvGender == 1 ? EmoteEnercitiesMessages.Gender.Male : EmoteEnercitiesMessages.Gender.Female,
                e.Settings.EcoGender == 1 ? EmoteEnercitiesMessages.Gender.Male : EmoteEnercitiesMessages.Gender.Female);
        }

        public static GameStatusClient Instance(GameStatus window) 
        {
            if (instance==null){
                instance = new GameStatusClient();
            }
            instance.statusWindow = window;
            return instance;
        }

        public static GameStatusClient Instance(Scores window, string thalamusCharacterName)
        {
            if (instance==null){
                instance = new GameStatusClient();
            }
            instance.scoresWindow = window;
            return instance;
        }


    }
}
