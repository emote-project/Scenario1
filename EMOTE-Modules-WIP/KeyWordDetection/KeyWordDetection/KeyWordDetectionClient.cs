using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Thalamus;
namespace KeyWordDetection
{
    public interface IKeyWordDetectionEvents : Thalamus.IPerception
    {
        void KeyWordDetected(string keyWord);
    }


    internal interface IKeyWordDetectionPulisher : IKeyWordDetectionEvents
    { }

    public class KeyWordDetectionClient : ThalamusClient
    {

        IKeyWordDetectionPulisher KeyWordPublisher;
        SpeechRecognitionEngine sre;
        Grammar g;

        internal class KeyWordDetectionPublisher : IKeyWordDetectionPulisher, IThalamusPublisher
        {
            dynamic publisher;
            public KeyWordDetectionPublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }
        
            public void KeyWordDetected(string keyWord)
            {
 	            publisher.KeyWordDetected(keyWord);
            }
        }

        public KeyWordDetectionClient(string character) :
            base("KeyWordDetection", character) 
        {
            SetPublisher<IKeyWordDetectionPulisher>();
            KeyWordPublisher = new KeyWordDetectionPublisher(Publisher);

            Choices keywords = new Choices();
            keywords.Add(new string[] {"robot", "he", "stupid"});
            GrammarBuilder gb = new GrammarBuilder();
            gb.Append(keywords);
            g = new Grammar(gb);

            foreach (RecognizerInfo ri in SpeechRecognitionEngine.InstalledRecognizers())
            {
                Debug("Installed recognizer: " + ri.Culture.DisplayName);
            }
            sre = new SpeechRecognitionEngine();
            Debug("Speech recognizer: " + sre.RecognizerInfo.Description);
            
            sre.LoadGrammar(g);
            sre.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(sr_SpeechRecognized);
            sre.SetInputToDefaultAudioDevice();
            (new Thread(new ThreadStart(RecognitionThread))).Start();
        }

        private void RecognitionThread()
        {
            while (!Shutingdown)
            {
                RecognitionResult rr = sre.Recognize(new TimeSpan(0, 0, 3));
                if (rr == null) Debug("Silence...");
                else Debug(rr.Text);
            }
        }

        private void sr_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Debug("Keyword detected: " + e.Result.Text);
            KeyWordPublisher.KeyWordDetected(e.Result.Text);
        }
    }
}
