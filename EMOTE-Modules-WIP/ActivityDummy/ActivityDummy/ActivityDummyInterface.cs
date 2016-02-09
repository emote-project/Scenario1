using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Thalamus;
using EmoteEvents;
using EmoteCommonMessages;

namespace ActivityDummy
{
    public interface IActivityDummy : EmoteCommonMessages.IAffectPerceptionEvents
    { }

    public interface IActivityDummyPublisher : EmoteCommonMessages.ILearnerModelToAffectPerceptionEvents
    { }

    public class ActivityDummyInterface : ThalamusClient, IActivityDummy
    {
        internal class ActivityDummyPublisher : IActivityDummyPublisher
        {
            dynamic publisher;
            public ActivityDummyPublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }


            public void learnerModelValueUpdateBeforeAffectPerceptionUpdate(string LearnerStateInfo_learnerState)
            {
                publisher.learnerModelValueUpdateBeforeAffectPerceptionUpdate(LearnerStateInfo_learnerState);
            }
        }
        
        MainWindow form;
        internal IActivityDummyPublisher mIActivityDummyPublisher;
        public ActivityDummyInterface(MainWindow form, string character = "")
            : base("Activity Dummy", character)
        {
            this.form = form;
            SetPublisher<IActivityDummyPublisher>();
            mIActivityDummyPublisher = new ActivityDummyPublisher(Publisher);
        }


        void IAffectPerceptionEvents.ProbeRequest(Probes type, int urgency)
        {
            int i = 0;
        }

        void IAffectPerceptionEvents.UserState(string AffectPerceptionInfo_AffectiveStates)
        {
            int i = 0;
        }
    }
}
