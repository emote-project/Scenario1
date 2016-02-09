using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Scenario1SpeechManager
{
    public class SpeechManager
    {
        public Dictionary<string, SpeechAct> SpeechDB { get; private set; }
        public SpeechManager()
        {
            SpeechDB = new Dictionary<string, SpeechAct>();
        }

        public string BuildText(string textTemplate, SpeechAct speechAct, Dictionary<string, string> parameters) {
            string text = textTemplate;
            foreach(SpeechTag tag in speechAct.Tags) {
                if (parameters.ContainsKey(tag.ParameterName)) {
                    text = text.Replace(tag.Tag, parameters[tag.ParameterName]);
                }
            }
            return text;
        }

        public string LoadDB(string filename)
        {
            if (!File.Exists(filename))
            {
                return "The specified file '" + filename + "' does not exist!";
            }
            else
            {
                try
                {
                    SpeechDB speechDB = Scenario1SpeechManager.SpeechDB.LoadSpeechDB(filename);
                    SpeechDB.Clear();
                    foreach (SpeechAct sa in speechDB.SpeechActs) SpeechDB[sa.SpeechType] = sa;
                    return "";
                }
                catch (Exception e)
                {
                    return e.Message;
                }
            }
        }
    }
}
