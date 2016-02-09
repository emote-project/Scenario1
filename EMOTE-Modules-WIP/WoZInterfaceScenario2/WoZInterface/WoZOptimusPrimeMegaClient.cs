using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;

namespace WOZInterface
{
    public interface IFMLSpeechAux : IAction
    {
        void ReplaceTagsAndPerform(string utterance, string category);
    }

    public interface IWoZOptimusPrimeMegaPublisher : IThalamusPublisher, 
        EmoteEnercitiesMessages.IEnercitiesGameStateEvents,
        EmoteEnercitiesMessages.IEnercitiesAIPerceptions, 
        EmoteEnercitiesMessages.IEnercitiesTaskActions,
        EmoteCommonMessages.IGazeStateActions,
        Thalamus.BML.IHeadActions,
        EmoteCommonMessages.IFMLSpeech,
        IFMLSpeechAux,
        Thalamus.BML.IAnimationActions
    { }

    public interface IWoZOptimusPrimeMegaClient : 
        EmoteEnercitiesMessages.IEnercitiesGameStateEvents,
        EmoteEnercitiesMessages.IEnercitiesTaskEvents,
        EmoteEnercitiesMessages.IEnercitiesAIActions, 
        EmoteCommonMessages.ISpeechDetectionEvents,
        EmoteCommonMessages.ITargetEvents,
        EmoteCommonMessages.IPerceptionEvents, 
        EmoteCommonMessages.ISoundLocalizationEvents,
        Thalamus.BML.ISpeakEvents, 
        Thalamus.BML.ISpeakActions,
        Thalamus.BML.IAnimationEvents
    {}

    class WoZOptimusPrimeMegaPublisher : IWoZOptimusPrimeMegaPublisher
    {
        dynamic publisher;

        public WoZOptimusPrimeMegaPublisher(dynamic publisher)
        {
            this.publisher = publisher;
        }

        public void EndGameNoOil(int totalScore)
        {
            publisher.EndGameNoOil(totalScore);
        }

        public void EndGameSuccessfull(int totalScore)
        {
            publisher.EndGameSuccessfull(totalScore);
        }

        public void EndGameTimeOut(int totalScore)
        {
            publisher.EndGameTimeOut(totalScore);
        }

        public void GameStarted(string player1name, string player1role, string player2name, string player2role)
        {
            publisher.GameStarted(player1name, player1role, player2name, player2role);
        }

        public void PlayersGender(EmoteEnercitiesMessages.Gender player1gender, EmoteEnercitiesMessages.Gender player2gender)
        {
            publisher.PlayersGender(player1gender, player2gender);
        }

        public void PlayersGenderString(string player1gender, string player2gender)
        {
            publisher.PlayersGenderString(player1gender, player2gender);
        }

        public void ReachedNewLevel(int level)
        {
            publisher.ReachedNewLevel(level);
        }

        public void ResumeGame(string player1name, string player1role, string player2name, string player2role, string serializedGameState)
        {
            publisher.ResumeGame(player1name, player1role, player2name, player2role, serializedGameState);
        }

        public void StrategyGameMoves(string environmentalistMove, string economistMove, string mayorMove, string globalMove)
        {
            publisher.StrategyGameMoves(environmentalistMove, economistMove, mayorMove, globalMove);
        }

        public void TurnChanged(string serializedGameState)
        {
            publisher.TurnChanged(serializedGameState);
        }

        public void UpdateStrategies(string StrategiesSet_strategies)
        {
            publisher.UpdateStrategies(StrategiesSet_strategies);
        }

        public void ConfirmConstruction(EmoteEnercitiesMessages.StructureType structure, int cellX, int cellY)
        {
            publisher.ConfirmConstruction(structure, cellX, cellY);
        }

        public void ImplementPolicy(EmoteEnercitiesMessages.PolicyType policy)
        {
            publisher.ImplementPolicy(policy);
        }

        public void PerformUpgrade(EmoteEnercitiesMessages.UpgradeType upgrade, int cellX, int cellY)
        {
            publisher.PerformUpgrade(upgrade, cellX, cellY);
        }

        public void PlayStrategy(EmoteEnercitiesMessages.EnercitiesStrategy strategy)
        {
            publisher.PlayStrategy(strategy);
        }

        public void SkipTurn()
        {
            publisher.SkipTurn();
        }

        public void GazeAtScreen(double x, double y)
        {
            publisher.GazeAtScreen(x, y);
        }

        public void GazeAtTarget(string targetName)
        {
            publisher.GazeAtTarget(targetName);
        }

        public void GlanceAtScreen(double x, double y)
        {
            publisher.GlanceAtScreen(x, y);
        }

