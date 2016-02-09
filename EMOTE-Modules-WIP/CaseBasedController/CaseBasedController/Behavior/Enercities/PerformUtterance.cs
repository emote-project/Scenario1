using CaseBasedController.Detection;
using System.Collections.Generic;
using CaseBasedController.Thalamus;
using EmoteEnercitiesMessages;

namespace CaseBasedController.Behavior.Enercities
{
    /// <summary>
    ///     Performs a specified utterance.
    /// </summary>
    public class PerformUtterance : BaseBehavior
    {
        public bool FinishImmediately = false;

        public PerformUtterance(bool finishImmediately = false)
        {
            this.Subcategory = this.Category = string.Empty;
            FinishImmediately = finishImmediately;
        }

        /// <summary>
        ///     The category of the specified
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        ///     The subcategory of the specified
        /// </summary>
        public string Subcategory { get; set; }

        string uttID = "";
        IFeatureDetector _detector;

        public override void Execute(IFeatureDetector detector)
        {
            _detector = detector;
            //just perform the utterance
            lock (this.locker)
            {
                System.Threading.Thread.Sleep(300);                                         // Waits to be sure that GameStatus is updated by all the coming events
                var tagsAndValues = GameInfo.GameStatus.GetTagNamesAndValues();
                uttID = "cbc"+ new System.Random().Next();
                string[] tags = new List<string>(tagsAndValues.Keys).ToArray();
                string[] values = new List<string>(tagsAndValues.Values).ToArray();
                this.actionPublisher.PerformUtteranceFromLibrary(uttID, Category, Subcategory,tags ,values);
                System.Console.WriteLine("Performing utterance: " + Category + ":" + Subcategory);

                if (FinishImmediately)
                {
                    this.RaiseFinishedEvent(_detector);
                }
            }
        }

        public override string ToString()
        {
            return "PerformUtterance:"+Category+":"+Subcategory;
        }

        public override void Cancel()
        {
            this.actionPublisher.CancelUtterance(uttID);
        }

        public override void Dispose()
        {
            perceptionClient.UtteranceFinishedEvent -= perceptionClient_SpeakFinishedEvent;
        }

        void perceptionClient_SpeakFinishedEvent(object sender, IFMLUtteranceEventArgs e)
        {
            if (e.Id.Equals(uttID))
            {
                this.RaiseFinishedEvent(_detector);
            }
        }

        protected override void AttachPerceptionEvents()
        {
            perceptionClient.UtteranceFinishedEvent += perceptionClient_SpeakFinishedEvent;
        }



    }
}