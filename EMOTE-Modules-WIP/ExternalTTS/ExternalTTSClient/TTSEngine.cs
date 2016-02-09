using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using System.Speech.Synthesis.TtsEngine;
using SpeechSoundDataStructures;
using Thalamus.BML;
using System.Runtime.InteropServices;

namespace ExternalTTS
{
    public class TTSEngine
    {
        public event EventHandler<SpeechGeneratedEventArgs> SpeechGenerationComplete;
        public class SpeechGeneratedEventArgs : EventArgs
        {
            public SpeechData data { get; set; }

            public SpeechGeneratedEventArgs(SpeechData data)
            {
                this.data = data;
            }
        }

        private SpeechData data = new SpeechData();
        private Dictionary<int, float> visemes;

        public TTSEngine()
        {
            SetupVisemes();
        }

        private void SetupVisemes()
        {
            visemes = new Dictionary<int, float>();

            // closed
            visemes.Add(0, 0); //silence
            visemes.Add(21, 0); //p, b, m

            // almost closed
            visemes.Add(18, 0.1f); //f, v
            visemes.Add(7, 0.1f); //w, uw

            // slight open
            visemes.Add(14, 0.3f);  //l
            visemes.Add(15, 0.3f);  //s, z
            visemes.Add(16, 0.3f);  //sh, ch, jh, zh
            visemes.Add(17, 0.3f);  //th, dh
            visemes.Add(19, 0.3f);  //d, t, n

            // mid open
            visemes.Add(8, 0.5f); //ow
            visemes.Add(13, 0.5f); //r
            visemes.Add(20, 0.5f); //k, g, ng

            // almost wide open
            visemes.Add(1, 0.7f); //ae, ax, ah
            visemes.Add(2, 0.7f); //aa
            visemes.Add(3, 0.7f); //ao
            visemes.Add(9, 0.7f); //aw
            visemes.Add(12, 0.7f); //h
            visemes.Add(10, 0.7f); //oy
            visemes.Add(11, 0.7f); //ay

            // wide open
            visemes.Add(4, 1.0f); //ey, eh, uh
            visemes.Add(5, 1.0f); //er
            visemes.Add(6, 1.0f); //y, iy, ih, ix
        }

        private float VisemeToPercent(int viseme)
        {
            if (visemes.ContainsKey(viseme)) return visemes[viseme];
            else return 0;
        }


        public void GenerateSpeech(Speech speech, string soundEffect = null, double delay = 0)
        {
            try
            {
                PromptBuilder p = new PromptBuilder();
                p.StartStyle(new PromptStyle(PromptEmphasis.None));
                for (int i = 0; i < speech.Text.Length; i++)
                {
                    if (speech.Bookmarks == null || speech.Bookmarks.Length < i + 1)
                    {
                        string s = "";
                        for (; i < speech.Text.Length; i++) s += speech.Text[i] + " ";
                        p.AppendText(s);
                        break;
                    }
                    else
                    {
                        p.AppendText(speech.Text[i]);
                        p.AppendBookmark(speech.Bookmarks[i]);
                    }
                }
                //p.Culture = tts.Voice.Culture;
                p.EndStyle();
                
                string ttsTempFileFolderPath = @".\";
                string filename = ttsTempFileFolderPath + "temp_id_" + speech.Id + ".wav";
                using (SpeechSynthesizer synth = new SpeechSynthesizer())
                {
                    bool voiceExists = false;
                    foreach (InstalledVoice v in synth.GetInstalledVoices())
                    {
                        if (v.VoiceInfo.Name == Properties.Settings.Default.Voice) voiceExists = true;
                    }
                    if (voiceExists) synth.SelectVoice(Properties.Settings.Default.Voice);

                    synth.SetOutputToWaveFile(filename,
                       new System.Speech.AudioFormat.SpeechAudioFormatInfo(11025, System.Speech.AudioFormat.AudioBitsPerSample.Sixteen, System.Speech.AudioFormat.AudioChannel.Mono)
                    );

                    synth.VisemeReached += synth_VisemeReached;
                    synth.PhonemeReached += synth_PhonemeReached;
                    synth.SpeakCompleted += synth_SpeakCompleted;
                    synth.SpeakStarted += synth_SpeakStarted;
                    synth.BookmarkReached += synth_BookmarkReached;

                    synth.Speak(p);
                }
                // Adding sound effect
                if (soundEffect != null)
                {
                    AudioUtilities.Mixer.ConcatenateWave(filename, soundEffect, (float)delay, filename);
                }

                string datastring = AudioUtilities.Serializer.FromFile(filename);
                data.id = speech.Id;
                data.dataString = datastring;
                if (SpeechGenerationComplete != null) SpeechGenerationComplete(null, new SpeechGeneratedEventArgs(data));
                System.IO.File.Delete(filename);
            }
            catch (Exception e)
            {
                Console.WriteLine("WindowsTTS Failed: " + e.Message);
            }
        }

        void synth_SpeakStarted(object sender, SpeakStartedEventArgs e)
        {
            Console.WriteLine("SpeakStarted");
        }

        void synth_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            Console.WriteLine("SpeakCompleted");
        }

        void synth_BookmarkReached(object sender, BookmarkReachedEventArgs e)
        {
            Console.WriteLine("BookmarkReached {0} at {1}", e.Bookmark, e.AudioPosition);
            data.bookmarks.Add(new SpeechData.SpeechBookmark(e.Bookmark,e.AudioPosition.TotalMilliseconds));
        }

        void synth_VisemeReached(object sender, VisemeReachedEventArgs e)
        {
            Console.WriteLine("VisemeReached {0} at {1}", e.Viseme, e.AudioPosition);
            data.visemes.Add(new SpeechData.SpeechViseme(e.Viseme, e.AudioPosition.TotalMilliseconds, VisemeToPercent(e.Viseme), e.NextViseme, VisemeToPercent(e.NextViseme)));
        }

        void synth_PhonemeReached(object sender, PhonemeReachedEventArgs e)
        {
            //Console.WriteLine("PhonemeReached {0} at {1}", e.Phoneme, e.AudioPosition);
        }

       
        
    }
}
