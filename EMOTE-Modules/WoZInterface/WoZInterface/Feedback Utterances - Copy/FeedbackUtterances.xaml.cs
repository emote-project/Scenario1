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
    /// <summary>
    /// Feedback Selector (User Control):  Allows the Wizard to slect from predefined Utterances
    /// </summary>
    public partial class FeedbackUtterances : UserControl
    {

        public WOZManager mWOZManager;
        public ListBox mListBox = new ListBox();
        private int mUtteranceCounter = 0;
        TaskStep mTaskStepData = new TaskStep();
        
        public FeedbackUtterances(WOZManager wozManager)
        {
            mWOZManager = wozManager;

            InitializeComponent();

            mListBox.SelectionChanged += new SelectionChangedEventHandler(SelectedUtterance);

            this.ScriptGrid.Children.Add(mListBox);
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


        //public int GetSkill()
        //{
        //    int skill = 0;

        //    if (distance.IsChecked == true && direction.IsChecked == false && symbol.IsChecked == false)
        //        skill = 1;
        //    else if (distance.IsChecked == false && direction.IsChecked == true && symbol.IsChecked == false)
        //        skill = 2;
        //    else if (distance.IsChecked == false && direction.IsChecked == false && symbol.IsChecked == true)
        //        skill = 3;
        //    else if (distance.IsChecked == true && direction.IsChecked == true && symbol.IsChecked == false)
        //        skill = 4;
        //    else if (distance.IsChecked == true && direction.IsChecked == false && symbol.IsChecked == true)
        //        skill = 5;
        //    else if (distance.IsChecked == false && direction.IsChecked == true && symbol.IsChecked == true)
        //        skill = 6;
        //    else if (distance.IsChecked == true && direction.IsChecked == true && symbol.IsChecked == true)
        //        skill = 7;

        //    return skill;
        //}

        /*
         * Strategy based on the feedback radio button selection 
         * ********************************************************************
         */
        public int GetStrategy()
        {
            int strategy = 1;

            if (ExcellentFeedback.IsChecked == true)
                strategy = 19;
            else if (GoodFeedback.IsChecked == true)
                strategy = 20;
            else if (NeutralFeedback.IsChecked == true)
                strategy = 21;
            else if (AlmostFeedback.IsChecked == true)
                strategy = 22;
            else if (NegativeFeedback.IsChecked == true)
                strategy = 23;

            return strategy;

        }

        /*
         * Refresh the utterances with the latest data
         * ********************************************************************
         */
        public void RefreshUtterances()
        {
            mUtteranceCounter = mWOZManager.SetUtterancesList(mWOZManager.mparticipantDetails, mTaskStepData, mListBox, UtteranceType.FEEDBACK, GetStrategy(), mWOZManager.mTaskScriptManager.GetActiveStepID(), GetSkill());
        }

        
        /*
         * Choose an utterance at random
         * ********************************************************************
         */
        private void SelectRandomUtterance()
        {
            Random rnd = new Random();
            int utteranceID = rnd.Next(0, mUtteranceCounter);

            ListBoxItem a = mListBox.Items.GetItemAt(utteranceID) as ListBoxItem;

            mWOZManager.SetSelectedUtterance((int)UtteranceType.FEEDBACK, a.Content.ToString());

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

                var uri = new Uri("pack://application:,,,/Images/Feedback/CorrectResponse.png");
                var bitmap = new BitmapImage(uri);
                ResponseIndicator.Source = bitmap;

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


                var uri = new Uri("pack://application:,,,/Images/Feedback/IncorrectResponse.png");
                var bitmap = new BitmapImage(uri);
                ResponseIndicator.Source = bitmap;
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

            var uri = new Uri("pack://application:,,,/Images/Feedback/WaitingResponse.png");
            var bitmap = new BitmapImage(uri);
            ResponseIndicator.Source = bitmap;

        }


        /*
        * Event Handlers
        * ********************************************************************
        */

        public void SelectedUtterance(object sender, RoutedEventArgs e)
        {

            ListBox lBox = (ListBox)sender;
            ListBoxItem lBoxItem = lBox.SelectedItem as ListBoxItem;

            if (lBoxItem != null)
                mWOZManager.SetSelectedUtterance((int)UtteranceType.FEEDBACK, lBoxItem.Content.ToString());
        }

        private void RefreshUtteranceList(object sender, RoutedEventArgs e)
        {
            RefreshUtterances();
        }

        private void ManualSelectUtterance(object sender, RoutedEventArgs e)
        {
            mWOZManager.SendUtteranceBP(mWOZManager.GetSelectedUtterance((int)UtteranceType.FEEDBACK));
        }

        private void AutoSelectUtterance(object sender, RoutedEventArgs e)
        {
            SelectRandomUtterance();
            mWOZManager.SendUtteranceBP(mWOZManager.GetSelectedUtterance((int)UtteranceType.FEEDBACK));
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

        

    }
}
