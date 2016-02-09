using EmoteEvents;
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

using EmoteCommonMessages;

namespace ActivityDummy
{
    public partial class MainWindow : Window
    {
        public ActivityDummyInterface mThalamusClient;

        LearnerStateInfo mLearnerStateInfo = new LearnerStateInfo();
        

        public MainWindow()
        {
            InitializeComponent();
            mThalamusClient = new ActivityDummyInterface(this);
        }

        private void OnFormClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mThalamusClient.Dispose();
        }

        private void PublishButtonClick(object sender, RoutedEventArgs e)
        {

            mLearnerStateInfo.learnerId = int.Parse(this.learnerID.Text);
            mLearnerStateInfo.stepId = int.Parse(this.stepID.Text);
            mLearnerStateInfo.activityId = int.Parse(this.activityID.Text);
            mLearnerStateInfo.scenarioId = int.Parse(this.scenarioID.Text);
            mLearnerStateInfo.sessionId = int.Parse(this.sessionID.Text);
            mLearnerStateInfo.mapEventId = this.mapEventID.Text;          
            

            //mLearnerStateInfo.correct = this.isCorrect;
            
            // if(this.
            
            //  LearnerStateInfo.CompetencyItem tmpCompetencyItem = new LearnerStateInfo.CompetencyItem("distance", this.

            mThalamusClient.mIActivityDummyPublisher.learnerModelValueUpdateBeforeAffectPerceptionUpdate(mLearnerStateInfo.SerializeToJson());


        }

        private void Distance_Checked(object sender, RoutedEventArgs e)
        {
            
        }

        private void ReasonIsAnswerAttempt(object sender, RoutedEventArgs e)
        {
            if (this.answerattempt.IsSelected)
                mLearnerStateInfo.reasonForUpdate = LearnerModelUpdateReason.StepAnswerAttempt;
            else if (this.toolUse.IsSelected)
                mLearnerStateInfo.reasonForUpdate = LearnerModelUpdateReason.ToolUse;
        }

        private void ReasonIsToolUse(object sender, RoutedEventArgs e)
        {

        }

        



    }
}
