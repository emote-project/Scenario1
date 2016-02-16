using CookComputing.XmlRpc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapInterface
{
    internal class JavaThalamusEventHandler : XmlRpcListenerService, IJavaInterface
    {
        MapInterfaceClient client;
        public JavaThalamusEventHandler(MapInterfaceClient client)
        {
            this.client = client;
        }

        [XmlRpcMethod()]
        public void TextShownOnScreen()
        {
            client.MapPublisher.TextShownOnScreen();
        }

        [XmlRpcMethod()]
        public void DistanceToolHasStarted()
        {
            client.MapPublisher.DistanceToolHasStarted();
        }

        [XmlRpcMethod()]
        public void DistanceToolHasEnded()
        {
            client.MapPublisher.DistanceToolHasEnded();
        }

        [XmlRpcMethod()]
        public void DistanceToolHasReset()
        {
            client.MapPublisher.DistanceToolHasReset();
        }


        [XmlRpcMethod()]
        public void GameState(bool running)
        {
            client.MapPublisher.GameState(running);
        }

        //[XmlRpcMethod()]
        //void EmoteMapReadingEvents.IMapEvents.AddOverlay()
        //{
        //    client.MapPublisher.AddOverlay();
        //}

        [XmlRpcMethod()]
        public void Select(double lat, double lon, string symbolName)
        {
            client.MapPublisher.Select( lat, lon,  symbolName);
        }

        [XmlRpcMethod()]
        public void CompassHide()
        {
            client.MapPublisher.CompassHide();
        }

        [XmlRpcMethod()]
        public void CompassShow()
        {
            client.MapPublisher.CompassShow();
        }


        [XmlRpcMethod()]
        public void DistanceHide()
        {
            client.MapPublisher.DistanceHide();
        }

        [XmlRpcMethod()]
        public void DistanceShow()
        {
            client.MapPublisher.DistanceShow();
        }

        [XmlRpcMethod()]
        public void DragTo()
        {
            client.MapPublisher.DragTo();
        }

        [XmlRpcMethod()]
        public void MapKeyHide()
        {
            client.MapPublisher.MapKeyHide();
        }

        [XmlRpcMethod()]
        public void MapKeyShow()
        {
            client.MapPublisher.MapKeyShow();
        }

        //[XmlRpcMethod()]
        //void EmoteMapReadingEvents.IMapEvents.RemoveOverlay()
        //{
        //    client.MapPublisher.RemoveOverlay();
        //}

        //[XmlRpcMethod()]
        //void EmoteMapReadingEvents.IMapEvents.Select()
        //{
        //    client.MapPublisher.Select();
        //}

        //[XmlRpcMethod()]
        //void EmoteMapReadingEvents.IDialogueEvents.RequestHelp()
        //{
        //    client.MapPublisher.RequestHelp();
        //}

        //[XmlRpcMethod()]
        //void EmoteMapReadingEvents.IDialogueEvents.RequestNextInstruction()
        //{
        //    client.MapPublisher.RequestNextInstruction();
        //}

        //[XmlRpcMethod()]
        //void EmoteMapReadingEvents.IDialogueEvents.RequestRepeatInstruction()
        //{
        //    client.MapPublisher.RequestRepeatInstruction();
        //}

        //[XmlRpcMethod()]
        //void EmoteMapReadingEvents.IDialogueEvents.RequestTutorial()
        //{
        //    client.MapPublisher.RequestTutorial();
        //}

        //[XmlRpcMethod()]
        //void EmoteMapReadingEvents.IDialogueEvents.RespondWithInfo()
        //{
        //    client.MapPublisher.RespondWithInfo();
        //}

        //[XmlRpcMethod()]
        //void EmoteMapReadingEvents.ITaskEvents.ChooseInformant()
        //{
        //    client.MapPublisher.ChooseInformant();
        //}

        //[XmlRpcMethod()]
        //public void stepAnswerAttempt(int stepId, Boolean correct, Boolean distanceCorrect, Boolean directionCorrect, Boolean symbolCorrect, double distanceSkillLevel, double directionSkillLevel, double symbolSkillLevel, Boolean toolCorrect, String actualTool, String actualSymbol, String actualDistance, String actualDirection)
        //{
          //  client.MapPublisher.stepAnswerAttempt(stepId, correct, distanceCorrect, directionCorrect, symbolCorrect, distanceSkillLevel, directionSkillLevel, symbolSkillLevel, toolCorrect, actualTool, actualSymbol, actualDistance, actualDirection);

        //}

        [XmlRpcMethod()]
        public void stepAnswerAttempt(int learnerId, int stepId, int activityId, int scenarioId, int sessionId, Boolean correct, String[] competencyName, Boolean[] competencyCorrect, String[] competencyActual, String[] competencyExpected, String mapEventId, String[] comptencyValue, String[] competencyConfidence, String[] oldCompetencyValue, String[] competencyDelta)
        {
            client.MapPublisher.stepAnswerAttempt( learnerId, stepId, activityId, scenarioId, sessionId, correct, competencyName , competencyCorrect, competencyActual, competencyExpected, mapEventId, comptencyValue, competencyConfidence, oldCompetencyValue, competencyDelta);

        }

        [XmlRpcMethod()]
        public void UpdateStaticScreenTargets(int mapKeyX, int mapKeyY, int compassX, int compassY, int distanceX, int distanceY, int midX, int midY, int x, int y, int scrapbookX, int scrapbookY)
        {

            client.MapPublisher.TargetScreenInfo("map_key_button",mapKeyX,mapKeyY);
            client.MapPublisher.TargetScreenInfo("map_key_tool",x,y);
            client.MapPublisher.TargetScreenInfo("compass_button",compassX,compassY);
            client.MapPublisher.TargetScreenInfo("compass_tool",midX,midY);
            client.MapPublisher.TargetScreenInfo("distance_button",distanceX,distanceY);
            client.MapPublisher.TargetScreenInfo("distance_tool",midX,midY);
            client.MapPublisher.TargetScreenInfo("tools",mapKeyX,mapKeyY);
            client.MapPublisher.TargetScreenInfo("scale",0,0);
            client.MapPublisher.TargetScreenInfo("scrapbook",midX,midY);
            client.MapPublisher.TargetScreenInfo("scrapbook_button", scrapbookX, scrapbookY);
            int compassOffset = 50;
            client.MapPublisher.TargetScreenInfo("cardinalNorth", midX, midY + compassOffset);
            client.MapPublisher.TargetScreenInfo("cardinalNorthWest", midX - compassOffset, midY + compassOffset);
            client.MapPublisher.TargetScreenInfo("cardinalWest", midX - compassOffset, midY);
            client.MapPublisher.TargetScreenInfo("cardinalSouthWest", midX - compassOffset, midY - compassOffset);
            client.MapPublisher.TargetScreenInfo("cardinalSouth", midX, midY - compassOffset);
            client.MapPublisher.TargetScreenInfo("cardinalSouthEast", midX + compassOffset, midY - compassOffset);
            client.MapPublisher.TargetScreenInfo("cardinalEast", midX + compassOffset, midY);
            client.MapPublisher.TargetScreenInfo("cardinalNorthEast", midX + compassOffset, midY + compassOffset);
            client.MapPublisher.TargetScreenInfo("cardinalDirections",midX,midY);
            client.MapPublisher.TargetScreenInfo("cardinalDirectionsInter", midX, midY);
            client.MapPublisher.TargetScreenInfo("interCardinalPoints", midX, midY);
            client.MapPublisher.TargetScreenInfo("cardinalPoints", midX, midY);

            client.MapPublisher.TargetScreenInfo("writtenTaskStep", 0, 0);



        }

        [XmlRpcMethod()]
        public void UpdateRightAnswerTarget(int x, int y)
        {

            client.MapPublisher.TargetScreenInfo("rightAnswer", x, y);
        }
        
        

        [XmlRpcMethod()]
        public void ScreenTouch(int screenX, int screenY)
        {
            client.MapPublisher.Click(screenX, screenY);
        }



        [XmlRpcMethod()]
        public void interactionEvaluation(int learnerId, int stepId, int activityId, int scenarioId, int sessionId, string action, string strategy, string evaluation)
        {
            client.MapPublisher.interactionEvaluation(learnerId, stepId, activityId, scenarioId, sessionId, action, strategy, evaluation);
        }

        [XmlRpcMethod()]
        public void SetLearnerInfo(string LearnerInfo_learnerInfo)
        {
            Console.WriteLine("Set Learner Info From Java");
            client.MapPublisher.SetLearnerInfo(LearnerInfo_learnerInfo);
        }

   
        /*
        [XmlRpcMethod()]
        public void learnerModelValueUpdateAfterAffectPerceptionUpdate(int learnerId, int stepId, int activityId, int scenarioId, int sessionId, EmoteCommonMessages.LearnerModelUpdateReason reasonForUpdate, bool correct, string[] competencyName, bool[] competencyCorrect, string[] competencyActual, string[] competencyExpected, double[] comptencyValue, int[] competencyConfidence, double[] oldCompetencyValue, double[] competencyDelta, EmoteCommonMessages.AffectPerceptionState[] state, EmoteCommonMessages.Charge[] stateCharge, int[] stateConfidence, int mapEventId)
        {
            client.MapPublisher.learnerModelValueUpdateAfterAffectPerceptionUpdate(learnerId, stepId, activityId, scenarioId, sessionId, reasonForUpdate, correct, competencyName,  competencyCorrect, competencyActual,  competencyExpected,  comptencyValue, competencyConfidence, oldCompetencyValue, competencyDelta, state,  stateCharge, stateConfidence,  mapEventId);
        }

        [XmlRpcMethod()]
        public void learnerModelValueUpdateBeforeAffectPerceptionUpdate(int learnerId, int stepId, int activityId, int scenarioId, int sessionId, EmoteCommonMessages.LearnerModelUpdateReason reasonForUpdate, bool correct, string[] competencyName, bool[] competencyCorrect, string[] competencyActual, string[] competencyExpected, double[] comptencyValue, int[] competencyConfidence, double[] oldCompetencyValue, double[] competencyDelta, int mapEventId)
        {
            client.MapPublisher.learnerModelValueUpdateBeforeAffectPerceptionUpdate( learnerId,  stepId,  activityId,  scenarioId,  sessionId, reasonForUpdate,  correct, competencyName,  competencyCorrect,  competencyActual,  competencyExpected,  comptencyValue,  competencyConfidence, oldCompetencyValue,  competencyDelta,  mapEventId);
       
        }
         */

        [XmlRpcMethod()]
        public void Reset()
        {
            client.MapPublisher.Reset();
        }
        
        /*
        [XmlRpcMethod()]
        public void Start(int participantId, int participantId2,string participant1Name, string participant2Name)
        {
            //Console.WriteLine("Start from Java");

            //client.MapPublisher.Start(participantId, participantId2, participant1Name,  participant2Name);
        }
        */

        [XmlRpcMethod()]
        public void Stop()
        {
            Console.WriteLine("Stop From Java");
            client.MapPublisher.Stop();
        }

        [XmlRpcMethod()]
        public void getAllLearnerInfo()
        {
            Console.WriteLine("Request for all learner info From Java");
            client.MapPublisher.getAllLearnerInfo();
        }

        [XmlRpcMethod()]
        public void getNextThalamusId()
        {
            Console.WriteLine("Request for next free id from Java");
            client.MapPublisher.getNextThalamusId();
        }

        [XmlRpcMethod()]
        public void getAllUtterancesForParticipant(int participantId)
        {
            throw new NotImplementedException();
        }


        public void Start(string StartMessageInfo_info)
        {
           // throw new NotImplementedException();
        }
    }
}
