using CookComputing.XmlRpc;
using EmoteEvents;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thalamus;

namespace MapInterface
{
    public interface IMapInterface : EmoteCommonMessages.IEmoteActions, EmoteMapReadingEvents.IMapActions, EmoteCommonMessages.ILearnerModelIdEvents
    {} //subscribing

    public interface IMapInterfacePublisher : IThalamusPublisher, EmoteMapReadingEvents.ITaskEvents, EmoteCommonMessages.IMapEvents, EmoteCommonMessages.ITargetEvents, EmoteMapReadingEvents.IGameStateEvents, EmoteMapReadingEvents.IMapEvents, EmoteCommonMessages.IEmoteActions, EmoteCommonMessages.ILearnerModelIdActions
    { } //publishing

    public interface IJavaInterface : EmoteMapReadingEvents.IGameStateEvents, EmoteMapReadingEvents.IMapEvents, EmoteMapReadingEvents.ITaskEvents, EmoteCommonMessages.IEmoteActions, EmoteCommonMessages.ILearnerModelIdActions
    { } //publishing

    public class MapInterfaceClient : ThalamusClient, IMapInterface
    {
        internal class MapInterfacePublisher : IMapInterfacePublisher
        {
            dynamic publisher;
            public MapInterfacePublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }

            public void TargetScreenInfo(string targetName, int X, int Y)
            {
                publisher.TargetScreenInfo(targetName, X, Y);
            }

            public void TargetScreenInfoCopyFrom(string targetName, string copyFromTarget)
            {
                publisher.TargetScreenInfoCopyFrom(targetName, copyFromTarget);
            }

            public void Click(double x, double y)
            {
                // User clicking on a point on the screen. x and y representing the coordinates of the point in pixels
                publisher.Click(x, y);
            }
            public void Zoom(double[] finger0, double[] finger1, double[] finger0Start, double[] finger1Start)
            {
                // each array contains the x and the y coordinate of each finger. The "start" represents the coordinates of the location where zoom gesture started
                publisher.Zoom(finger0, finger1, finger0Start, finger1Start);

            }
            public void Pan(double x, double y, double startX, double startY)
            {
                publisher.Pan( x,  y,  startX,  startY);
            }

            public void GameState(Boolean running)
            {
                publisher.GameState(running);
            }

            public void Highlight(double a,double b)
            {
                publisher.Highlight(a,b);
            }

            public void HighlightRightAnswer()
            {
                publisher.HighlightRightAnswer();
            }

            public void Pan()
            {
                publisher.Pan();
            }

            public void Zoom()
            {
                publisher.Zoom();
            }

            public void Click()
            {
                publisher.Click();
            }

            public void GiveQuestionnaire(String name)
            {
                publisher.GiveQuestionnaire(name);
            }

            public void TextShownOnScreen()
            {
                publisher.TextShownOnScreen();
            }

            public void DistanceToolHasStarted()
            {
                publisher.DistanceToolHasStarted();
            }

            public void DistanceToolHasEnded()
            {
                publisher.DistanceToolHasEnded();
            }

            public void DistanceToolHasReset()
            {
                publisher.DistanceToolHasReset();
            }
       
            public void Select(double lat, double lon, string symbolName)
            {
                publisher.Select( lat,  lon,  symbolName);
            }

       //     public void stepAnswerAttempt(int stepId, Boolean correct, Boolean distanceCorrect, Boolean directionCorrect, Boolean symbolCorrect, double distanceSkillLevel, double directionSkillLevel, double symbolSkillLevel, Boolean toolCorrect, String actualTool, String actualSymbol, String actualDistance, String actualDirection)
         //   {
           //    publisher.stepAnswerAttempt( stepId,  correct,  distanceCorrect,  directionCorrect,  symbolCorrect,  distanceSkillLevel,  directionSkillLevel,  symbolSkillLevel, toolCorrect, actualTool, actualSymbol, actualDistance, actualDirection);
            //}
            //public void AddOverlay()
            //{
            //    publisher.AddOverlay();
            //}

            public void CompassHide()
            {
                publisher.CompassHide();
            }

            public void CompassShow()
            {
                publisher.CompassShow();
            }

            public void DistanceHide()
            {
                publisher.DistanceHide();
            }

