using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Thalamus;

namespace WOZInterface
{

    public interface IWOZInterface : EmoteCommonMessages.IEmoteActions, EmoteMapReadingEvents.IGameStateEvents, EmoteMapReadingEvents.ITaskEvents
    { }

    public interface IWOZInterfacePublisher : EmoteCommonMessages.IEmoteActions, EmoteMapReadingEvents.IMapActions , EmoteCommonMessages.IFMLSpeech
    { }


    public class WOZInterfaceClient : ThalamusClient, IWOZInterface
    {
        internal class WOZInterfacePublisher : IWOZInterfacePublisher
        {
            dynamic publisher;
            public WOZInterfacePublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }

            /*
             * Control Panel Actions
             */


            public void Reset()
            {
                publisher.Reset();
            }

            public void Start(int participantId, string participantName)
            {
                publisher.Start(participantId, participantName);
            }

            public void Stop()
            {
                publisher.Stop();
            }


            /*
             * MapInterface Actions
             */


            public void BlockUI()
            {
                publisher.BlockUI();
            }

            public void CompassHide()
            {
                publisher.CompassHide();
            }

            public void CompassHighlight()
            {
                publisher.CompassHighlight();
            }

            public void CompassShow()
            {
                publisher.CompassShow();
            }

            public void DistanceHide()
            {
                publisher.DistanceHide();
            }

            public void DistanceHighlight()
            {
                publisher.DistanceHighlight();
            }

            public void DistanceShow()
            {
                publisher.DistanceShow();
            }

            public void DistanceToolEnd()
            {
                publisher.DistanceToolEnd();
            }

            public void DistanceToolReset()
            {
                publisher.DistanceToolEnd();
            }

            public void DistanceToolStart()
            {
                publisher.DistanceToolEnd();
            }

            public void MapKeyHide()
            {
                publisher.MapKeyHide();
            }

            public void MapKeyHighlight()
            {
               publisher.MapKeyHighlight();
            }

            public void MapKeyShow()
            {
                publisher.MapKeyShow();
            }

            public void StartNextStep()
            {
                publisher.StartNextStep();
            }

            public void StartStep(int stepid)
            {
                publisher.StartStep(stepid);
            }

            public void UnBlockUI()
            {
                publisher.UnBlockUI();
            }

            public void CompassHighlight(string direction)
            {
                publisher.CompassHighlight();
            }

            public void ToolAction(string toolName, string toolAction)
            {
                publisher.ToolAction(toolName, toolAction);
            }

            /*
             * IMPLEMENTED CHANGES 
             */
            public void GiveQuestionnaire(string name)
            {
                publisher.GiveQuestionnaire(name);
            }


            public void CompassHighlightDirection(string direction)
            {
                publisher.CompassHighlightDirection(direction);
            }


            public void CancelUtterance(string id)
            {
                publisher.CancelUtterance(id);
            }

            public void PerformUtterance(string id, string utterance, string category)
            {
                publisher.PerformUtterance(id, utterance, category);
            }

            public void PerformUtteranceFromLibrary(string id, string category, string subcategory, string[] tagNames, string[] tagValues)
            {
                publisher.PerformUtteranceFromLibrary(id, category, subcategory, tagNames, tagValues);
            }

            public void ReplaceTagsAndPerform(string utterance, string category)
            {
                publisher.ReplaceTagsAndPerform(utterance, category);
            }

            public void HighlightRightAnswer()
            {
                publisher.HighlightRightAnswer();
            }
        }

        MainWindow form;
        internal WOZInterfacePublisher WOZPublisher;
        public WOZInterfaceClient(MainWindow form): base("EMOTEWOZInterface")
        {
            this.form = form;
            SetPublisher<IWOZInterfacePublisher>();
            WOZPublisher = new WOZInterfacePublisher(Publisher);
        }


        public void GameState(bool running)
        {
            Console.WriteLine("Started");
        }

        public void Reset()
        {
            Console.WriteLine("Reset");
        }

        public void Start(int participantId, string participantName)
        {
            Console.WriteLine("Start");
        }

        public void Stop()
        {
            Console.WriteLine("Stop");
        }

        public void PerformUtterance(string utterance)
        {
            Console.WriteLine("PerformUtterance");
        }

        public void stepAnswerAttempt(int learnerId, int stepId, int activityId, int scenarioId, int sessionId, bool correct,
            string[] competencyName, bool[] competencyCorrect, string[] competencyActual, string[] competencyExpected,
            string mapEventId)
        {
            
        }

        public void interactionEvaluation(int learnerId, int stepId, int activityId, int scenarioId, int sessionId, string action,
            string strategy, string evaluation)
        {
           
        }
    }
}
