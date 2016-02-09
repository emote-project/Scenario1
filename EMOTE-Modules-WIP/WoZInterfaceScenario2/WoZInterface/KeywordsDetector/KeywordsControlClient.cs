using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;

namespace WOZInterface.KeywordsDetector
{
    
    
    class KeywordsControlClient
    {
        KeywordsControl window;

        public KeywordsControlClient(KeywordsControl window)
        {
            this.window = window;
            WoZOptimusPrimeMegaClient.GetInstance().WordDetectedEvent += KeywordsControlClient_WordDetectedEvent;
        }

        void KeywordsControlClient_WordDetectedEvent(object sender, WoZOptimusPrimeMegaClient.WordDetectedEventArgs e)
        {
            window.KeywordDetected (e.Words[0]);
        }
    }
}