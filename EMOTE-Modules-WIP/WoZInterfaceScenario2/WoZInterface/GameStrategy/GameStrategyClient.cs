using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WOZInterface.GameStrategy.Common;
using EmoteEnercitiesMessages;
using EmoteEvents;

namespace WOZInterface.GameStrategy
{

    class GameStrategyClient
    {
        GameStrategy window;

        public GameStrategyClient(GameStrategy window) 
        {
            this.window = window;
            WoZOptimusPrimeMegaClient.GetInstance().StrategiesUpdatedEvent += GameStrategyClient_StrategiesUpdatedEvent;
            WoZOptimusPrimeMegaClient.GetInstance().GameStartedEvent += GameStrategyClient_GameStartedEvent;
            WoZOptimusPrimeMegaClient.GetInstance().BestActionsPlannedEvent += GameStrategyClient_BestActionsPlannedEvent;
            WoZOptimusPrimeMegaClient.GetInstance().TurnChangedEvent += GameStrategyClient_TurnChangedEvent;
        }

        void GameStrategyClient_TurnChangedEvent(object sender, WoZOptimusPrimeMegaClient.GenericGameEventArgs e)
        {
            window.TurnChanged(EnercitiesGameInfo.DeserializeFromJson(e.SerializedGameState));
        }

        void GameStrategyClient_BestActionsPlannedEvent(object sender, WoZOptimusPrimeMegaClient.IAEventArgs e)
        {
            window.UpdateBestActionList(e.EnercitiesActionInfo_actionInfos);
        }

        void GameStrategyClient_GameStartedEvent(object sender, WoZOptimusPrimeMegaClient.GenericGameEventArgs e)
        {
            window.Reset();
        }

        void GameStrategyClient_StrategiesUpdatedEvent(object sender, WoZOptimusPrimeMegaClient.IAEventArgs e)
        {
            StrategiesSet strategiesSet = StrategiesSet.DeserializeFromJson(e.StrategiesSet_strategies);
            window.StrategyUpdateByAI(strategiesSet.Strategies);
        }
        
    }
}
