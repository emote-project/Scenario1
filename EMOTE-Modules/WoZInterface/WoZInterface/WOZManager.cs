﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows;

namespace WOZInterface
{
    public enum TaskState : int
    {
        UNINTIALISED,
        READY,
        STARTED,
        STOPPED
    }
     
    public enum GazeDirection : int
    {
        PARTICIPANT = 0,
        CLICKS = 1,
        RANDOM = 2,
        SYMBOL = 3
    }

    public enum PointDirection : int
    {
        TOOLS = 0,
        SYMBOL = 1
    }

    public enum GlanceDirection : int
    {
        TABLE2PARTICIPANT = 0,
        PARTICIPANT2TABLE = 1
    }

    public class UserResponse
    {
        public int mStepId;
        public bool mCorrect;
        public bool mDistanceCorrect;
        public bool mDirectionCorrect;
        public bool mSymbolCorrect;
        public double mDistanceSkillLevel;
        public double mDirectionSkillLevel;
        public double mSymbolSkillLevel;

        public string mDistance = "";
        public string mDirection = "";
        public string mSymbol = "";

    }

    public class participantDetails
    {
        public string mName = "Default";
        public string mID = "99";
    }

    public class ReQuestion
    {
        public int[] mTotalRequestions = new int[15];
        public int mRequestionCounter = 0;
    }
    
    
    public class WOZManager : Selector
    {

        public int mActivityID = 12;

        public LoggingManager mLoggingManager = new LoggingManager();

        public WOZInterfaceClient mThalamusClient;
        
        public CameraManager[] mCameraControl = new CameraManager[3];
        public ControlPanel mControlPanel = null;
        public TaskScriptManager mTaskScriptManager;
        public Competence mCompetence = null;

        public UtteranceControl mUtteranceControl = null;

        public ToolsControl mToolsControl = null;
        
        public MainWindow mMainWindow = null;

        public participantDetails mparticipantDetails = new participantDetails();
        public TaskState mTaskState;

        public string[,] mStepSpecificUtterances = new string[15, 5];

        ReQuestion mRequestion = new ReQuestion();

        int mProbeCount = 0;

        public WOZManager(MainWindow window)
        {
            mMainWindow = window;

            CheckScreenSize();
            
            mTaskState = new TaskState();
            mTaskState = TaskState.UNINTIALISED;

            ReadUtterances(GetResourcesPath() + "XML\\utterancesV18.XML");
            
            mTaskScriptManager = new TaskScriptManager(this, mMainWindow.TaskScriptHolder);

            mControlPanel = new ControlPanel(this);
            mMainWindow.ControlPanelHolder.Children.Add(mControlPanel);


            mUtteranceControl = new UtteranceControl(this);
            mMainWindow.UtteranceControlHolder.Children.Add(mUtteranceControl);

            mToolsControl = new ToolsControl(this);
            mMainWindow.CompetenceBoxHolder.Children.Add(mToolsControl);

            mCameraControl[0] = new CameraManager(1, this, mMainWindow.CameraHolder1);
            mCameraControl[1] = new CameraManager(2, this, mMainWindow.CameraHolder2);
            mCameraControl[2] = new CameraManager(3, this, mMainWindow.CameraHolder3, 480, 640);

            //mCompetence = new Competence(this);
            //mMainWindow.CompetenceBoxHolder.Children.Add(mCompetence);


            mThalamusClient = new WOZInterfaceClient(mMainWindow);

            mTaskScriptManager.Init();
        }

        
        private void CheckScreenSize()
        {
            if (System.Windows.SystemParameters.PrimaryScreenHeight < 1080 || System.Windows.SystemParameters.PrimaryScreenWidth < 1920)
            {
                mMainWindow.Close();
            }
        }