            public void DistanceShow()
            {
                publisher.DistanceShow();
            }

            public void DragTo()
            {
                publisher.DragTo();
            }

            public void MapKeyHide()
            {
                publisher.MapKeyHide();
            }

            public void MapKeyShow()
            {
                publisher.MapKeyShow();
            }

            public void BlockUI()
            {
                publisher.BlockUI();
            }

            public void UnBlockUI()
            {
                publisher.UnBlockUI();
            }

            

            public void ToolAction(String toolName, String toolAction)
            {
                publisher.ToolAction(toolName,toolAction);
            }
            public void CompassHighlightDirection(String direction)
            {
                publisher.CompassHighlightDirection(direction);
            }

            //public void RemoveOverlay()
            //{
            //    publisher.RemoveOverlay();
            //}

            //public void Select()
            //{
            //    publisher.Select();
            //}

            //public void RequestHelp()
            //{
            //    publisher.RequestHelp();
            //}

            //public void RequestNextInstruction()
            //{
            //    publisher.RequestNextInstruction();
            //}

            //public void RequestRepeatInstruction()
            //{
            //    publisher.RequestRepeatInstruction();
            //}

            //public void RequestTutorial()
            //{
            //    publisher.RequestTutorial();
            //}

            //public void RespondWithInfo()
            //{
            //    publisher.RespondWithInfo();
            //}

            //public void ChooseInformant()
            //{
            //    publisher.ChooseInformant();
            //}





            public void  interactionEvaluation(int learnerId, int stepId, int activityId, int scenarioId, int sessionId, string action, string strategy, string evaluation)
            {
                throw new NotImplementedException();
            }

            public void stepAnswerAttempt(int learnerId, int stepId, int activityId, int scenarioId, int sessionId, bool correct, string[] competencyName, bool[] competencyCorrect, string[] competencyActual, string[] competencyExpected, string mapEventId, string[] comptencyValue, string[] competencyConfidence, string[] oldCompetencyValue, string[] competencyDelta)
            {
                publisher.stepAnswerAttempt(learnerId, stepId, activityId, scenarioId, sessionId, correct, competencyName, competencyCorrect, competencyActual, competencyExpected, mapEventId, comptencyValue, competencyConfidence,oldCompetencyValue,  competencyDelta);

            }

            public void TargetLink(string targetName, string linkedTargetName)
            {
                throw new NotImplementedException();
            }

            /*
            public void learnerModelValueUpdateAfterAffectPerceptionUpdate(int learnerId, int stepId, int activityId, int scenarioId, int sessionId, EmoteCommonMessages.LearnerModelUpdateReason reasonForUpdate, bool correct, string[] competencyName, bool[] competencyCorrect, string[] competencyActual, string[] competencyExpected, double[] comptencyValue, int[] competencyConfidence, double[] oldCompetencyValue, double[] competencyDelta, EmoteCommonMessages.AffectPerceptionState[] state, EmoteCommonMessages.Charge[] stateCharge, int[] stateConfidence, String mapEventId)
            {
                publisher.learnerModelValueUpdateAfterAffectPerceptionUpdate(learnerId, stepId, activityId, scenarioId, sessionId, reasonForUpdate, correct, competencyName, competencyCorrect, competencyActual, competencyExpected, comptencyValue, competencyConfidence, oldCompetencyValue, competencyDelta, state, stateCharge, stateConfidence, mapEventId);   
            }

            public void learnerModelValueUpdateBeforeAffectPerceptionUpdate(int learnerId, int stepId, int activityId, int scenarioId, int sessionId, EmoteCommonMessages.LearnerModelUpdateReason reasonForUpdate, bool correct, string[] competencyName, bool[] competencyCorrect, string[] competencyActual, string[] competencyExpected, double[] comptencyValue, int[] competencyConfidence, double[] oldCompetencyValue, double[] competencyDelta, String mapEventId)
            {
                publisher.learnerModelValueUpdateBeforeAffectPerceptionUpdate(learnerId, stepId, activityId, scenarioId, sessionId, reasonForUpdate, correct, competencyName, competencyCorrect, competencyActual, competencyExpected, comptencyValue, competencyConfidence, oldCompetencyValue, competencyDelta, mapEventId);       
            }*/

