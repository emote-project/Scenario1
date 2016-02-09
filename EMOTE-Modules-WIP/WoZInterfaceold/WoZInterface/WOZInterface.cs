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

            public void PerformUtterance(string utterance)
            {
                publisher.PerformUtterance(utterance);
                //System.Media.SystemSounds.Beep.Play();
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
                throw new NotImplementedException();
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



        /*
         * Receive Step Attempts from the Map
         */

        void EmoteMapReadingEvents.ITaskEvents.stepAnswerAttempt(int stepId, Boolean correct, Boolean distanceCorrect, Boolean directionCorrect, Boolean symbolCorrect, double distanceSkillLevel, double directionSkillLevel, double symbolSkillLevel, Boolean toolCorrect, String actualTool, String actualSymbol, String actualDistance, String actualDirection)
        {

            form.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {

                UserResponse responseData = new UserResponse();

                responseData.mStepId = stepId;
                responseData.mCorrect = correct;
                responseData.mDistanceCorrect = distanceCorrect;
                responseData.mDirectionCorrect = directionCorrect;
                responseData.mSymbolCorrect = symbolCorrect;
                
                responseData.mDistanceSkillLevel = distanceSkillLevel;
                responseData.mDirectionSkillLevel = directionSkillLevel;
                responseData.mSymbolSkillLevel = symbolSkillLevel;

                responseData.mDistance = actualDistance;
                responseData.mDirection = actualDirection;
                responseData.mSymbol = actualSymbol;

                form.mWOZManager.mUtteranceControl.SetUserResponseData(responseData);
                   // mFeedbackSelector.SetUserResponseData(responseData);
                //form.mWOZManager.mCompetence.SetTaskGrade(responseData);

                form.mWOZManager.mLoggingManager.AddLogItem(form.mWOZManager.mTaskScriptManager.GetActiveStepID(), 0, 0, "", 1, 1, responseData.mDistanceSkillLevel, (int)responseData.mDirectionSkillLevel, (int)responseData.mSymbolSkillLevel, 1, Convert.ToInt16(correct), responseData.mDistance, responseData.mDirection, responseData.mSymbol);

            }));

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
    }
}
