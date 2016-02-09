using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaseBasedController.Detection;
using CaseBasedController.Detection.Composition;
using CaseBasedController.Thalamus;

namespace CaseBasedController.Behavior.Enercities
{
    class PerformWrapupAndGoodbye : BaseBehavior
    {
        private string _firstUttId;
        private string _secondUttId;

        public override void Dispose()
        {
            perceptionClient.UtteranceFinishedEvent -= PerceptionClientOnUtteranceFinishedEvent;
        }

        public override void Execute(IFeatureDetector detector)
        {
            _firstUttId = DateTime.Now.Ticks + "wrapupUtt";
            actionPublisher.PerformUtteranceFromLibrary(_firstUttId, "wrapup", "0", new string[] { }, new string[] { });

        }

        public override void Cancel()
        {
        }

        protected override void AttachPerceptionEvents()
        {
            perceptionClient.UtteranceFinishedEvent+= PerceptionClientOnUtteranceFinishedEvent;
        }

        private void PerceptionClientOnUtteranceFinishedEvent(object sender, IFMLUtteranceEventArgs ifmlUtteranceEventArgs)
        {
            if (_firstUttId!=null && ifmlUtteranceEventArgs.Id.Equals(_firstUttId))
            {
                _secondUttId = DateTime.Now.Ticks + "goodbyeUtt";
                actionPublisher.PerformUtteranceFromLibrary(_secondUttId, "greeting", "goodbye", new string[] { }, new string[] { });
            }
            if (_secondUttId != null && ifmlUtteranceEventArgs.Id.Equals(_secondUttId))
            {
                RaiseFinishedEvent();
            }
        }
    }
}