        public string GetResourcesPath()
        {
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string commonPath = basePath.Remove(basePath.Length - @"bin\debug\".Length);
        return (commonPath + @"Assets\");
        }


        private void InitWOZManager()
        {
            mTaskScriptManager.SetActiveStep(0);

            mTaskScriptManager.mScriptViewer.SetTaskSteps();
            
            TaskStep taskStep = mTaskScriptManager.GetActiveStepData();

            ResetUtteranceControl();
            mUtteranceControl.RefreshUtterances();
            
    
            mTaskState = TaskState.READY;
        }

        private TaskStep ResetUtteranceControl()
        {
            TaskStep taskStep = mTaskScriptManager.GetActiveStepData(); 
            mUtteranceControl.SetTaskData(taskStep);
            mUtteranceControl.ResetResponseData();

            return taskStep;
        }

        
        /*
         * 
         */
        public void WOZStart(string name, string id)
        {

            mLoggingManager.Init(int.Parse(id), name);
            mLoggingManager.AddLogItem(mTaskScriptManager.GetActiveStepID(), 1, 0, "", 0, 0, 0, 0, 0, 0, 0, "", "", "");

            mparticipantDetails.mName = name;
            mparticipantDetails.mID = id;

            mTaskScriptManager.Init();
            InitWOZManager();

            SetSkillUtterances(mparticipantDetails, mRequestion.mTotalRequestions, mStepSpecificUtterances, mTaskScriptManager.GetActiveStepID());
            mUtteranceControl.RefreshUtterances();

            mThalamusClient.WOZPublisher.Start(int.Parse(id), name);

            for (int i = 0; i < 3; i++)
            {
                if(mCameraControl[i].mCameraStatus == CameraStatus.READY)
                    mCameraControl[i].StartRecording();
            }

            
            mTaskScriptManager.mScriptViewer.RepeatQuestionButton.Visibility = Visibility.Visible;
            mTaskScriptManager.mScriptViewer.NextTaskStepButton.Visibility = Visibility.Visible;

            
            mTaskState = TaskState.STARTED;

                
        }

        /*
         * 
         */
        public void WOZStop(int log = 1)
        {
            mThalamusClient.WOZPublisher.Stop();

            for (int i = 0; i < 3; i++)
            {
                if (mCameraControl[i].mCameraStatus == CameraStatus.RECORDING)
                    mCameraControl[i].StopRecording();
            }

            mTaskState = TaskState.READY;
            mTaskScriptManager.mScriptViewer.RepeatQuestionButton.Visibility = Visibility.Hidden;
            //mTaskScriptManager.mScriptViewer.AutoRequestionButton.Visibility = Visibility.Hidden;
            mTaskScriptManager.mScriptViewer.NextTaskStepButton.Visibility = Visibility.Hidden;
            //mTaskScriptManager.mScriptViewer.SendProbeButton.Visibility = Visibility.Hidden;
            
            InitWOZManager();
            //mTaskScriptManager.Reset();

            if (log == 1)
            {
                mLoggingManager.AddLogItem(mTaskScriptManager.GetActiveStepID(), 2, 0, "", 0, 0, 0, 0, 0, 0, 0, "", "", "");
                mLoggingManager.WriteLogFile();
                mLoggingManager.ClearLog();
            }

            ResetProbes();
        }


        
        
        /*
         * 
         * TASK STEP SPECIFIC
         * 
         */
        
        /* Advance to the next task step */
        public void AdvanceTaskStep()
        {
            mTaskScriptManager.SetActiveStep(mTaskScriptManager.GetActiveStepID() + 1);

            TaskStep taskStep = ResetUtteranceControl();

            // Send the Question from this step to the BP.
            SendUtteranceBP(taskStep.mStepSpeech);
            
            //mThalamusClient.WOZPublisher.StartNextStep();
            mThalamusClient.WOZPublisher.StartStep(mTaskScriptManager.GetActiveStepID());
           
            mRequestion.mRequestionCounter = 0;


            if (taskStep.mQuestion.Length != 0)
                mMainWindow.question.Content = taskStep.mQuestion.ToString();
            else
                mMainWindow.question.Content = "--------------------------------------";

            mUtteranceControl.RefreshUtterances();

                       
            /// Is it time for a probe
            //if (mTaskScriptManager.GetActiveStepID() == 20 || mTaskScriptManager.GetActiveStepID() == 25 || mTaskScriptManager.GetActiveStepID() == 31)
            //{
            //    mTaskScriptManager.mScriptViewer.NextTaskStepButton.Visibility = Visibility.Hidden;
            //    mTaskScriptManager.mScriptViewer.SendProbeButton.Visibility = Visibility.Visible;
            //}
         }

        public void StartTask()
        {
            TaskStep taskStep = ResetUtteranceControl();
            
            // Send the Question from this step to the BP.
            SendUtteranceBP(taskStep.mStepSpeech);
        }

        
        public void RepeatQuestion()
        {
            TaskStep taskStep = ResetUtteranceControl();

            // Send the Question from this step to the BP.
            SendUtteranceBP(taskStep.mStepSpeech);  
        }

        public void ReQuestion()
        {

            int stepID = mTaskScriptManager.GetActiveStepID() - mActivityID;
            
            SendUtteranceBP(mStepSpecificUtterances[stepID, mRequestion.mRequestionCounter]);
            
            if (mRequestion.mRequestionCounter > mRequestion.mTotalRequestions[stepID])
                mRequestion.mRequestionCounter = 0;
            else
                mRequestion.mRequestionCounter++;
        }

        public void RestartTaskStep()
        {
            TaskStep taskStep = ResetUtteranceControl();

            // Send the Question from this step to the BP.
            SendUtteranceBP(taskStep.mStepSpeech);
        }


        public void ResetProbes()
        {
            mProbeCount = 0;
        }
        
        
        public void RequestProbeEvent()
        {
            mProbeCount++;
            SendUtteranceBP(" Please take this time to fill out these few questions");
            mThalamusClient.WOZPublisher.GiveQuestionnaire("Probe" + mProbeCount);

            mTaskScriptManager.mScriptViewer.NextTaskStepButton.Visibility = Visibility.Visible;
           // mTaskScriptManager.mScriptViewer.SendProbeButton.Visibility = Visibility.Hidden;

        }

        /*
         * Methods for sending data to Thalamus
         * 
         */
        public void SendUtteranceBP(string utterance)
        {
           // MessageBox.Show(utterance);
            
            
            mThalamusClient.WOZPublisher.PerformUtterance(utterance);
            mLoggingManager.AddLogItem(mTaskScriptManager.GetActiveStepID(), 0, 1, utterance, 0, 0, 0, 0, 0, 0, 0, "", "", "");
        }
    }
}
