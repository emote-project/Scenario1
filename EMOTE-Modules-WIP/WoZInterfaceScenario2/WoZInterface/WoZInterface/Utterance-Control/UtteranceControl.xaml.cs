using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WOZInterface
{

    public partial class UtteranceControl : UserControl
    {
        // VARIABLES
        public WOZManager mWOZManager;
        //public ListBox mListBox = new ListBox();
        private int mUtteranceCounter = 0;
        TaskStep mTaskStepData = new TaskStep();

        List<UtteranceSet> mUtteranceContainerList = new List<UtteranceSet>();

        public ListView mListView = new ListView();



        public UtteranceControl(WOZManager wozManager)
        {
            mWOZManager = wozManager; 
            
            InitializeComponent();

            mListView.SelectionChanged += new SelectionChangedEventHandler(SelectedUtterance);
            mListView.Foreground = Brushes.White;
            mListView.Background = Brushes.Transparent;

            this.ScriptGrid.Children.Add(mListView);
        }

        public void SetTaskData(TaskStep taskstepdata)
        {
            mTaskStepData = taskstepdata;

            distance.Uid = mTaskStepData.mDistanceValue + " " + mTaskStepData.mDistanceUnit;
            direction.Uid = mTaskStepData.mDirectionValue;
            symbol.Uid = mTaskStepData.mSymbolValue;

        }

        
        /*
         * Skill based on the checkbox buttons selected 
         * ********************************************************************
         */
        public SkillSelection GetSkill()
        {

            SkillSelection skill = new SkillSelection();

            if (distance.IsChecked == true && direction.IsChecked == false && symbol.IsChecked == false)
            {
                skill.mSkill = 1;
                skill.mSkillText = "distance";
            }
            else if (distance.IsChecked == false && direction.IsChecked == true && symbol.IsChecked == false)
            {
                skill.mSkill = 2;
                skill.mSkillText = "direction";
            }
            else if (distance.IsChecked == false && direction.IsChecked == false && symbol.IsChecked == true)
            {
                skill.mSkill = 3;
                skill.mSkillText = "symbol";
            }
            else if (distance.IsChecked == true && direction.IsChecked == true && symbol.IsChecked == false)
            {
                skill.mSkill = 4;
                skill.mSkillText = "distance and direction";
            }
            else if (distance.IsChecked == true && direction.IsChecked == false && symbol.IsChecked == true)
            {
                skill.mSkill = 5;
                skill.mSkillText = "distance and symbol";
            }
            else if (distance.IsChecked == false && direction.IsChecked == true && symbol.IsChecked == true)
            {
                skill.mSkill = 6;
                skill.mSkillText = "direction and symbol";
            }
            else if (distance.IsChecked == true && direction.IsChecked == true && symbol.IsChecked == true)
            {
                skill.mSkill = 7;
                skill.mSkillText = "distance, direction and symbol";
            }

            return skill;
        }

        /*
         * Strategy based on the feedback radio button selection 
         * ********************************************************************
         */
        public int GetStrategy()
        {
            

            int strategy = 1;

            if (rb1.IsChecked == true)
                strategy = 1;
            else if (rb2.IsChecked == true)
                strategy = 2;
            else if (rb3.IsChecked == true)
                strategy = 3;
            else if (rb4.IsChecked == true)
                strategy = 4;
            else if (rb5.IsChecked == true)
                strategy = 5;
            else if (rb6.IsChecked == true)
                strategy = 6;
            else if (rb7.IsChecked == true)
                strategy = 7;
            else if (rb8.IsChecked == true)
                strategy = 8;

            else if (rebound.IsChecked == true)
                strategy = 10;
            else if (answer.IsChecked == true)
                strategy = 11;
            else if (pleased.IsChecked == true)
                strategy = 12;
            else if (help.IsChecked == true)
                strategy = 13;
            else if (backoff.IsChecked == true)
                strategy = 14;
            else if (technical.IsChecked == true)
                strategy = 15;
            else if (smalltalk.IsChecked == true)
                strategy = 16;
            else if (stall.IsChecked == true)
                strategy = 17;
            else if (unable.IsChecked == true)
                strategy = 18; 
            
            
            
            else if (ExcellentFeedback.IsChecked == true)
                strategy = 19;
            else if (GoodFeedback.IsChecked == true)
                strategy = 20;
            else if (NeutralFeedback.IsChecked == true)
                strategy = 21;
            else if (AlmostFeedback.IsChecked == true)
                strategy = 22;
            else if (NegativeFeedback.IsChecked == true)
                strategy = 23;

            else if (guidance.IsChecked == true)
                strategy = 25;

            return strategy;
        }

        
        
        /*
         * Refresh the utterances with the latest data
         * ********************************************************************
         */
        public void RefreshUtterances()
        {
            mUtteranceContainerList.Clear();
            
            mUtteranceCounter = mWOZManager.SetUtterancesList(mWOZManager.mparticipantDetails, mTaskStepData, UtteranceType.FEEDBACK, GetStrategy(), mWOZManager.mTaskScriptManager.GetActiveStepID(), mWOZManager.mActivityID, GetSkill(), mUtteranceContainerList);

            mListView.Items.Clear();

            foreach (UtteranceSet utSet in mUtteranceContainerList)
            {
                ListViewItem listViewItem = new ListViewItem();
                listViewItem.Content = utSet.mNonTaggedText;
                mListView.Items.Add(listViewItem);
            }
        }


        /* 
         * Update the interface so it contains the resonse the from the participant
         * ********************************************************************
         */
        public void SetUserResponseData(UserResponse userResponseData)
        {

            if (userResponseData.mCorrect)
            {
                // Set the icons to ticks...

                distance.Tag = "correct";
                direction.Tag = "correct";
                symbol.Tag = "correct";

            }
            else
            {
                if (userResponseData.mDistanceCorrect)
                    distance.Tag = "correct";
                else
                    distance.Tag = "wrong";

                if (userResponseData.mDistanceCorrect)
                    direction.Tag = "correct";
                else
                    direction.Tag = "wrong";

                if (userResponseData.mSymbolCorrect)
                    symbol.Tag = "correct";
                else
                    symbol.Tag = "wrong";

            }

            distance.Content = userResponseData.mDistance;
            direction.Content = userResponseData.mDirection;
            symbol.Content = userResponseData.mSymbol;

        }

        public void ResetResponseData()
        {
            //distance.Tag = "waiting";
            //direction.Tag = "waiting";
            //symbol.Tag = "waiting";

            if (mTaskStepData.mDistanceValue > 0)
                distance.Uid = mTaskStepData.mDistanceValue + " " + mTaskStepData.mDistanceUnit;
            else
                distance.Uid = "Not Required!";

            if (mTaskStepData.mDirectionValue.Length == 0)
                direction.Uid = "Not Required!";
            else
                direction.Uid = mTaskStepData.mDirectionValue;

            if (mTaskStepData.mSymbolValue.Length == 0)
                symbol.Uid = "Not Required!";
            else
                symbol.Uid = mTaskStepData.mSymbolValue;

            distance.Content = "Waiting";
            direction.Content = "Waiting";
            symbol.Content = "Waiting";

        }
        
        
        /*
         * 
         */

        public void SelectedUtterance(object sender, RoutedEventArgs e)
        {
            e.Handled = true;

            if (mUtteranceCounter > 0)
            {

                ListView lBox = (ListView)sender;
                ListViewItem lBoxItem = lBox.SelectedItem as ListViewItem;
                
                int i = lBox.SelectedIndex;

                if (lBoxItem != null)
                {
                    mWOZManager.SendUtteranceBP(mUtteranceContainerList[i].mText);

                }
            }
        }

        private void RefreshUtteranceList(object sender, RoutedEventArgs e)
        {
            RefreshUtterances();
        }

        private void AutoSelectUtterance(object sender, RoutedEventArgs e)
        {
            e.Handled = true; 
            
            if (mUtteranceCounter > 1)
            {
                Random rnd = new Random();
                int utteranceID = rnd.Next(0, mUtteranceCounter);
                
                mWOZManager.SendUtteranceBP(mUtteranceContainerList.ElementAt(utteranceID).mText);
            }
            else if (mUtteranceCounter == 1 )
            {
                mWOZManager.SendUtteranceBP(mUtteranceContainerList.ElementAt(0).mText);
            }
        }
        
        private void SelectAllSkills(object sender, RoutedEventArgs e)
        {
            distance.IsChecked = true;
            direction.IsChecked = true;
            symbol.IsChecked = true;

            RefreshUtterances();
        }

        private void ClearAllSkills(object sender, RoutedEventArgs e)
        {
            distance.IsChecked = false;
            direction.IsChecked = false;
            symbol.IsChecked = false;

            RefreshUtterances();
        }


        /*
         * Tools Control
         */

        private void DistanceOpen(object sender, RoutedEventArgs e)
        {
            mWOZManager.mThalamusClient.WOZPublisher.DistanceShow();
        }

        private void DistanceClose(object sender, RoutedEventArgs e)
        {
            mWOZManager.mThalamusClient.WOZPublisher.DistanceHide();
        }

        private void DirectionOpen(object sender, RoutedEventArgs e)
        {
            mWOZManager.mThalamusClient.WOZPublisher.CompassShow();
        }

        private void DirectionClose(object sender, RoutedEventArgs e)
        {
            mWOZManager.mThalamusClient.WOZPublisher.CompassHide();
        }

        private void SymbolOpen(object sender, RoutedEventArgs e)
        {
            mWOZManager.mThalamusClient.WOZPublisher.MapKeyShow();
        }

        private void SymbolClose(object sender, RoutedEventArgs e)
        {
            mWOZManager.mThalamusClient.WOZPublisher.MapKeyHide();
        }

        private void ScrapBookOpen(object sender, RoutedEventArgs e)
        {
            mWOZManager.mThalamusClient.WOZPublisher.ToolAction("scrapBook", "show");
        }

        private void ScrapBookClose(object sender, RoutedEventArgs e)
        {
            mWOZManager.mThalamusClient.WOZPublisher.ToolAction("scrapBook", "hide");
        }

        
        /// <summary>
        /// //Free Speech
        /// </summary>
        
        
        public void Say()
        {
            if (freespeechtext.Text.Length != 0)
            {
                mWOZManager.SendUtteranceBP(freespeechtext.Text);
                freespeechtext.Text = "";

            }
        }



        private void SayHandler(object sender, RoutedEventArgs e)
        {
            Say();
        }

        private void OnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Say();
            }
        }
    }
}