        public void GlanceAtTarget(string targetName)
        {
            publisher.GlanceAtTarget(targetName);
        }

        public void Head(string id, string lexeme, int repetitions, double amplitude = 20.0f, double frequency = 1.0f)
        {
            publisher.Head(id, lexeme, repetitions, amplitude, frequency);
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

        public void ReplaceTagsAndPerform(string utterance, string category)
        {
            publisher.ReplaceTagsAndPerform(utterance, category);
        }

        public void PlayAnimation(string id, string animation)
        {
            publisher.PlayAnimation(id, animation);
        }

        public void PlayAnimationQueued(string id, string animation)
        {
            publisher.PlayAnimationQueued(id, animation);
        }

        public void StopAnimation(string id)
        {
            publisher.StopAnimation(id);
        }
    }


    class WoZOptimusPrimeMegaClient : ThalamusClient, IWoZOptimusPrimeMegaClient
    {
        static private WoZOptimusPrimeMegaClient instance = null;
        private IWoZOptimusPrimeMegaPublisher wozPublisher;
        public IWoZOptimusPrimeMegaPublisher WOZPublisher
        {
            get
            {
                return wozPublisher;
            }
        }
        public string CharacterName { get; set; }

        public WoZOptimusPrimeMegaClient(string thalamusCharacterName)
            : base("WoZOptimusPrimeMegaClient", thalamusCharacterName)
        {
            instance = this;
            SetPublisher<IWoZOptimusPrimeMegaPublisher>();
            wozPublisher = new WoZOptimusPrimeMegaPublisher(Publisher);
            CharacterName = thalamusCharacterName;
        }

        public static WoZOptimusPrimeMegaClient GetInstance() {
            return instance;
        }

        #region event arguments classes definition

        public class EndGameEventArgs : EventArgs
        {
            public int TotalScore { get; set; }
        }
        public class GenericGameEventArgs : EventArgs
        {
            public string Player1name {get; set; }
            public string Player1role {get; set; }
            public string Player2name {get; set; }
            public string Player2role { get; set; }
            public string SerializedGameState { get; set; }     //TODO deserialize it here and send it as a class, not a string
        }
        public class PlayersGenderEventArgs : EventArgs
        {
            public EmoteEnercitiesMessages.Gender Player1gender { get; set; }
            public EmoteEnercitiesMessages.Gender Player2gender { get; set; }
        }
        public class ReachedNewLevelEventArgs : EventArgs
        {
            public int Level { get; set; }
        }
        public class StrategyGameMovesEventArgs : EventArgs
        {
            public string EnvironmentalistMove { get; set; }
            public string EconomistMove { get; set; }
            public string MayorMove { get; set; }
            public string GlobalMove { get; set; }
        }
        public class ExamineCellEventArgs : EventArgs
        {
            public double ScreenX { get; set; }
            public double ScreenY { get; set; }
            public int CellX { get; set; }
            public int CellY { get; set; }
            public EmoteEnercitiesMessages.StructureType StructureType_structure { get; set; }
            public string StructureType_translated { get; set; }
        }
        public class GameActionEventArgs : EventArgs
        {
            public int ActionTypeEnum { get; set; }
            public string Translation { get; set; }
            public int CellX { get; set; }
            public int CellY { get; set; }
        }
        public class IAEventArgs : EventArgs
        {
            public string[] EnercitiesActionInfo_actionInfos {get; set; }
            public double[] Values {get; set;}
            public string StrategiesSet_strategies { get; set; }
        }
        public class WordDetectedEventArgs : EventArgs
        {
            public string[] Words { get; set; }
        }
        public class TargetsEventArgs : EventArgs
        {
            public string TargetName { get; set; }
            public string LinkedTargetName { get; set; }
            public int X { get; set; }
            public int Y { get; set; }
        }
        public class EyebrowsAUEventArgs : EventArgs
        {
            public double AU2_user1 { get; set; }
            public double AU4_user1 { get; set; }
            public double AU2_user2 { get; set; }
            public double AU4_user2 { get; set; }
        }
        public class PerceptionEventArgs : EventArgs
        {
            public int UserID { get; set; }
            //public EmoteCommonMessages.HeadDirection Direction { get; set; }
            public EmoteCommonMessages.Hand Hand { get; set; }
            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }
            public bool DetectedSkeleton { get; set; }
            public bool Value { get; set; }
        }
        public class ActiveSoundUserEventArgs : EventArgs
        {
            public EmoteCommonMessages.ActiveUser UserAct { get; set; }
            public double LeftValue { get; set; }
            public double RightValue { get; set; }
        }
        public class SpeechEventArgs : EventArgs
        {
            public string ID { get; set; }
            public string Text { get; set; }
            public string[] Texts { get; set; }
            public string[] Bookmarks { get; set; }
        }
        public class AnimationEventArgs : EventArgs
        {
            public string ID { get; set; }
        }
        #endregion

