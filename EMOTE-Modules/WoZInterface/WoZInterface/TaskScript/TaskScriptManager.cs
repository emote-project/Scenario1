using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Controls;
using System.Windows;







namespace WOZInterface
{

    public class TaskStep
    {
        public int mId;
        public String  mStepId;
        public String  mTrailId;

        public String mStepSpeech;
        public String  mQuestion;

        public Boolean mDistanceReq;
        public String  mDistanceUnit;
        public Double  mDistanceValue;

        public Boolean mDirectionReq;
        public String  mDirectionUnit;
        public String  mDirectionValue;

        public Boolean mSymbolReq;
        public String  mSymbolValue;

        public Boolean mComplete;

        public Boolean mIsActive;
        public int mRowID;
        public int mColumnID;
        public String mLabel;
        public Boolean mIsHeading;

        public TaskStep()
        {
            mId = 0;
            mStepId = "";
            mTrailId = "";
            mQuestion = "";
            mDistanceReq = false;
            mDistanceUnit = "";
            mDistanceValue = 0.0;
            mDirectionReq = false;
            mDirectionUnit = "";
            mDirectionValue = "";
            mSymbolReq = false;
            mSymbolValue = "";
            mComplete = false;
            mIsActive = false;
            mRowID = 0;
            mColumnID = 0;
            mLabel = "";
            mIsHeading = false;
        }
        
    }

    
    public class TaskScriptManager
    {
        public WOZManager mWOZManager;
        public Grid MyHolder = null;
        public TaskScriptViewer mScriptViewer = null;

        public List<TaskStep> mTaskSteps;
        public int mActiveStepID;

        public int mTotalTaskSteps = 0;

        public Boolean mTaskStarted = false;

        public TaskScriptManager(WOZManager wozManager, Grid holder)
        {
            mWOZManager = wozManager;
            MyHolder = holder;

            mScriptViewer = new TaskScriptViewer(this);

            MyHolder.Children.Add(mScriptViewer);
        }


        public void Init()
        {
            ReadScript(mWOZManager.GetResourcesPath() + "XML\\bristol.xml");
            mScriptViewer.SetTaskSteps();

            mScriptViewer.NextTaskStepButton.Content = "Start";
            mTaskStarted = false;
        }

        public void Reset()
        {
            mTaskSteps.Clear();
        }
        
        public void SetActiveStep(int stepID)
        {
            if (stepID < mTotalTaskSteps)
            {
                mActiveStepID = stepID;

                //if (mActiveStepID == mWOZManager.mActivityID)
                //{
                //   mScriptViewer.AutoRequestionButton.Visibility = Visibility.Visible;
                //}
            }
            else
            {
                MessageBox.Show("The end of the task script has been reached");
            }
        }

        public int GetActiveStepID()
        {
            return mActiveStepID;
        }

        public TaskStep GetActiveStepData()
        {
            return mTaskSteps.ElementAt(GetActiveStepID());
        }

        public void AdvanceTaskStep()
        {
            mWOZManager.AdvanceTaskStep();
        }

     
        public void ReadScript(string filename)
        {
            mTotalTaskSteps = 0; 
            
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filename);

            XmlNodeList nodes = xmlDocument.SelectNodes("/scenario/activities/steps");

            mTaskSteps = new List<TaskStep>();

            int rowCount = 0;
            int stepCounter = 0;
            string trailID = "z";

            foreach (XmlNode node in nodes)
            {

                TaskStep stepInfo = new TaskStep();

                stepInfo.mId = int.Parse(node.Attributes["id"].Value);

                stepInfo.mStepId = node.Attributes["id"].InnerText;
                stepInfo.mTrailId = node.SelectSingleNode("trail-id").InnerText;

                if (node.SelectSingleNode("step-speech") != null)
                {
                    stepInfo.mStepSpeech = node.SelectSingleNode("step-speech").InnerText;
                    stepInfo.mStepSpeech = stepInfo.mStepSpeech.Replace("/student/", mWOZManager.mparticipantDetails.mName);
                }
                else
                {
                    stepInfo.mStepSpeech = "none";
                }


                // Question
                stepInfo.mQuestion = node.SelectSingleNode("name").InnerText;

                if (stepInfo.mTrailId.Equals(trailID) ==  false)
                {
                    stepCounter = 0;
                    trailID = stepInfo.mTrailId;
                }



                if (node.SelectSingleNode("distance") != null)
                {
                    // Distance
                    stepInfo.mDistanceReq = Convert.ToBoolean(node.SelectSingleNode("distance").InnerText);
                    if (stepInfo.mDistanceReq)
                    {
                        stepInfo.mDistanceUnit = node.SelectSingleNode("distance-units").InnerText;
                        stepInfo.mDistanceValue = double.Parse(node.SelectSingleNode("distance-required").InnerText);
                    }
                }

                if (node.SelectSingleNode("direction") != null)
                {
                    // Direction
                    stepInfo.mDirectionReq = Convert.ToBoolean(node.SelectSingleNode("direction").InnerText);
                    if (stepInfo.mDirectionReq)
                    {
                        stepInfo.mDirectionUnit = node.SelectSingleNode("direction-units").InnerText;
                        stepInfo.mDirectionValue = node.SelectSingleNode("direction-required").InnerText;
                    }
                }

                if (node.SelectSingleNode("symbol") != null)
                {
                    // Symbol
                    stepInfo.mSymbolReq = Convert.ToBoolean(node.SelectSingleNode("symbol").InnerText);
                    if (stepInfo.mSymbolReq)
                    {
                        stepInfo.mSymbolValue = node.SelectSingleNode("symbol-name-required").InnerText;
                    }
                }

                stepInfo.mComplete = false;


                /* Add the step to the list */
                rowCount++;
                stepCounter++;

                stepInfo.mLabel = "Step " + stepCounter;// stepInfo.mStepId;
                stepInfo.mRowID = rowCount;
                stepInfo.mColumnID = 4;
                stepInfo.mIsHeading = false;

                /* Set the first Step as Active */
                if (stepInfo.mId == GetActiveStepID())
                {
                    stepInfo.mIsActive = true;
                }

                mTaskSteps.Add(stepInfo);

                mTotalTaskSteps++;

            }

        }        
    }


}
