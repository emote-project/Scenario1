using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thalamus;

namespace ExternalTTS
{
    public interface ISpeechSoundActions : IAction
    {
        void SpeakWaveFile(string speechData_serialized);
    }

    //TODO: these following messages should be put together with ISpeakActions in the Thalamus messages definition
    public interface ISpeakActionsTemp : IAction
    {
        void Speak(string id, string text, string soundEffectName, double delay);
        void SpeakBookmarks(string id, string[] text, string[] bookmarks, string soundEffectName, double delay);
    }

    public interface ISpeechSoundEvents : IPerception
    {
        void SpeakWaveFileStarted(string id);
        void SpeakWaveFileFinished(string id);
    }

    
}