        #region Thalamus messages receiving and calling events

        public event EventHandler<EndGameEventArgs> EndGameNoOilEvent;
        public void EndGameNoOil(int totalScore)
        {
            if (EndGameNoOilEvent != null) EndGameNoOilEvent(this, new EndGameEventArgs() { TotalScore = totalScore });
        }

        public event EventHandler<EndGameEventArgs> EndGameSuccessfullEvent;
        public void EndGameSuccessfull(int totalScore)
        {
            if (EndGameSuccessfullEvent != null) EndGameSuccessfullEvent(this, new EndGameEventArgs() { TotalScore = totalScore });
        }

        public event EventHandler<EndGameEventArgs> EndGameTimeOutEvent;
        public void EndGameTimeOut(int totalScore)
        {
            if (EndGameTimeOutEvent != null) EndGameTimeOutEvent(this, new EndGameEventArgs() { TotalScore = totalScore });
        }

        public event EventHandler<GenericGameEventArgs> GameStartedEvent;
        public void GameStarted(string player1name, string player1role, string player2name, string player2role)
        {
            if (GameStartedEvent != null) GameStartedEvent(this, new GenericGameEventArgs() { Player1name = player1name, Player2name = player2name, Player1role = player1role, Player2role = player2role  });
        }

        public event EventHandler<PlayersGenderEventArgs> PlayersGenderEvent;
        public void PlayersGender(EmoteEnercitiesMessages.Gender player1gender, EmoteEnercitiesMessages.Gender player2gender)
        {
            if (PlayersGenderEvent != null) PlayersGenderEvent(this, new PlayersGenderEventArgs() { Player1gender = player1gender, Player2gender = player2gender });
        }
        public void PlayersGenderString(string player1gender, string player2gender)
        {
            EmoteEnercitiesMessages.Gender p1 = (EmoteEnercitiesMessages.Gender)Enum.Parse(typeof(EmoteEnercitiesMessages.Gender), player1gender);
            EmoteEnercitiesMessages.Gender p2 = (EmoteEnercitiesMessages.Gender)Enum.Parse(typeof(EmoteEnercitiesMessages.Gender), player2gender);
            if (PlayersGenderEvent != null) PlayersGenderEvent(this, new PlayersGenderEventArgs() { Player1gender = p1, Player2gender = p2 });
        }

        public event EventHandler<ReachedNewLevelEventArgs> ReachedNewLevelEvent;
        public void ReachedNewLevel(int level)
        {
            if (ReachedNewLevelEvent != null) ReachedNewLevelEvent(this, new ReachedNewLevelEventArgs() { Level = level });
        }

        public event EventHandler<GenericGameEventArgs> ResumeGameEvent;
        public void ResumeGame(string player1name, string player1role, string player2name, string player2role, string serializedGameState)
        {
            if (GameStartedEvent != null) GameStartedEvent(this, new GenericGameEventArgs() { Player1name = player1name, Player2name = player2name, Player1role = player1role, Player2role = player2role, SerializedGameState = serializedGameState });
        }

        public event EventHandler<GenericGameEventArgs> TurnChangedEvent;
        public void TurnChanged(string serializedGameState)
        {
            if (TurnChangedEvent != null) TurnChangedEvent(this, new GenericGameEventArgs() { SerializedGameState = serializedGameState });
        }

        public event EventHandler<StrategyGameMovesEventArgs> StrategyGameMovesEvent;
        public void StrategyGameMoves(string environmentalistMove, string economistMove, string mayorMove, string globalMove)
        {
            if (StrategyGameMovesEvent != null) StrategyGameMovesEvent(this, new StrategyGameMovesEventArgs() { EnvironmentalistMove = environmentalistMove, EconomistMove = economistMove, MayorMove = mayorMove, GlobalMove = globalMove });
        }
        
        public event EventHandler<ExamineCellEventArgs> ExamineCellEvent;
        public void ExamineCell(double screenX, double screenY, int cellX, int cellY, EmoteEnercitiesMessages.StructureType structureType_structure, string structureType_translated)
        {
            if (ExamineCellEvent != null) ExamineCellEvent(this, new ExamineCellEventArgs() { ScreenX = screenX, ScreenY = screenY, CellX = cellX, CellY = cellY, StructureType_structure = structureType_structure, StructureType_translated = structureType_translated });
        }

