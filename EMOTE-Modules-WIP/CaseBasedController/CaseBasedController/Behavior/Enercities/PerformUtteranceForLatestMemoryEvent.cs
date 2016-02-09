using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaseBasedController.Detection;
using EmoteEvents;

namespace CaseBasedController.Behavior.Enercities
{
    class PerformUtteranceForLatestMemoryEvent : BaseBehavior
    {
        string uttID = "";
        IFeatureDetector _detector;


        public override void Execute(IFeatureDetector detector)
        {
            lock (this.locker)
            {
                _detector = detector;
                System.Threading.Thread.Sleep(300);                                         // Waits to be sure that GameStatus is updated by all the coming events

                List<MemoryEvent.MemoryEventItem> memoryEventItems = GameInfo.GameStatus.CurrentState.MemoryEventData.memoryEventItems;
                foreach (var mei in memoryEventItems)
                {
                    string category = mei.category;
                    string subcategory =
                        mei.subcategory;

                    var tagsAndValues = GameInfo.GameStatus.GetTagNamesAndValues();
                    uttID = "cbc" + new System.Random().Next();

                    var tagsList = new List<string>(tagsAndValues.Keys);
                    tagsList.AddRange(
                        mei.tagNames.ToList());
                    string[] tags = tagsList.ToArray();

                    var valuesList = new List<string>(tagsAndValues.Values);
                    valuesList.AddRange(
                        mei.tagNames.ToList());
                    string[] values = valuesList.ToArray();

                    actionPublisher.PerformUtterance(uttID,"The learner model says that now I should talk about the category "+category+" and subcategory "+subcategory,"");
                    //actionPublisher.PerformUtteranceFromLibrary(uttID, category, subcategory, tags, values);
                    Console.WriteLine(">>>>>>>>>>>>>>>>>> LATEST MEMORY EVENT UTTERANCE: " + category + ":" + subcategory);
                }

            }
        }

        public override void Cancel()
        {
            this.actionPublisher.CancelUtterance(uttID);
        }

        protected override void AttachPerceptionEvents()
        {
            perceptionClient.SpeakFinishedEvent += perceptionClient_SpeakFinishedEvent;
        }

        void perceptionClient_SpeakFinishedEvent(object sender, Thalamus.SpeechEventArgs e)
        {
            if (e.ID.Equals(uttID))
                this.RaiseFinishedEvent(_detector);
        }
        
        public override void Dispose()
        {
            perceptionClient.SpeakFinishedEvent -= perceptionClient_SpeakFinishedEvent;
        }

        public override string ToString()
        {
            return "Perform utterance for Memory Event";
        }
    }
}
