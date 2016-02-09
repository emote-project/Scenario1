using CaseBasedController.Detection;
using System.Collections.Generic;

namespace CaseBasedController.Behavior.Enercities
{
    /// <summary>
    ///     Performs a specified utterance.
    /// </summary>
    public class PerformCustomUtterance : BaseBehavior
    {

        public string Utterance { get; set; }

        string uttID = "";


        public PerformCustomUtterance()
        {
        }

        
        public override void Execute(IFeatureDetector detector)
        {
            //just perform the utterance
            lock (this.locker)
            {
                this.actionPublisher.PerformUtterance(uttID, Utterance, "custom");

                //TODO verify wait for utterance to finish?
                this.RaiseFinishedEvent(detector);
            }
        }

        public override void Cancel()
        {
            this.actionPublisher.CancelUtterance(uttID);
        }

        public override void Dispose()
        {
        }

        protected override void AttachPerceptionEvents()
        {
        }

        
    }
}