        public event EventHandler<GameActionEventArgs> ImplementPolicyEvent;
        public void ImplementPolicy(EmoteEnercitiesMessages.PolicyType policy, string translation)
        {
            if (ImplementPolicyEvent != null) ImplementPolicyEvent(this, new GameActionEventArgs() { ActionTypeEnum = (int)policy, Translation = translation });
        }

        public event EventHandler<GameActionEventArgs> ConfirmConstructionEvent;
        public void ConfirmConstruction(EmoteEnercitiesMessages.StructureType structure, string translation, int cellX, int cellY)
        {
            if (ConfirmConstructionEvent != null) ConfirmConstructionEvent(this, new GameActionEventArgs() { ActionTypeEnum = (int)structure, Translation = translation, CellX = cellX, CellY = cellY });
        }

        public event EventHandler<GameActionEventArgs> PerformUpgradeEvent;
        public void PerformUpgrade(EmoteEnercitiesMessages.UpgradeType upgrade, string translation, int cellX, int cellY)
        {
            if (PerformUpgradeEvent != null) PerformUpgradeEvent(this, new GameActionEventArgs() { ActionTypeEnum = (int)upgrade, Translation = translation, CellX = cellX, CellY = cellY });
        }

        public event EventHandler SkipTurnEvent;
        public void SkipTurn()
        {
            if (SkipTurnEvent != null) SkipTurnEvent(this, null);

        }

        public event EventHandler<IAEventArgs> BestActionPlannedEvent;
        public void BestActionPlanned(string[] enercitiesActionInfo_actionInfos)
        {
            if (BestActionPlannedEvent != null) BestActionPlannedEvent(this, new IAEventArgs() { EnercitiesActionInfo_actionInfos = enercitiesActionInfo_actionInfos });
        }

        public event EventHandler<IAEventArgs> BestActionsPlannedEvent;
        public void BestActionsPlanned(string[] enercitiesActionInfo_actionInfos)
        {
            if (BestActionsPlannedEvent != null) BestActionsPlannedEvent(this, new IAEventArgs() { EnercitiesActionInfo_actionInfos = enercitiesActionInfo_actionInfos });
        }

        public event EventHandler<IAEventArgs> PredictedValuesUpdatedEvent;
        public void PredictedValuesUpdated(double[] values)
        {
            if (PredictedValuesUpdatedEvent != null) PredictedValuesUpdatedEvent(this, new IAEventArgs() { Values = values });
        }

        public event EventHandler<IAEventArgs> StrategiesUpdatedEvent;
        public void StrategiesUpdated(string strategiesSet_strategies)
        {
            if (StrategiesUpdatedEvent != null) StrategiesUpdatedEvent(this, new IAEventArgs() { StrategiesSet_strategies = strategiesSet_strategies });
        }
        
        public event EventHandler<WordDetectedEventArgs> WordDetectedEvent;
        public void WordDetected(string[] words)
        {
            if (WordDetectedEvent != null) WordDetectedEvent(this, new WordDetectedEventArgs() { Words = words });
        }
        
        public event EventHandler<TargetsEventArgs> TargetLinkEvent;
        public void TargetLink(string targetName, string linkedTargetName)
        {
            if (TargetLinkEvent != null) TargetLinkEvent(this, new TargetsEventArgs() { TargetName = targetName, LinkedTargetName = linkedTargetName });
        }

        public event EventHandler<TargetsEventArgs> TargetScreenInfoEvent;
        public void TargetScreenInfo(string targetName, int x, int y)
        {
            if (TargetScreenInfoEvent != null) TargetScreenInfoEvent(this, new TargetsEventArgs() { TargetName = targetName, X = x, Y = y });
        }

        public event EventHandler<EyebrowsAUEventArgs> EyebrowsAUEvent;
        public void EyebrowsAU(double au2_user1, double au4_user1, double au2_user2, double au4_user2)
        {
            if (EyebrowsAUEvent != null) EyebrowsAUEvent(this, new EyebrowsAUEventArgs() { AU2_user1 = au2_user1, AU2_user2 = au2_user2, AU4_user1 = au4_user1, AU4_user2 = au4_user2 });
        }

        public void EyebrowsAU2(double au4left_user1, double au4right_user1, double au4left_user2, double au4right_user2)
        {
            //if (EyebrowsAUEvent != null) EyebrowsAUEvent(this, new EyebrowsAUEventArgs() { AU2_user1 = au2_user1, AU2_user2 = au2_user2, AU4_user1 = au4_user1, AU4_user2 = au4_user2 });
            throw new NotImplementedException();
        }

        
        public event EventHandler<PerceptionEventArgs> GazeTrackingEvent;
        public void GazeTracking(int userID, EmoteCommonMessages.GazeEnum direction, int ConfidenceVal)
        {
            if (GazeTrackingEvent != null) GazeTrackingEvent(this, new PerceptionEventArgs() { UserID = userID});// , GazeEnum = direction });
        }

