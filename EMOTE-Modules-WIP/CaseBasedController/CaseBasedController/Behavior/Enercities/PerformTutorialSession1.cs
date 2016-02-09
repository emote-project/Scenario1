using System;
using CaseBasedController.Detection;
using System.Collections.Generic;
using CaseBasedController.Thalamus;
using EmoteEnercitiesMessages;

namespace CaseBasedController.Behavior.Enercities
{
    /// <summary>
    ///     Performs a specified utterance.
    /// </summary>
    public class PerformTutorialSession1 : BaseBehavior
    {
        string _greetingsUttId;
        string _tutorialConstructionUttId;
        string _confirmConstructionUttId;
        string _tutorialConstructionForOtherUttId;

        IFeatureDetector _detector;
        private bool _shouldComment;

        public override void Execute(IFeatureDetector detector)
        {
            _detector = detector;
            //just perform the utterance
            lock (this.locker)
            {
                System.Threading.Thread.Sleep(300);                                         // Waits to be sure that GameStatus is updated by all the coming events
                Console.WriteLine("Do Greetings");
                _greetingsUttId = PerformUtterance("greeting","welcome");
            }
        }

        public override void Dispose()
        {
            perceptionClient.UtteranceFinishedEvent -= perceptionClient_UtteranceFinishedEvent;
        }

        void perceptionClient_UtteranceFinishedEvent(object sender, IFMLUtteranceEventArgs e)
        {
            if (e.Id.Equals(_greetingsUttId))
            {
                Console.WriteLine("Do tutorial construction");
                _tutorialConstructionUttId = PerformUtterance("tutorial","OwnConstruction");
            }
            if (e.Id.Equals(_tutorialConstructionUttId))
            {
                _shouldComment = true;
                Console.WriteLine("Do confirm contruction");
                actionPublisher.ConfirmConstruction(StructureType.Suburban, 5, 2);
            }
            if (e.Id.Equals(_confirmConstructionUttId))
            {
                _tutorialConstructionForOtherUttId = PerformUtterance("tutorial", "OtherConstruction");
            }
            if (e.Id.Equals(_tutorialConstructionForOtherUttId))
            {
                RaiseFinishedEvent(_detector);
            }
        }

        protected override void AttachPerceptionEvents()
        {
            perceptionClient.UtteranceFinishedEvent += perceptionClient_UtteranceFinishedEvent;
            perceptionClient.ConfirmConstructionEvent += PerceptionClientOnConfirmConstructionEvent;
        }

        private void PerceptionClientOnConfirmConstructionEvent(object sender, GameActionEventArgs gameActionEventArgs)
        {
            if (_shouldComment)     // Avoiding commenting future actions
            {
                _shouldComment = false;
                Console.WriteLine("Do perform confirmconstruction");
                _confirmConstructionUttId = PerformUtterance("confirmconstruction", "self");
            }
        }

        private string PerformUtterance(string category, string subcategory)
        {
            var tagsAndValues = GameInfo.GameStatus.GetTagNamesAndValues();
            var uttId = "cbc" + new System.Random().Next();
            string[] tags = new List<string>(tagsAndValues.Keys).ToArray();
            string[] values = new List<string>(tagsAndValues.Values).ToArray();
            Console.WriteLine("GAME VALUES: "+values);
            this.actionPublisher.PerformUtteranceFromLibrary(uttId, category, subcategory, tags, values);
            return uttId;
        }
        
        public override string ToString()
        {
            return "Tutorial Session 1";
        }

        public override void Cancel()
        {
            this.actionPublisher.CancelUtterance(_greetingsUttId);
        }
    }
}