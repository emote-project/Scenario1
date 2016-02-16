using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;
using System.Timers;

using PipeClient;
using EmoteEvents;
using EmoteCommonMessages;

namespace IA
{

    public class InteractionAnalysis
    {        
        /* Import the functions from the C++ library */

        [DllImport("FeatureExtraction.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void print_line(string str);

        [DllImport("FeatureExtraction.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void AddTaskData(bool correct, bool distanceCorrect, double distanceScore, bool directionCorrect, double directionScore, bool symbolCorrect, double symbolScore);

        [DllImport("FeatureExtraction.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UpdateTaskActivity();
      
        [DllImport("FeatureExtraction.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AddLLI(string[] str, int userCount);

        [DllImport("FeatureExtraction.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr ExtractFeatures();

        public double mConf = 0;
        public double mConftemp = 0;
        public int mconfcounter = 0;
        public double mArousaltemp = 0;
        public double oldArousal = 0;
        public double currentArousal = 0;
        public double thresArousal = 0;
        /* Create Objects */
        InteractionAnalysisInterfaceClient mThalamusClient;
        Client mPipeClient = new Client();
        
        
        Mutex[] mMutex = new Mutex[3];
        public AutoResetEvent[] mEvents = new AutoResetEvent[2];

        InteractionState mInteractionState = new InteractionState();
        InternalState mInternalState = new InternalState();
        UserState mUserState;
        UserState mPreviousUserState;
        bool mProcessFlag = false;
          
      
        
        /* Constructor */
        public InteractionAnalysis()
        {


            for (int i = 0; i < 3; i++)
            {
                if(i < 2) mEvents[i] = new AutoResetEvent(false);
                mMutex[i] = new Mutex(true);
                mMutex[i].ReleaseMutex();   
            }

            mUserState = new UserState();
            mPreviousUserState = new UserState(mUserState);

            mInternalState.SetupTimer();
        }

/* THREAD 1 */

        public void InitThalamusClient()
        {
            mThalamusClient = new InteractionAnalysisInterfaceClient(this);
        }

        public void Shutdown()
        {
            mThalamusClient.Dispose();
            Environment.Exit(0);
        }

        public void TaskUpdateCallback(LearnerStateInfo lsi)
        {
            if (mInternalState.mState != Status.OKAO_ONLY)
            {

                if (lsi.reasonForUpdate == LearnerModelUpdateReason.StepAnswerAttempt)
                    AddTaskData(lsi.correct, lsi.competencyItems[0].competencyCorrect, lsi.competencyItems[0].comptencyValue, lsi.competencyItems[1].competencyCorrect, lsi.competencyItems[1].comptencyValue, lsi.competencyItems[2].competencyCorrect, lsi.competencyItems[2].comptencyValue);
                else if (lsi.reasonForUpdate == LearnerModelUpdateReason.ToolUse)
                    UpdateTaskActivity();
            }

            UpdateLM(lsi.learnerId, lsi.mapEventId);
        }

        public void UpdateLM(int learnerID = 0, string mEID = "")
        {
            
            AffectPerceptionInfo mAffect = new AffectPerceptionInfo();
            mAffect.mMapEventId = mEID;

            mAffect.mAffectiveStates.Add(new AffectPerceptionInfo.AffectType(AffectPerceptionState.Valence, mUserState.mValence, PointofFocus.NonApplicable, (int)mUserState.mValenceCI));
            mAffect.mAffectiveStates.Add(new AffectPerceptionInfo.AffectType(AffectPerceptionState.Arousal, mUserState.mArousal, PointofFocus.NonApplicable, (int)mUserState.mArousalCI));
            mAffect.mAffectiveStates.Add(new AffectPerceptionInfo.AffectType(AffectPerceptionState.Engagement, mUserState.mSocialEngagement, PointofFocus.Social, (int)mUserState.mArousalCI));
            mAffect.mAffectiveStates.Add(new AffectPerceptionInfo.AffectType(AffectPerceptionState.Engagement, mUserState.mTaskEngagement, PointofFocus.Task, (int)mUserState.mArousalCI));
            //mAffect.mAffectiveStates.Add(new AffectPerceptionInfo.AffectType(AffectPerceptionState.Engagement, mUserState.mSocialEngagement, PointofFocus.Social, (int)mUserState.mSocialEngagementCI));
            //mAffect.mAffectiveStates.Add(new AffectPerceptionInfo.AffectType(AffectPerceptionState.Engagement, mUserState.mTaskEngagement, PointofFocus.Task, (int)mUserState.mTaskEngagementCI));
            mAffect.mAffectiveStates.Add(new AffectPerceptionInfo.AffectType(AffectPerceptionState.Attention, mUserState.mAttention, mUserState.mAttentionFocus, (int)mUserState.mArousalCI));

            mMutex[0].WaitOne();
            
            try
            {
                mThalamusClient.mInteractionAnalysisPublisher.UserState(mAffect.SerializeToJson());
            }
            catch { }

            mMutex[0].ReleaseMutex();
        }


/* THREAD 2 */
       
        public void SetupPipeConnection()
        {
            mPipeClient.PipeName = "\\\\.\\pipe\\affectperception";
            mPipeClient.MessageReceived += new Client.MessageReceivedHandler(PerceptionUpdateCallback);
            mPipeClient.Connect();

            while (mPipeClient.mIsConnected == false)
                mPipeClient.Connect();

            Thread.Sleep(1000);

            mPipeClient.SendMessage("AP_OK");
        }
       
 
        public void PerceptionUpdateCallback(string message)
        {
            string[] parseMsg = message.Split(';');
            
            mConftemp=mConftemp + Convert.ToDouble(parseMsg[1]);

            mArousaltemp = mArousaltemp + Convert.ToDouble(parseMsg[31]);

            mconfcounter++;


            mMutex[2].WaitOne();

            if(int.Parse(parseMsg[36]) != 0 )
                Console.Out.Write(".*.");
            else
                Console.Out.Write("..");
            
            int uC = 1;
            if (parseMsg.Count() > 39)
                uC = 1; // 2 // Force one user for now...

            if (AddLLI(parseMsg, uC))
                mProcessFlag = true;
            
            mMutex[2].ReleaseMutex(); 
        }

        
/* THREAD 3 */

        /* When the maximum number of LLI states has been reached, process the segment */
        public void ProcessSegment()
        {
            while(true)
            {
                mMutex[2].WaitOne();
                
                if (mProcessFlag == true)
                {
                    mProcessFlag = false;

                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Out.Write("\n****************************************");
                    Console.Out.Write("\n******** ");
                    Console.ResetColor();
                    Console.Out.Write("Interaction Analysis ");
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.Out.Write("**********");
                    Console.Out.Write("\n\n****************************************\n");
                    Console.ResetColor();
                    
                    // Extract features using the data we have...
                    string parseMs = System.Runtime.InteropServices.Marshal.PtrToStringAnsi(ExtractFeatures());

                    string[] featureVector = parseMs.Split('\t');
                    int count = featureVector.Count();
                                      
                    double[,] cls = new double[,] { { 0.75, 0.75 }, { 0.75, 0.75 }, { 0.75, 0.75 }, { 0.75, 0.75 }, { 0.75, 0.75 } };
                    
                    
                    
                    if (mInternalState.mState == Status.OKAO_ONLY)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Out.Write("\n\n****************************************");
                        Console.ResetColor();
                        Console.Out.Write("\n******* Basic Affective Output *********\n");

                        if (featureVector[90] == "0")
                            mUserState.mValence = Charge.Negative;
                        else if (featureVector[90] == "1")
                            mUserState.mValence = Charge.Neutral;
                        else if (featureVector[90] == "2")
                            mUserState.mValence = Charge.Positive;
                        if (mArousaltemp == 0) //no q sensor data, use OKAO instead for arousal
                        {
                            if (featureVector[91] == "0")
                                mUserState.mArousal = Charge.Negative;
                            else if (featureVector[91] == "1")
                                mUserState.mArousal = Charge.Neutral;
                            else if (featureVector[91] == "2")
                                mUserState.mArousal = Charge.Positive;
                        }
                    }

                    if (mInternalState.mModuleStatus == ModuleStatus.Ready)
                    {
                        //confidence here from OKAO
                        //mUserState.mValenceCI = 1000 * cls[0, 1];
                        //mUserState.mArousalCI = 1000 * cls[1, 1];
                        mConf = mConftemp / mconfcounter;

                        //arousal calc if q sensor sents data
                        if (mArousaltemp > 0)
                        {
                            oldArousal = currentArousal;
                            currentArousal = Math.Round((mArousaltemp / mconfcounter),2);

                            

                            //compare old arousal with new
                            if (currentArousal - thresArousal > oldArousal)
                            {//positive arousal
                                
                                mUserState.mArousal = Charge.Positive;
                            }
                            else if (currentArousal + thresArousal < oldArousal)
                            {//negative arousal
                                mUserState.mArousal = Charge.Negative;
                                
                            }
                            else //same as before
                            {
                                mUserState.mArousal = Charge.Neutral;
                                
                            }
                            if(currentArousal!=oldArousal)
                            thresArousal = Math.Abs(currentArousal - oldArousal) / 1;

                        }
                        

                        mUserState.mValenceCI =  mConf;
                        mUserState.mArousalCI = mConf; 
                        cls[0, 1] = mConf / 1000;//valence confidence
                        cls[1, 1] = mConf / 1000;//arousal confidence
                        //reset confidence for next interval
                        mConf = 0;
                        mConftemp = 0;
                        mconfcounter = 0;
                        Console.Out.Write("\n\n");
                        
                        if (mUserState.mValence == Charge.Negative)
                            Console.Out.WriteLine("\tV: Negative - CI: " + cls[0, 1].ToString("0.00"));
                        else if (mUserState.mValence == Charge.Neutral)
                            Console.Out.WriteLine("\tV: Neutral - CI: " + cls[0, 1].ToString("0.00"));
                        else if (mUserState.mValence == Charge.Positive)
                            Console.Out.WriteLine("\tV: Positive - CI: " + cls[0, 1].ToString("0.00"));

                        Console.Out.Write("\n");

                        if (mUserState.mArousal == Charge.Negative)
                            Console.Out.WriteLine("\tA: Negative - CI: " + cls[1, 1].ToString("0.00"));
                        else if (mUserState.mArousal == Charge.Neutral)
                            Console.Out.WriteLine("\tA: Neutral - CI: " + cls[1, 1].ToString("0.00"));
                        else if (mUserState.mArousal == Charge.Positive)
                            Console.Out.WriteLine("\tA: Positive - CI: " + cls[1, 1].ToString("0.00"));
                        Console.Out.WriteLine("\n");
                        if (mArousaltemp == 0)
                            Console.Out.WriteLine("\tNO Q Sensor data!!!");
                        else
                        {

                            Console.Out.WriteLine("\tQ Sensor data:");
                            Console.Out.WriteLine("\tOldArousal: " + oldArousal.ToString("0.00"));
                            Console.Out.WriteLine("\tThreshold: " + thresArousal.ToString("0.00"));
                            Console.Out.WriteLine("\tCurrentArousal: " + currentArousal.ToString("0.00"));
                        }
                        mArousaltemp = 0;
                    }
                    else
                        Console.Out.Write("\n\tInitialising...\n");

                    Console.ForegroundColor = ConsoleColor.DarkRed; 
                    Console.Out.WriteLine("\n****************************************");
                    Console.ResetColor();

                    
                    /////////////////////////////////////
                    // Has there been a change in state?
                    if (mUserState.Compare(mPreviousUserState))
                        UpdateLM();

                    mPreviousUserState = new UserState(mUserState);
                }

                mMutex[2].ReleaseMutex();
                Thread.Sleep(1000);
            }
        } 
        
/* THREAD 4 */

        public void KeyListener()
        {
            ConsoleKeyInfo cki;
            
            do
            {
                cki = Console.ReadKey();

                if (cki.Key == ConsoleKey.Spacebar && mInternalState.mState == Status.Full)
                {
                    mMutex[2].WaitOne();
                    if (mInternalState.mState == Status.OKAO_ONLY)
                        mInternalState.mState = Status.Full;
                    else
                        mInternalState.mState = Status.OKAO_ONLY;
                    mMutex[2].ReleaseMutex();
                }

            } while (cki.Key != ConsoleKey.Escape);
        }
    }
}