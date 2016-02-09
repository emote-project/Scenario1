using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using EmoteEvents.ComplexData;
using Thalamus;

namespace S2Wrapper
{
    public interface IS2WrapperPublisher : EmoteCommonMessages.IFMLSpeech, EmoteEnercitiesMessages.IEnercitiesTaskActions
    {}

    public interface IS2Wrapper : EmoteCommonMessages.IEmoteActions, EmoteEnercitiesMessages.IEnercitiesGameStateEvents
    {}

    public class S2WrapperClient : ThalamusClient, IS2Wrapper
    {
        public class S2WrapperPublisher : IS2WrapperPublisher, IThalamusPublisher
        {
            dynamic publisher;
            public S2WrapperPublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }

            public void CancelUtterance(string id)
            {
                publisher.CancelUtterance(id);
            }

            public void PerformUtterance(string id, string utterance, string category)
            {
                publisher.PerformUtterance(id, utterance, category);
            }

            public void PerformUtteranceFromLibrary(string id, string category, string subcategory, string[] tagNames, string[] tagValues)
            {
                publisher.PerformUtteranceFromLibrary(id, category, subcategory, tagNames, tagValues);
            }

            public void ConfirmConstruction(EmoteEnercitiesMessages.StructureType structure, int cellX, int cellY)
            {
                throw new NotImplementedException();
            }

            public void ImplementPolicy(EmoteEnercitiesMessages.PolicyType policy)
            {
                throw new NotImplementedException();
            }

            public void PerformUpgrade(EmoteEnercitiesMessages.UpgradeType upgrade, int cellX, int cellY)
            {
                throw new NotImplementedException();
            }

            public void PlayStrategy(EmoteEnercitiesMessages.EnercitiesStrategy strategy)
            {
                throw new NotImplementedException();
            }

            public void SkipTurn()
            {
                publisher.SkipTurn();
            }
        }

        S2WrapperPublisher S2WPublisher;

        int sessionId = 1;
        string ecoName = "";
        string envName = "";
        public int SessionId
        {
            get { return sessionId; }
            set { sessionId = value; Debug("Set SessionID to {0}", value); }
        }

        public S2WrapperClient(string character)
            : base("Scenario2 Wrapper", character)
        {
            SetPublisher<IS2WrapperPublisher>();
            S2WPublisher = new S2WrapperPublisher(Publisher);
        }



        public void EndGame(string cat, string subcat)
        {
            Debug("Performing EndGame({0}, {1})", cat, subcat);
            S2WPublisher.PerformUtteranceFromLibrary("", cat, subcat, new string[] { }, new string[] { });
            Thread.Sleep(1000);
            S2WPublisher.PerformUtteranceFromLibrary("", "wrapup", (sessionId -1).ToString(), new string[] { "/ecoName/", "/envName/" }, new string[] { ecoName, envName });
        }

        void EmoteEnercitiesMessages.IEnercitiesGameStateEvents.GameStarted(string player1Name, string player1Role, string player2Name, string player2Role)
        {
            Debug("Performing GameStarted");
            Thread.Sleep(10000);
            Debug("Skipping turn");
            S2WPublisher.SkipTurn();
        }

        void EmoteCommonMessages.IEmoteActions.Start(string StartMessageInfo_info)
        {
            Debug("Got start message.");
            StartMessageInfo smi = StartMessageInfo.DeserializeFromJson<StartMessageInfo>(StartMessageInfo_info);
            SessionId = smi.SessionId;
        }

        void EmoteEnercitiesMessages.IEnercitiesGameStateEvents.EndGameNoOil(int totalScore)
        {
            EndGame("endgame", "nooil:positive:indifferent");
        }

        void EmoteEnercitiesMessages.IEnercitiesGameStateEvents.EndGameSuccessfull(int totalScore)
        {
            EndGame("endgame", "successful:positive:indifferent");
        }

        void EmoteEnercitiesMessages.IEnercitiesGameStateEvents.EndGameTimeOut(int totalScore)
        {
            EndGame("gamestatus", "timeup:positive:indifferent");
        }



        void EmoteCommonMessages.IEmoteActions.Reset()
        {
        }

        void EmoteCommonMessages.IEmoteActions.SetLearnerInfo(string LearnerInfo_learnerInfo)
        {
        }

        void EmoteCommonMessages.IEmoteActions.Stop()
        {
        }

        void EmoteEnercitiesMessages.IEnercitiesGameStateEvents.PlayersGender(EmoteEnercitiesMessages.Gender player1Gender, EmoteEnercitiesMessages.Gender player2Gender)
        {
        }

        void EmoteEnercitiesMessages.IEnercitiesGameStateEvents.PlayersGenderString(string player1Gender, string player2Gender)
        {
        }

        void EmoteEnercitiesMessages.IEnercitiesGameStateEvents.ReachedNewLevel(int level)
        {
        }

        void EmoteEnercitiesMessages.IEnercitiesGameStateEvents.ResumeGame(string player1Name, string player1Role, string player2Name, string player2Role, string serializedGameState)
        {
        }

        void EmoteEnercitiesMessages.IEnercitiesGameStateEvents.StrategyGameMoves(string environmentalistMove, string economistMove, string mayorMove, string globalMove)
        {
        }

        void EmoteEnercitiesMessages.IEnercitiesGameStateEvents.TurnChanged(string serializedGameState)
        {
        }
    }
}