            public void SetLearnerInfo(string LearnerInfo_learnerInfo)
            {
                LearnerInfo li = LearnerInfo.DeserializeFromJson(LearnerInfo_learnerInfo);
                Console.WriteLine("Learner Info Set For:" + li.firstName);
                    
                publisher.SetLearnerInfo(LearnerInfo_learnerInfo);
            }

            public void Reset()
            {
                publisher.Reset();
            }
/*
            public void Start(int participantId, string participantName, string participant1Name, string participant2Name)
            {
                publisher.Start(participantId, participantName, participant1Name,  participant2Name);
            }
            */
            public void Stop()
            {
                publisher.Stop();
            }

            public void getAllLearnerInfo()
            {
                publisher.getAllLearnerInfo();
            }

            public void getNextThalamusId()
            {
                publisher.getNextThalamusId();
            }

            /*
            public void Start(int participantId, int participantId2, string participant1Name, string participant2Name)
            {
                publisher.Start(participantId, participantId2, participant1Name,  participant2Name);
            }
            */

            public void getAllUtterancesForParticipant(int participantId)
            {
               // throw new NotImplementedException();
            }


            public void Start(string StartMessageInfo_info)
            {
               // throw new NotImplementedException();
                publisher.Start(StartMessageInfo_info);
            }
        }

        HttpListener JavaRpcListener;
        private Thread javaListenerThread;
        private Thread javaRequestDispatcherThread;
        bool serviceRunning = false;
        private int JavaXmlRpcPort = 10000;
        private String  javaHost = "localhost";

        List<HttpListenerContext> javaHttpRequestsQueue = new List<HttpListenerContext>();

        internal MapInterfacePublisher MapPublisher;
        public MapInterfaceClient() : base("MapApplication") 
        {
            SetPublisher<IMapInterfacePublisher>();
            MapPublisher = new MapInterfacePublisher(Publisher);

            javaListenerThread = new Thread(new ThreadStart(JavaHttpListenerThread));
            javaListenerThread.Start();

            javaRequestDispatcherThread = new Thread(new ThreadStart(JavaRequestDispatcher));
            javaRequestDispatcherThread.Start();
        }

        public override void Dispose()
        {
            base.Dispose();
            try
            {
                if (javaListenerThread != null) javaListenerThread.Join();
            }
            catch { }
            try
            {
                if (javaRequestDispatcherThread != null) javaRequestDispatcherThread.Join();
            }
            catch { }
        }

        internal void JavaHttpListenerThread()
        {
            while (!Shutingdown)
            {
                while (!serviceRunning)
                {
                    try
                    {
                        Debug("Attempt to start service on port '" + JavaXmlRpcPort + "'");
                        JavaRpcListener = new HttpListener();
                        JavaRpcListener.Prefixes.Add(string.Format("http://*:{0}/", JavaXmlRpcPort));
                        JavaRpcListener.Start();
                        Debug("XMLRPC Listening on port " + JavaXmlRpcPort);
                        serviceRunning = true;
                    }
                    catch
                    {
                        Debug("Port unavaliable.");
                        serviceRunning = false;
                        Thread.Sleep(1000);
                    }
                }

                try
                {
                    HttpListenerContext context = JavaRpcListener.GetContext();
                    lock (javaHttpRequestsQueue)
                    {
                        javaHttpRequestsQueue.Add(context);
                    }
                }
                catch (Exception e)
                {
                    DebugException(e);
                    serviceRunning = false;
                    if (JavaRpcListener != null) JavaRpcListener.Close();
                }
            }
            if (JavaRpcListener != null) JavaRpcListener.Close();
            Debug("Terminated JavaHttpListenerThread");
        }

        internal void JavaRequestDispatcher()
        {
            while (!Shutingdown)
            {
                bool performSleep = true;
                try
                {
                    if (javaHttpRequestsQueue.Count > 0)
                    {
                        performSleep = false;
                        List<HttpListenerContext> httpRequests;
                        lock (javaHttpRequestsQueue)
                        {
                            httpRequests = new List<HttpListenerContext>(javaHttpRequestsQueue);
                            javaHttpRequestsQueue.Clear();
                        }
                        foreach (HttpListenerContext r in httpRequests)
                        {
                            ProcessJavaRequest(r);
                        }
                    }
                }
                catch (Exception e)
                {
                    DebugException(e);
                }
                if (performSleep) Thread.Sleep(10);
            }
            Debug("Terminated JavaRequestDispatcherThread");
        }

