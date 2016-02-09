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

using System.Windows.Threading;
using System.Windows.Forms;
using PipeClient;
using EmoteEvents;
using EmoteCommonMessages;

namespace AffectPerception
{
    public partial class MainWindow : Window
    {
        Client mPipeClient;
        public AffectPerceptionInterfaceClient mThalamusClient;
        public MainWindow mMainWindow = null;

        public Charge mValenceCharge;
        public Charge mArousalCharge;
        public Charge mTaskEngagementCharge;
        public Charge mSocialEngagementCharge;
        public PointofFocus mAttentionFocus;

        public MainWindow()
        {
            InitializeComponent();

            mPipeClient = new Client();
            mPipeClient.MessageReceived +=
                new Client.MessageReceivedHandler(pipeClient_MessageReceived);

            mThalamusClient = new AffectPerceptionInterfaceClient(this);
        }

        void pipeClient_MessageReceived(string message)
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() => 
            {
                listlog.Items.Add(message);
                string[] parseMsg = message.Split(';');
            }));
           
        }

        private void ValenceSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.ValenceValue.Content = this.Valence_Slider.Value.ToString();
        }

        private void ArousalSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.ArousalValue.Content = this.Arousal_Slider.Value.ToString();
        }

        private void TaskEngagementSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.TaskEngagementValue.Content = this.TaskEngagement_Slider.Value.ToString();
        }

        private void SocialEngagementSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.SocialEngagementValue.Content = this.SocialEngagement_Slider.Value.ToString();
        }

        private void AttentionSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            this.AttentionValue.Content = this.Attention_Slider.Value.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!mPipeClient.Connected)
            {
                try
                {
                    mPipeClient.PipeName = "\\\\.\\pipe\\affectperception";
                    mPipeClient.Connect();

                    if (mPipeClient.Connected)
                        this.PipeStatus.Content = "Connection O.K !";
                    else
                        this.PipeStatus.Content = "Connection Failed !";
                }
                catch (Exception ex)
                {
                    this.PipeStatus.Content = "Connection Failed !";
                }
            }
            else
                this.PipeStatus.Content = "Already Connected!!";
        }

        private void OnFormClose(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (mPipeClient.Connected == true) mPipeClient.Stop();
            mThalamusClient.Dispose();
        }

        private void SendTestMessage(object sender, RoutedEventArgs e)
        {
            if (mPipeClient.Connected == true)
            {
                mPipeClient.SendMessage("AP_OK");
                this.PipeStatus.Content = "Now Connected!";
            }
        }

        public void SubmitAffectiveInfo(object sender, RoutedEventArgs e)
        {
                        
            AffectPerceptionInfo mAffect = new AffectPerceptionInfo();
            mAffect.mMapEventId = "";

            mAffect.mAffectiveStates.Add(new AffectPerceptionInfo.AffectType(AffectPerceptionState.Valence, mValenceCharge, PointofFocus.NonApplicable, (int)this.Valence_Slider.Value));
            mAffect.mAffectiveStates.Add(new AffectPerceptionInfo.AffectType(AffectPerceptionState.Arousal, mArousalCharge, PointofFocus.NonApplicable, (int)this.Arousal_Slider.Value));
            mAffect.mAffectiveStates.Add(new AffectPerceptionInfo.AffectType(AffectPerceptionState.Engagement, mTaskEngagementCharge, PointofFocus.Social, (int)this.SocialEngagement_Slider.Value));
            mAffect.mAffectiveStates.Add(new AffectPerceptionInfo.AffectType(AffectPerceptionState.Engagement, mSocialEngagementCharge, PointofFocus.Task, (int)this.TaskEngagement_Slider.Value));
            mAffect.mAffectiveStates.Add(new AffectPerceptionInfo.AffectType(AffectPerceptionState.Attention, Charge.Positive, mAttentionFocus, (int)this.Attention_Slider.Value));

            mThalamusClient.mAffectPerceptionPublisher.UserState(mAffect.SerializeToJson());
        }

        private void Charge_Changed(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.RadioButton ck = sender as System.Windows.Controls.RadioButton;
            if (ck.IsChecked.Value)
            {
                if (String.Compare(ck.Content.ToString(), "Positive",true) == 0)
                {
                    if (ck.GroupName == "valence")
                        mValenceCharge = Charge.Positive;
                    else if (ck.GroupName == "arousal")
                        mArousalCharge = Charge.Positive;
                    else if (ck.GroupName == "taskEngagement")
                        mTaskEngagementCharge = Charge.Positive;
                    else if (ck.GroupName == "socialEngagement")
                        mSocialEngagementCharge = Charge.Positive;
                }
                else if (String.Compare(ck.Content.ToString(), "Negative", true) == 0)
                {
                    if (ck.GroupName == "valence")
                        mValenceCharge = Charge.Negative;
                    else if (ck.GroupName == "arousal")
                        mArousalCharge = Charge.Negative;
                    else if (ck.GroupName == "taskEngagement")
                        mTaskEngagementCharge = Charge.Negative;
                    else if (ck.GroupName == "socialEngagement")
                        mSocialEngagementCharge = Charge.Negative;
                }
            }
        }
        private void Focus_Changed(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.RadioButton ck = sender as System.Windows.Controls.RadioButton;
            if (ck.IsChecked.Value)
            {
                if (String.Compare(ck.Content.ToString(), "Social", true) == 0)
                    mAttentionFocus = PointofFocus.Social;
                if (String.Compare(ck.Content.ToString(), "Task", true) == 0)
                    mAttentionFocus = PointofFocus.Task;
                if (String.Compare(ck.Content.ToString(), "Other", true) == 0)
                    mAttentionFocus = PointofFocus.Other;
            }
        } 
    }
}
