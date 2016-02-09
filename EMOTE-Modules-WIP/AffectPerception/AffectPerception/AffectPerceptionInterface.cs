using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Thalamus;
using EmoteEvents;
using EmoteCommonMessages;


namespace AffectPerception
{

    public interface IAffectPerception : EmoteCommonMessages.ILearnerModelToAffectPerceptionEvents
    { }

    public interface IAffectPerceptionPublisher : EmoteCommonMessages.IAffectPerceptionEvents
    { }

    public class AffectPerceptionInterfaceClient : ThalamusClient, IAffectPerception
    {
        internal class AffectPerceptionPublisher : IAffectPerceptionPublisher
        {
            dynamic publisher;
            public AffectPerceptionPublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }

            public void ProbeRequest(EmoteCommonMessages.Probes type, int urgency)
            {
                publisher.ProbeRequest(type, urgency);
            }

            public void UserState(string AffectPerceptionInfo_AffectiveStates)
            {
                publisher.UserState(AffectPerceptionInfo_AffectiveStates);
            }
        }

        MainWindow form;
        internal AffectPerceptionPublisher mAffectPerceptionPublisher;
        public AffectPerceptionInterfaceClient(MainWindow form, string character = "")
            : base("AffectPerception", character)
        {
            this.form = form;
            SetPublisher<AffectPerceptionPublisher>();
            mAffectPerceptionPublisher = new AffectPerceptionPublisher(Publisher);
        }


        void EmoteCommonMessages.ILearnerModelToAffectPerceptionEvents.learnerModelValueUpdateBeforeAffectPerceptionUpdate(string LearnerStateInfo_learnerState)
        {
            LearnerStateInfo lsi = LearnerStateInfo.DeserializeFromJson(LearnerStateInfo_learnerState);
            form.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                form.learnerID.Text = lsi.learnerId.ToString();
                form.stepID.Text = lsi.stepId.ToString();
                form.activityID.Text = lsi.activityId.ToString();
                form.scenarioID.Text = lsi.scenarioId.ToString();
                form.sessionID.Text = lsi.sessionId.ToString();


                AffectPerceptionInfo mAffect = new AffectPerceptionInfo();
                mAffect.mMapEventId = lsi.mapEventId;

                mAffect.mAffectiveStates.Add(new AffectPerceptionInfo.AffectType(AffectPerceptionState.Valence, form.mValenceCharge, PointofFocus.NonApplicable, (int)form.Valence_Slider.Value));
                mAffect.mAffectiveStates.Add(new AffectPerceptionInfo.AffectType(AffectPerceptionState.Arousal, form.mArousalCharge, PointofFocus.NonApplicable, (int)form.Arousal_Slider.Value));
                mAffect.mAffectiveStates.Add(new AffectPerceptionInfo.AffectType(AffectPerceptionState.Engagement, form.mTaskEngagementCharge, PointofFocus.Social, (int)form.SocialEngagement_Slider.Value));
                mAffect.mAffectiveStates.Add(new AffectPerceptionInfo.AffectType(AffectPerceptionState.Engagement, form.mSocialEngagementCharge, PointofFocus.Task, (int)form.TaskEngagement_Slider.Value));
                mAffect.mAffectiveStates.Add(new AffectPerceptionInfo.AffectType(AffectPerceptionState.Attention, Charge.Positive, form.mAttentionFocus, (int)form.Attention_Slider.Value));

                mAffectPerceptionPublisher.UserState(mAffect.SerializeToJson());

            }));
        }
    }
}