        public event EventHandler<PerceptionEventArgs> HeadTrackingEvent;
        public void HeadTracking(int userID, double x, double y, double z, bool detectedSkeleton)
        {
            if (HeadTrackingEvent != null) HeadTrackingEvent(this, new PerceptionEventArgs() { UserID = userID, X = x, Y = y, Z = z, DetectedSkeleton = detectedSkeleton});
        }

        public event EventHandler<PerceptionEventArgs> PointingPositionEvent;
        public void PointingPosition(int userID, EmoteCommonMessages.Hand hand, double x, double y, double z)
        {
            if (PointingPositionEvent != null) PointingPositionEvent(this, new PerceptionEventArgs() { UserID = userID, X = x, Y = y, Z = z, Hand = hand });
        }

        public event EventHandler<PerceptionEventArgs> UserMutualGazeEvent;
        public void UserMutualGaze(bool value)
        {
            if (UserMutualGazeEvent != null) UserMutualGazeEvent(this, new PerceptionEventArgs() { Value = value });
        }

        public event EventHandler<PerceptionEventArgs> UserMutualPointEvent;
        public void UserMutualPoint(bool value, double avegX, double avegY)
        {
            if (UserMutualPointEvent != null) UserMutualPointEvent(this, new PerceptionEventArgs() { Value = value, X = avegX, Y = avegY });
        }

        public event EventHandler<PerceptionEventArgs> GazeOKAOEvent;
        public void GazeOKAO(int userID, bool GazeAtrobot)
        {
            if (GazeOKAOEvent != null) GazeOKAOEvent(this, new PerceptionEventArgs() { UserID = userID, Value = GazeAtrobot });
        }

        public event EventHandler<PerceptionEventArgs> UserTouchChinEvent;
        public void UserTouchChin(int userID, bool value)
        {
            if (UserTouchChinEvent != null) UserTouchChinEvent(this, new PerceptionEventArgs() { UserID = userID, Value = value });
        }

        public void Smile(int userID, int smileVal, int confienceVal)
        {
            throw new NotImplementedException();
        }

        public event EventHandler<ActiveSoundUserEventArgs> ActiveSoundUserEvent;
        public void ActiveSoundUser(EmoteCommonMessages.ActiveUser userAct, double leftValue, double rightValue)
        {
            if (ActiveSoundUserEvent != null) ActiveSoundUserEvent(this, new ActiveSoundUserEventArgs() { UserAct = userAct, LeftValue = leftValue, RightValue = rightValue });
        }

        public event EventHandler<SpeechEventArgs> SpeakStartedEvent;
        public void SpeakStarted(string id)
        {
            if (SpeakStartedEvent != null) SpeakStartedEvent(this, new SpeechEventArgs() { ID = id });
        }

        public event EventHandler<SpeechEventArgs> SpeakFinishedEvent;
        public void SpeakFinished(string id)
        {
            if (SpeakFinishedEvent != null) SpeakFinishedEvent(this, new SpeechEventArgs() { ID = id });
        }

        public event EventHandler<SpeechEventArgs> SpeakEvent;
        public void Speak(string id, string text)
        {
            if (SpeakEvent != null) SpeakEvent(this, new SpeechEventArgs() { ID = id, Text = text });
        }

        public event EventHandler<SpeechEventArgs> SpeakBookmarksEvent;
        public void SpeakBookmarks(string id, string[] text, string[] bookmarks)
        {
            if (SpeakBookmarksEvent != null) SpeakBookmarksEvent(this, new SpeechEventArgs() { ID = id, Texts = text, Bookmarks = bookmarks });
        }

        public event EventHandler SpeakStopEvent;
        public void SpeakStop()
        {
            if (SpeakStopEvent != null) SpeakStopEvent(this, null);
        }

        public event EventHandler<AnimationEventArgs> AnimationStartedEvent;
        public void AnimationStarted(string id)
        {
            if (AnimationStartedEvent != null) AnimationStartedEvent(this, new AnimationEventArgs() { ID = id });
        }

        public event EventHandler<AnimationEventArgs> AnimationFinishedEvent;
        public void AnimationFinished(string id)
        {
            if (AnimationFinishedEvent != null) AnimationFinishedEvent(this, new AnimationEventArgs() { ID = id });
        }

#endregion



    }
}