        internal void ProcessJavaRequest(object oContext)
        {
            try
            {
                XmlRpcListenerService svc = new JavaThalamusEventHandler(this);
                svc.ProcessRequest((HttpListenerContext)oContext);
            }
            catch (Exception e)
            {
                DebugException(e);
            }

        }


        void EmoteCommonMessages.IEmoteActions.Reset()
        {
         
        }

        void JavaWebRequest(String endBit, string[] jsonStringArray)
        {
            Console.WriteLine("http://" + javaHost + ":8080/namshub/" + endBit);
            var httpWebRequest = (System.Net.HttpWebRequest)WebRequest.Create("http://" + javaHost + ":8080/namshub/" + endBit);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            //  using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            // {
            string json = "";
            json = json + "{\"learnerInfos\":[";
            //json = json + "{[";
            int i = 0;
            foreach (string s in jsonStringArray)
            {
                if (s != "")
                {
                    json = json + s + ",";
                    i++;
                }
                //Console.WriteLine("Item: " + s);
            }
            json = json.Remove(json.Length - 1);
            json = json + "]}";

            // "{\"participantId\":\"" + participantId + "\",\"participantName\":\"" + participantName + "\"}";
            Console.WriteLine("json: " + json);
            Encoding encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(json);
            httpWebRequest.ContentLength = data.Length;


            Stream stream = httpWebRequest.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            //  streamWriter.Write(json);

            // }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                Console.WriteLine("Response from Java " + responseText);

            }
        }

        void JavaWebRequest(String endBit, String jsonString)
        {
            Console.WriteLine("http://" + javaHost + ":8080/namshub/" + endBit);
            var httpWebRequest = (System.Net.HttpWebRequest)WebRequest.Create("http://" + javaHost + ":8080/namshub/" + endBit);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            Console.WriteLine("json: " + jsonString);
            Encoding encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(jsonString);
            httpWebRequest.ContentLength = data.Length;


            Stream stream = httpWebRequest.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                Console.WriteLine("Response from Java " + responseText);

            }
        }
        void JavaWebRequest(String endBit, Dictionary <String,String> parametersForJson)
        {
            Console.WriteLine("http://" + javaHost + ":8080/namshub/" + endBit);
            var httpWebRequest = (System.Net.HttpWebRequest)WebRequest.Create("http://" + javaHost + ":8080/namshub/" + endBit);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            //  using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            // {
            string json ="";
            json = json + "{";
            foreach (KeyValuePair<String, String> item in parametersForJson)
            {
                json = json + "\"" + item.Key + "\":\"" + item.Value + "\",";
            }           
            json = json.Remove(json.Length - 1);
            json = json + "}";

            // "{\"participantId\":\"" + participantId + "\",\"participantName\":\"" + participantName + "\"}";
            Console.WriteLine("json: " + json);
            Encoding encoding = new UTF8Encoding();
            byte[] data = encoding.GetBytes(json);
            httpWebRequest.ContentLength = data.Length;


            Stream stream = httpWebRequest.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();

            //  streamWriter.Write(json);

            // }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                Console.WriteLine("Response from Java " + responseText);

            }
        }

         
        void EmoteCommonMessages.IEmoteActions.Stop()
        {
            MapPublisher.GameState(false);
        }


        //void EmoteMapReadingEvents.IMapActions.AddOverlay()
        //{
        //    throw new NotImplementedException();
        //}

        void EmoteMapReadingEvents.IMapActions.StartNextStep()
        {
            Console.WriteLine("Start Next Step");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["startNextStep"] = "true";
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }


        void EmoteMapReadingEvents.IMapActions.StartStep(int stepId)
        {
            Console.WriteLine("Start Step");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["startStep"] = "true";
            parametersForJson["stepId"] = "" + stepId;
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }

        void EmoteMapReadingEvents.IMapActions.CompassHide()
        {
            Console.WriteLine("CompassTool: Hide");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["tool"] = "compass";
            parametersForJson["toolAction"] = "hide";
         //   parametersForJson["show"] = "false";
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }

        void EmoteMapReadingEvents.IMapActions.CompassShow()
        {
            Console.WriteLine("CompassTool: Show");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["tool"] = "compass";
            parametersForJson["toolAction"] = "show";
            //  parametersForJson["show"] = "true";
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }

        void EmoteMapReadingEvents.IMapActions.CompassHighlight()
        {
            Console.WriteLine("CompassTool: Highlight");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["tool"] = "compass";
            parametersForJson["toolAction"] = "highlight";
           // parametersForJson["highlight"] = "true";
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }

        void EmoteMapReadingEvents.IMapActions.DistanceHide()
        {
            Console.WriteLine("DistanceTool: Hide");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["tool"] = "distance";
            parametersForJson["toolAction"] = "hide";
            //parametersForJson["show"] = "false";
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }

        void EmoteMapReadingEvents.IMapActions.DistanceShow()
        {
            Console.WriteLine("DistanceTool: Show");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["tool"] = "distance";
            parametersForJson["toolAction"] = "show";
           // parametersForJson["show"] = "true";
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }

        void EmoteMapReadingEvents.IMapActions.DistanceToolReset()
        {
            Console.WriteLine("DistanceTool: Reset");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["tool"] = "distance";
            parametersForJson["toolAction"] = "reset";
          //  parametersForJson["reset"] = "true";
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }

        void EmoteMapReadingEvents.IMapActions.DistanceToolEnd()
        {
            Console.WriteLine("DistanceTool: End Measurement");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["tool"] = "distance";
            parametersForJson["toolAction"] = "end";
            //parametersForJson["end"] = "true";
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }

        void EmoteMapReadingEvents.IMapActions.DistanceToolStart()
        {
            Console.WriteLine("DistanceTool: Start Measurement");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["tool"] = "distance";
            parametersForJson["toolAction"] = "start";
          //  parametersForJson["start"] = "true";
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }

        void EmoteMapReadingEvents.IMapActions.DistanceHighlight()
        {
            Console.WriteLine("DistanceTool: Highlight");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["tool"] = "distance";
            parametersForJson["toolAction"] = "highlight";
            //parametersForJson["highlight"] = "true";
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }

        //void EmoteMapReadingEvents.IMapActions.DragTo()
        //{
        //    throw new NotImplementedException();
        //}

        void EmoteMapReadingEvents.IMapActions.MapKeyHide()
        {
            Console.WriteLine("Map Key: Hide");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["tool"] = "mapKey";
            parametersForJson["toolAction"] = "hide";
            //parametersForJson["show"] = "false";
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }

        void EmoteMapReadingEvents.IMapActions.MapKeyHighlight()
        {
            Console.WriteLine("Map Key: Highlight");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["tool"] = "mapKey";
            parametersForJson["toolAction"] = "highlight";
         //   parametersForJson["show"] = "false";
         //   parametersForJson["highlight"] = "true";
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }

        void EmoteMapReadingEvents.IMapActions.MapKeyShow()
        {
            Console.WriteLine("Map Key: Show");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["tool"] = "mapKey";
            parametersForJson["toolAction"] = "show";
        //    parametersForJson["show"] = "true";
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }

        void EmoteMapReadingEvents.IMapActions.BlockUI()
        {
            Console.WriteLine("Block UI");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["blockUI"] = "true";
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }

        void EmoteMapReadingEvents.IMapActions.UnBlockUI()
        {
            Console.WriteLine("UnBlock UI");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["unBlockUI"] = "true";
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }

        void EmoteMapReadingEvents.IMapActions.ToolAction(String toolName, String toolAction)
        {
            Console.WriteLine("Tool action");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            //parametersForJson["doToolAction"] = "true";
            parametersForJson["tool"] = toolName;
            parametersForJson["toolAction"] = toolAction;
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }

        void EmoteMapReadingEvents.IMapActions.CompassHighlightDirection(String direction)
        {
            Console.WriteLine("Compass highlight");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["doCompassHighlight"] = "true";
            parametersForJson["directionToHighlight"] = direction;
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }
/*
        public void GiveQuestionnaire(String name)
        {
            Console.WriteLine("Questionnaire");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["giveQuestionnaire"] = "true";
            parametersForJson["questionnaireName"] = name;
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }

        public void Highlight(double a, double b)
        {
            
        }

        public void HighlightRightAnswer()
        {
            Console.WriteLine("HighlightRightAnswer");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["highlightRightAnswer"] = "true";
            JavaWebRequest("thalamusMapAction", parametersForJson);
        }

  */  

      //  public void Highlight(double a, double b)
        //{
          //  throw new NotImplementedException();
        //}
        //void EmoteMapReadingEvents.IMapActions.RemoveOverlay()
        //{
        //    throw new NotImplementedException();
        //}

        //void EmoteMapReadingEvents.IMapActions.Select()
        //{
        //    throw new NotImplementedException();
        //}


        /* void EmoteCommonMessages.IAffectPerceptionEvents.ProbeRequest(EmoteCommonMessages.Probes type, int urgency)
         {
            
         }

         void EmoteCommonMessages.IAffectPerceptionEvents.UserState(EmoteCommonMessages.AffectPerceptionState state, EmoteCommonMessages.Charge stateCharge, int stateConfidence, int mapEventId)
         {
             Console.WriteLine("AffectPerceptionStateUpdate");
             Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
             parametersForJson["state"] = state.ToString();
             parametersForJson["stateCharge"] = stateCharge.ToString();
             parametersForJson["stateConfidence"] = stateConfidence.ToString();
             parametersForJson["mapEventId"] = mapEventId.ToString();

             JavaWebRequest("affectPerceptionStateUpdate", parametersForJson);
         }
         * */

        void EmoteCommonMessages.IEmoteActions.SetLearnerInfo(string LearnerInfo_learnerInfo)
        {
            
        }


        void EmoteMapReadingEvents.IMapActions.GiveQuestionnaire(string name)
        {
            
        }

        void EmoteMapReadingEvents.IMapActions.HighlightRightAnswer()
        {
         
        }
        /*
        void EmoteCommonMessages.IEmoteActions.Start(int participantId, int participantId2, string participant1Name, string participant2Name)
        {
            Console.WriteLine("Start message");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["participantId"] = "" + participantId;
            parametersForJson["participantName"] = participant1Name;
            JavaWebRequest("thalamusStart", parametersForJson);
        }
        */
        void EmoteCommonMessages.ILearnerModelIdEvents.allLearnerInfo(string[] LearnerInfo_learnerInfos)
        {
            //TODO send to java
            //foreach (string LearnerInfo_learnerInfo in LearnerInfo_learnerInfos)
            //{
            //    Console.WriteLine(s);
            // }
            Console.WriteLine("Got all learner info, passing to java");
            JavaWebRequest("allLearnerInfo", LearnerInfo_learnerInfos);
        }

        void EmoteCommonMessages.ILearnerModelIdEvents.nextThalamusId(int participantId)
        {
            Console.WriteLine("Sending next thalamus id to java");
            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["nextThalamusId"] = "" + participantId;
            JavaWebRequest("nextThalamusId", parametersForJson);
        }


        void EmoteCommonMessages.ILearnerModelIdEvents.allUtterancesForParticipant(int participantId, string[] Utterance_utterances)
        {
           // throw new NotImplementedException();
        }


        void EmoteCommonMessages.IEmoteActions.Start(string StartMessageInfo_info)
        {
            Console.WriteLine("Start message");

            EmoteEvents.ComplexData.StartMessageInfo smi = EmoteEvents.ComplexData.StartMessageInfo.DeserializeFromJson<EmoteEvents.ComplexData.StartMessageInfo>(StartMessageInfo_info);

            LearnerInfo li = smi.Students.First();

            Dictionary<String, String> parametersForJson = new Dictionary<String, String>();
            parametersForJson["participantId"] = "" + li.thalamusLearnerId;
            parametersForJson["participantName"] = li.firstName;
            parametersForJson["scenario"] = smi.ScenarioXmlName;
            parametersForJson["sessionNumber"] = "" + smi.SessionId;
            parametersForJson["language"] = "" + smi.Language;
            JavaWebRequest("thalamusStart", parametersForJson);
        }
    }
}
