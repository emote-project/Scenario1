using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;
/*
namespace WOZInterface
{
    public interface WozThalamusClientActions :  
        EmoteEnercitiesMessages.IEnercitiesGameStateEvents, 
        EmoteEnercitiesMessages.IEnercitiesExamineEvents, 
        EmoteEnercitiesMessages.IEnercitiesTaskEvents
    { }

    public interface WozThalamusClientPublisher : EmoteCommonMessages.IFMLSpeech, EmoteCommonMessages.IGazeStateActions
    { }

    public class WozThalamusClient : ThalamusClient, WozThalamusClientActions
    {
        public class WozPublisher : WozThalamusClientPublisher
        {
            dynamic publisher;

            public WozPublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }


            public void PerformUtterance(string utterance)
            {
                publisher.PerformUtterance(utterance);
            }

            public void GazeAtPerson()
            {
                publisher.GazeAtPerson();
            }

            public void GazeAtScreen(string id, double x, double y)
            {
                publisher.GazeAtScreen(id, x, y);
            }

            public void GazeAtScreenClicks()
            {
                publisher.GazeAtSceenClicks();
            }

            public void GazeAtTarget(string targetName)
            {
                publisher.GazeAtTarget(targetName);
            }
        }

        public class GameStatusUpdateEventArgs : EventArgs
        {
            public EmoteEnercitiesMessages.EnercitiesGameInfo GameInfo { get; set; }
            public GameStatusUpdateEventArgs(EmoteEnercitiesMessages.EnercitiesGameInfo gameInfo)
            {
                GameInfo = gameInfo;
            }
        }
        public event EventHandler<GameStatusUpdateEventArgs> GameStatusUpdate;
        public WozPublisher wozPublisher;


        public WozThalamusClient(string thalamusCharacterName) : base("WozThalamusClient",thalamusCharacterName)
        {
            SetPublisher<WozThalamusClientPublisher>();
            wozPublisher = new WozPublisher(Publisher);
        }


        public void EndGameNoOil(int totalScore)
        {
            throw new NotImplementedException();
        }

        public void EndGameSuccessfull(int totalScore)
        {
            throw new NotImplementedException();
        }

        public void EndGameTimeOut(int totalScore)
        {
            throw new NotImplementedException();
        }

        public void GameStarted(string player1name, string player1role, string player2name, string player2role)
        {
            throw new NotImplementedException();
        }

        public void ReachedNewLevel(int level)
        {
            throw new NotImplementedException();
        }

        public void TurnChanged(string serializedGameState)
        {
            GameStatusUpdate(this, new GameStatusUpdateEventArgs(EmoteEnercitiesMessages.EnercitiesGameInfo.DeserializeFromJson(serializedGameState)));
        }

        public void BuildMenuTooltipClosed(EmoteEnercitiesMessages.StructureCategory category)
        {
            throw new NotImplementedException();
        }

        public void BuildMenuTooltipShowed(EmoteEnercitiesMessages.StructureCategory category)
        {
            throw new NotImplementedException();
        }

        public void BuildingMenuToolSelected(EmoteEnercitiesMessages.StructureType structure)
        {
            throw new NotImplementedException();
        }

        public void BuildingMenuToolUnselected(EmoteEnercitiesMessages.StructureType structure)
        {
            throw new NotImplementedException();
        }

        public void PoliciesMenuClosed()
        {
            throw new NotImplementedException();
        }

        public void PoliciesMenuShowed()
        {
            throw new NotImplementedException();
        }

        public void PolicyTooltipClosed()
        {
            throw new NotImplementedException();
        }

        public void PolicyTooltipShowed(EmoteEnercitiesMessages.PolicyType policy)
        {
            throw new NotImplementedException();
        }

        public void UpgradeTooltipClosed()
        {
            throw new NotImplementedException();
        }

        public void UpgradeTooltipShowed(EmoteEnercitiesMessages.UpgradeType upgrade)
        {
            throw new NotImplementedException();
        }

        public void UpgradesMenuClosed()
        {
            throw new NotImplementedException();
        }

        public void UpgradesMenuShowed()
        {
            throw new NotImplementedException();
        }

        public void ConfirmConstruction(EmoteEnercitiesMessages.StructureType structure, double x, double y)
        {
            throw new NotImplementedException();
        }

        public void ImplementPolicy(EmoteEnercitiesMessages.PolicyType policy)
        {
            throw new NotImplementedException();
        }

        public void PerformUpgrade(EmoteEnercitiesMessages.UpgradeType upgrade)
        {
            throw new NotImplementedException();
        }

        public void SkipTurn()
        {
            throw new NotImplementedException();
        }


        public void ResumeGame(string player1name, string player1role, string player2name, string player2role, string serializedGameState)
        {
            throw new NotImplementedException();
        }

        public void StrategyGameMoves(string environmentalistMove, string economistMove, string mayorMove, string globalMove)
        {
            throw new NotImplementedException();
        }
    }
}
*/