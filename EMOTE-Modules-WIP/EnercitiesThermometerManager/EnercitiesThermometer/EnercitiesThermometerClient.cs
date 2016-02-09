using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;
using EmoteEvents;
using EmoteCommonMessages;

namespace EnercitiesThermometer
{
    public interface IEnercitiesThermometerPublisher : IThalamusPublisher, EmoteEnercitiesMessages.IEnercitiesThermometerActions, ILearnerModelActions
    { }

    public interface IEnercitiesThermometerClient : EmoteEnercitiesMessages.IEnercitiesGameStateEvents 
    { }



    class EnercitiesThermometerClient : ThalamusClient, IEnercitiesThermometerClient
    {
        public class EnercitiesThermometerPublisher : IEnercitiesThermometerPublisher
        {
            dynamic publisher;

            public EnercitiesThermometerPublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }

            public void ThermometerAddRound(int quality)
            {
                publisher.ThermometerAddRound(quality);
            }

            public void ThermometerNewLevel()
            {
                publisher.ThermometerNewLevel();
            }

            public void learnerModelUpdate(string LearnerStateInfo_learnerState)
            {
                publisher.learnerModelUpdate(LearnerStateInfo_learnerState);
            }
        }

        public EnercitiesThermometerPublisher ETPublisher;

        private GameQualityEvaluator evaluator;

        public EnercitiesThermometerClient(string character = "") :
            base("EnercitiesThermometer", character) 
        {
            
            SetPublisher<IEnercitiesThermometerPublisher>();
            ETPublisher = new EnercitiesThermometerPublisher(Publisher);
            evaluator = new GameQualityEvaluator(this);
        }



        public void TurnChanged(string serializedGameState)
        {
            int evaluation = Convert.ToInt32(evaluator.GetNewEvaluation(serializedGameState));
            ETPublisher.ThermometerAddRound(evaluation);
        }

        public void ReachedNewLevel(int level)
        {
            ETPublisher.ThermometerNewLevel();
        }

        public void GameStarted(string player1name, string player1role, string player2name, string player2role)
        {
            evaluator.Reset();
        }

        #region NOT HANDLED
        public void EndGameNoOil(int totalScore)
        {
            
        }

        public void EndGameSuccessfull(int totalScore)
        {
            
        }

        public void EndGameTimeOut(int totalScore)
        {
            
        }


        public void PlayersGender(EmoteEnercitiesMessages.Gender player1gender, EmoteEnercitiesMessages.Gender player2gender)
        {
            
        }

        public void PlayersGenderString(string player1gender, string player2gender)
        {
            
        }

        

        public void ResumeGame(string player1name, string player1role, string player2name, string player2role, string serializedGameState)
        {
            
        }

        public void StrategyGameMoves(string environmentalistMove, string economistMove, string mayorMove, string globalMove)
        {

        }
        #endregion

    }
}
