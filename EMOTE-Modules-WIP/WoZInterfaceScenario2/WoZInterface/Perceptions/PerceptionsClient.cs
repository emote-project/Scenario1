using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;

namespace WOZInterface.Perceptions
{
 
    class PerceptionsClient 
    {
        PerceptionsControl window;

        public PerceptionsClient(PerceptionsControl window)
        {
            WoZOptimusPrimeMegaClient.GetInstance().GazeTrackingEvent += PerceptionsClient_GazeTrackingEvent;
            WoZOptimusPrimeMegaClient.GetInstance().GazeOKAOEvent += PerceptionsClient_GazeOKAOEvent;
            WoZOptimusPrimeMegaClient.GetInstance().ActiveSoundUserEvent += PerceptionsClient_ActiveSoundUserEvent;
            WoZOptimusPrimeMegaClient.GetInstance().EyebrowsAUEvent += PerceptionsClient_EyebrowsAUEvent;
            WoZOptimusPrimeMegaClient.GetInstance().UserMutualGazeEvent += PerceptionsClient_UserMutualGazeEvent;
            WoZOptimusPrimeMegaClient.GetInstance().UserMutualPointEvent += PerceptionsClient_UserMutualPointEvent;
            WoZOptimusPrimeMegaClient.GetInstance().UserTouchChinEvent += PerceptionsClient_UserTouchChinEvent;
            this.window = window;
        }

        void PerceptionsClient_UserTouchChinEvent(object sender, WoZOptimusPrimeMegaClient.PerceptionEventArgs e)
        {
            if (e.UserID == 1)
            {
                window.SetEnvTouchChin(e.Value);
            }
            else
            {
                window.SetEcoTouchChin(e.Value);
            }
        }

        void PerceptionsClient_UserMutualPointEvent(object sender, WoZOptimusPrimeMegaClient.PerceptionEventArgs e)
        {
            window.SetMutualPoint(e.Value);
        }

        void PerceptionsClient_UserMutualGazeEvent(object sender, WoZOptimusPrimeMegaClient.PerceptionEventArgs e)
        {
            window.SetMutualGaze(e.Value);
        }

        void PerceptionsClient_EyebrowsAUEvent(object sender, WoZOptimusPrimeMegaClient.EyebrowsAUEventArgs e)
        {
            window.SetEcoEyebrowsAUs(e.AU4_user1, e.AU4_user1, e.AU2_user1, e.AU2_user1);
            window.SetEnvEyebrowsAUs(e.AU4_user2, e.AU4_user2, e.AU2_user2, e.AU2_user2);
        }

        void PerceptionsClient_ActiveSoundUserEvent(object sender, WoZOptimusPrimeMegaClient.ActiveSoundUserEventArgs e)
        {
            switch (e.UserAct)
            {
                case EmoteCommonMessages.ActiveUser.Both:
                    window.SetEnvActiveSpk(true);
                    window.SetEcoActiveSpk(true);
                    break;
                case EmoteCommonMessages.ActiveUser.Right:
                    window.SetEnvActiveSpk(false);
                    window.SetEcoActiveSpk(true);
                    break;
                case EmoteCommonMessages.ActiveUser.Left:
                    window.SetEnvActiveSpk(true);
                    window.SetEcoActiveSpk(false);
                    break;
                case EmoteCommonMessages.ActiveUser.None:
                    window.SetEnvActiveSpk(false);
                    window.SetEcoActiveSpk(false);
                    break;
            }
            window.SetEnvActiveSpkVolume(Math.Round(e.RightValue, 2));
            window.SetEcoActiveSpkVolume(Math.Round(e.LeftValue, 2));
        }

        void PerceptionsClient_GazeOKAOEvent(object sender, WoZOptimusPrimeMegaClient.PerceptionEventArgs e)
        {
            if (e.UserID == 1)
            {
                window.SetEcoGazeToRobot(e.Value);
            }
            else
            {
                window.SetEnvGazeToRobot(e.Value);
            }
        }

        void PerceptionsClient_GazeTrackingEvent(object sender, WoZOptimusPrimeMegaClient.PerceptionEventArgs e)
        {
            //if (e.UserID == 1) window.SetEnvHeadDirection(e.Direction);
            //else window.SetEcoHeadDirection(e.Direction);
        }
    }
}
