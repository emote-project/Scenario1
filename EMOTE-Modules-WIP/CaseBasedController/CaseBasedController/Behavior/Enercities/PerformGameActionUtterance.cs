using System;
using System.Collections.Generic;
using System.Linq;
using CaseBasedController.Detection;
using CaseBasedController.GameInfo;
using CaseBasedController.Thalamus;
using EmoteEnercitiesMessages;
using EmoteEvents;
using Newtonsoft.Json;

namespace CaseBasedController.Behavior.Enercities
{
    /// <summary>
    ///     Performs an utterance related with the execution of some action.
    /// </summary>
    public class PerformGameActionUtterance : BaseBehavior
    {
        private const string SELF_SUB_CAT_STR = "self";
        private string _uttID;
        private IFeatureDetector _detector;

        private static readonly Dictionary<ActionType, string> Categories =
            new Dictionary<ActionType, string>
            {
                {ActionType.BuildStructure, "ConfirmConstruction"},
                {ActionType.UpgradeStructure, "PerformUpgrade"},
                {ActionType.ImplementPolicy, "ImplementPolicy"}
            };

        public override void Execute(IFeatureDetector detector)
        {
            Console.WriteLine("Execute PerformGameActionUtterance: " + detector);
            _detector = detector;
            ActionType actionType = ActionType.SkipTurn;
            if (GameStatus.CurrentState.PlayedStructure != null) actionType = ActionType.BuildStructure;
            if (GameStatus.CurrentState.PlayedPolicy != null) actionType = ActionType.ImplementPolicy;
            if (GameStatus.CurrentState.PlayedUpgrade != null) actionType = ActionType.UpgradeStructure;
                
            //just perform the utterance
            lock (this.locker)
            {
                if (Categories.ContainsKey(actionType))
                {
                    var tagsAndValues = GameInfo.GameStatus.GetTagNamesAndValues();
                    _uttID = PerformUtterance(Categories[actionType], SELF_SUB_CAT_STR);


                    //string mainStrategy = GameStatus.GetMainStrategy();

                    //switch (actionType)
                    //{
                    //    case ActionType.BuildStructure:
                    //        this.actionPublisher.PerformUtterance(_uttID,
                    //            "I'm building " + GameInfo.GameStatus.CurrentState.PlayedStructure.Translation +
                    //            (mainStrategy!=null?" because I think this will have a good inpact on our " + mainStrategy:""), "");
                    //        break;
                    //    case ActionType.ImplementPolicy:
                    //        this.actionPublisher.PerformUtterance(_uttID,
                    //            "I'm implementing the policy " + GameInfo.GameStatus.CurrentState.PlayedPolicy.Translation +
                    //            (mainStrategy!=null?" because I think this will have a good inpact on our " + mainStrategy:""), "");
                    //        break;
                    //    case ActionType.UpgradeStructure:
                    //        this.actionPublisher.PerformUtterance(_uttID,
                    //            "I'm upgrading " + GameInfo.GameStatus.CurrentState.PlayedUpgrade.Translation +
                    //            (mainStrategy != null ? " because I think this will have a good inpact on our " + mainStrategy : ""), "");
                    //        break;
                    //    default:
                    //        this.RaiseFinishedEvent(_detector);
                    //        break;
                    //}
                    
                }
                else
                {
                    this.RaiseFinishedEvent(_detector);
                }
            }
        }

        public override void Cancel()
        {
        }

        protected override void AttachPerceptionEvents()
        {
            perceptionClient.SpeakFinishedEvent += perceptionClient_SpeakFinishedEvent;
        }

        void perceptionClient_SpeakFinishedEvent(object sender, Thalamus.SpeechEventArgs e)
        {
            Console.WriteLine("SpeakFinishedEvent: "+e.Text);
            if (e.ID.Equals(_uttID))
                this.RaiseFinishedEvent(_detector);
        }

        public override void Dispose()
        {
            perceptionClient.SpeakFinishedEvent -= perceptionClient_SpeakFinishedEvent;
        }

        private string PerformUtterance(string category, string subcategory)
        {
            var tagsAndValues = GameInfo.GameStatus.GetTagNamesAndValues();
            var uttId = "cbc" + new System.Random().Next();
            string[] tags = new List<string>(tagsAndValues.Keys).ToArray();
            string[] values = new List<string>(tagsAndValues.Values).ToArray();
            this.actionPublisher.PerformUtteranceFromLibrary(uttId, category, subcategory, tags, values);
            return uttId;
        }
    }
}