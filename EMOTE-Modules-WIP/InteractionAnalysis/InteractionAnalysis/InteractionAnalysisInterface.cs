using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;
using EmoteEvents;
using EmoteCommonMessages;


namespace IA
{
    public interface IInteractionAnalysis : EmoteCommonMessages.ILearnerModelToAffectPerceptionEvents
    { }

    public interface IInteractionAnalysisPublisher : EmoteCommonMessages.IAffectPerceptionEvents
    { }

    class InteractionAnalysisInterfaceClient : ThalamusClient, IInteractionAnalysis
    {
        internal class InteractionAnalysisPublisher : IInteractionAnalysisPublisher
        {
            dynamic publisher;
            public InteractionAnalysisPublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }


            public void ProbeRequest(Probes type, int urgency)
            {
                publisher.ProbeRequest(type, urgency);
            }

            public void UserState(string AffectPerceptionInfo_AffectiveStates)
            {
                publisher.UserState(AffectPerceptionInfo_AffectiveStates);
            }
        }

        InteractionAnalysis mIA;
        internal InteractionAnalysisPublisher mInteractionAnalysisPublisher;
        public InteractionAnalysisInterfaceClient(InteractionAnalysis program, string character = "")
            : base("InteractionAnalysis", character)
        {
            this.mIA = program;
            SetPublisher<InteractionAnalysisPublisher>();
            mInteractionAnalysisPublisher = new InteractionAnalysisPublisher(Publisher);
        }

        public override void Dispose()
        {
            base.Dispose();         
        }

        void ILearnerModelToAffectPerceptionEvents.learnerModelValueUpdateBeforeAffectPerceptionUpdate(string LearnerStateInfo_learnerState)
        {
            LearnerStateInfo lsi = LearnerStateInfo.DeserializeFromJson(LearnerStateInfo_learnerState);
          
            mIA.TaskUpdateCallback(lsi);
        }
    }
}
