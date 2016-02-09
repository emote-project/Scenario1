 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thalamus;
using SpeechSoundDataStructures;
using Thalamus.BML;

namespace ExternalTTS
{
    public interface ITTSClient :
        Thalamus.BML.ISpeakActions,
        ISpeakActionsTemp
    { }

    public interface ITTSClientPublisher : IThalamusPublisher, ISpeechSoundActions { }

    public class ExternalTTS : ThalamusClient, ITTSClient
    {
        protected string voice = "DefaultVoice";

        internal class TTSClientPublisher : ITTSClientPublisher
        {
            dynamic publisher;

            public TTSClientPublisher(dynamic publisher)
            {
                this.publisher = publisher;
            }

            public void SpeakWaveFile(string id, string data)
            {
 	            publisher.SpeakWaveFile(id, data);
            }

            public void SpeakWaveFile(string speechData_serialization)
            {
                publisher.SpeakWaveFile(speechData_serialization);
            }
        }

        internal TTSClientPublisher ttsPublisher;

        private TTSEngine ttsEngine;

        public ExternalTTS() : this("ExternalTTS") { }
        public ExternalTTS(string name)
            : base(name)
        {
            SetPublisher<TTSClientPublisher>();
            ttsPublisher = new TTSClientPublisher(Publisher);
        }

        void TTSEngine_SpeechGenerationComplete(object sender, TTSEngine.SpeechGeneratedEventArgs e)
        {
            ttsPublisher.SpeakWaveFile(SpeechData.Serialize(e.data));
        }

        public void Speak(string id, string text)
        {
            //DoSpeak(id, new string[1] { text });
            ttsEngine = new TTSEngine();
            ttsEngine.SpeechGenerationComplete += TTSEngine_SpeechGenerationComplete;
            Console.WriteLine("Speaking '" + text + "'");
            ttsEngine.GenerateSpeech(new Speech(id, text));   
        }

        public void SpeakBookmarks(string id, string[] text, string[] bookmarks)
        {
            //DoSpeak(id, text, bookmarks);
            ttsEngine = new TTSEngine();
            ttsEngine.SpeechGenerationComplete += TTSEngine_SpeechGenerationComplete;
            Speech speech = new Speech(id, text, bookmarks);
            Console.WriteLine("Speaking '" + speech.FullText() + "'");
            ttsEngine.GenerateSpeech(speech);
        }
        
        public void Speak(string id, string text, string soundEffectName, double delay)
        {
            DoSpeak(id, new string[1] { text }, null, soundEffectName, delay);
        }

        public void SpeakBookmarks(string id, string[] text, string[] bookmarks, string soundEffectName, double delay)
        {
            DoSpeak(id, text, bookmarks, soundEffectName, delay);
        }

        private void DoSpeak(string id, string[] text, string[] bookmarks = null, string soundEffectName = null, double delay = 0)
        {
            ttsEngine = new TTSEngine();
            ttsEngine.SpeechGenerationComplete += TTSEngine_SpeechGenerationComplete;
            Speech speech;
            if (bookmarks!=null) {
                speech = new Speech(id, text, bookmarks);
            } else {
                speech = new Speech(id, text[0]);
            }
            Console.WriteLine("Speaking '" + speech.FullText() + "'");
            ttsEngine.GenerateSpeech(speech,soundEffectName,delay);
        }


        public void SpeakStop()
        {
            throw new NotImplementedException();
        }
    }
}
