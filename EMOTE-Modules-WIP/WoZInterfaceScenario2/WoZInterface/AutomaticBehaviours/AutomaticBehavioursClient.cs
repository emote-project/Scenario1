using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;

namespace WOZInterface.AutomaticBehaviours
{
    public interface IAutomaticBehavioursPublisher : IThalamusPublisher,
        EmoteCommonMessages.IFMLSpeech,
        Thalamus.BML.IAnimationActions
    { }

    public interface IAutomaticBehavioursClient :
        EmoteEnercitiesMessages.IEnercitiesGameStateEvents,
        EmoteEnercitiesMessages.IEnercitiesTaskEvents,
        EmoteEnercitiesMessages.IEnercitiesAIActions,
        EmoteEnercitiesMessages.IEnercitiesAIPerceptions,
        Thalamus.BML.IAnimationEvents
    { }

    class AutomaticBehavioursPublisher : IAutomaticBehavioursPublisher
    {
        dynamic publisher;

        public AutomaticBehavioursPublisher(dynamic publisher)
        {
            this.publisher = publisher;
        }

        public void PerformUtteranceFromLibrary(string id, string category, string subcategory, string[] tagNames, string[] tagValues)
        {
            publisher.PerformUtteranceFromLibrary(id, category, subcategory, tagNames, tagValues);
        }

        public void PlayAnimation(string id, string animation)
        {
            publisher.PlayAnimation(id, animation);
        }

        #region NOT USED
        public void CancelUtterance(string id)
        {
        }

        public void PerformUtterance(string id, string utterance, string category)
        {
        }

        public void PlayAnimationQueued(string id, string animation)
        {
        }

        public void StopAnimation(string id)
        {
        }

        void EmoteCommonMessages.IFMLSpeech.CancelUtterance(string id)
        {
            throw new NotImplementedException();
        }

        void EmoteCommonMessages.IFMLSpeech.PerformUtterance(string id, string utterance, string category)
        {
            throw new NotImplementedException();
        }

        void EmoteCommonMessages.IFMLSpeech.PerformUtteranceFromLibrary(string id, string category, string subcategory, string[] tagNames, string[] tagValues)
        {
            throw new NotImplementedException();
        }

        void Thalamus.BML.IAnimationActions.PlayAnimation(string id, string animation)
        {
            throw new NotImplementedException();
        }

        void Thalamus.BML.IAnimationActions.PlayAnimationQueued(string id, string animation)
        {
            throw new NotImplementedException();
        }

        void Thalamus.BML.IAnimationActions.StopAnimation(string id)
        {
            throw new NotImplementedException();
        }
        #endregion

        
    }

    class AutomaticBehavioursClient : ThalamusClient, IAutomaticBehavioursClient
    {
        AutomaticBehaviours ab;
        private IAutomaticBehavioursPublisher wozPublisher;
        public IAutomaticBehavioursPublisher WOZPublisher
        {
            get
            {
                return wozPublisher;
            }
        }

        public AutomaticBehavioursClient(AutomaticBehaviours automaticBehaviours, string thalamusCharacterName)
            : base("AutomaticBehavioursClient", thalamusCharacterName)
        {
            SetPublisher<IAutomaticBehavioursPublisher>();
            wozPublisher = new AutomaticBehavioursPublisher(Publisher);
            ab = automaticBehaviours;
        }


        public void ResumeGame(string player1name, string player1role, string player2name, string player2role, string serializedGameState)
        {
        }

        public void TurnChanged(string serializedGameState)
        {
            ab.TurnChangedEvent(EmoteEvents.EnercitiesGameInfo.DeserializeFromJson(serializedGameState));
        }

        public void ConfirmConstruction(EmoteEnercitiesMessages.StructureType structure, string translation, int cellX, int cellY)
        {
            ab.GameActionPlayed(EmoteEnercitiesMessages.ActionType.BuildStructure, (int)structure, translation);
        }

        public void ImplementPolicy(EmoteEnercitiesMessages.PolicyType policy, string translation)
        {
            ab.GameActionPlayed(EmoteEnercitiesMessages.ActionType.ImplementPolicy, (int)policy, translation);
        }

        public void PerformUpgrade(EmoteEnercitiesMessages.UpgradeType upgrade, string translation, int cellX, int cellY)
        {
            ab.GameActionPlayed(EmoteEnercitiesMessages.ActionType.UpgradeStructure, (int)upgrade, translation);
        }

        public void SkipTurn()
        {
        }


        public void BestActionsPlanned(string[] EnercitiesActionInfo_actionInfos)
        {
            ab.BestActionsPlannedEvent();
        }

        public void UpdateStrategies(string StrategiesSet_strategies)
        {
            ab.StrategiesUpdatedEvent();
        }

        public void AnimationFinished(string id)
        {
            ab.AnimationFinishedEvent(id);
        }

        #region NOT USED
        public void StrategiesUpdated(string StrategiesSet_strategies)
        {
        }

        public void AnimationStarted(string id)
        {
        }

        public void StrategyGameMoves(string environmentalistMove, string economistMove, string mayorMove, string globalMove)
        {
        }

        public void ReachedNewLevel(int level)
        {
        }

        public void ExamineCell(double screenX, double screenY, int cellX, int cellY, EmoteEnercitiesMessages.StructureType StructureType_structure, string StructureType_translated)
        {
        }

        public void BestActionPlanned(string[] EnercitiesActionInfo_actionInfos)
        {
        }

        public void PredictedValuesUpdated(double[] values)
        {
        }

        public void EndGameNoOil(int totalScore)
        {
        }

        public void EndGameSuccessfull(int totalScore)
        {
        }

        public void EndGameTimeOut(int totalScore)
        {
        }

        public void GameStarted(string player1name, string player1role, string player2name, string player2role)
        {
        }

        public void PlayersGender(EmoteEnercitiesMessages.Gender player1gender, EmoteEnercitiesMessages.Gender player2gender)
        {
        }

        public void PlayersGenderString(string player1gender, string player2gender)
        {
        }
        #endregion 
    
       

        
    
        
    }
}
