using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Scenario1SpeechManager
{
    public class SpeechDB
    {
        public List<SpeechAct> SpeechActs { get; set; }

        public SpeechDB()
        {
            SpeechActs = new List<SpeechAct>();
        }
        public static SpeechDB LoadSpeechDB(string filename)
        {
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                return ((SpeechDB)serializer.Deserialize(file, typeof(SpeechDB)));
            }
        }

        public static void SaveSpeechDB(string filename, SpeechDB speechFile)
        {
            using (StreamWriter file = File.CreateText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Formatting = Formatting.Indented;
                serializer.Serialize(file, speechFile);
            }
        }

        public static SpeechDB CreateExampleFile()
        {
            SpeechDB speechExample = new SpeechDB();
            for (int i = 0; i < 3; i++)
            {
                SpeechAct sa = new SpeechAct();
                sa.SpeechType = "SpeechType" + i;
                for(int j=0; j<3; j++) {
                    sa.Speech.Add("Speech " + j + " for " + sa.SpeechType);
                }
                for(int j=0; j<3; j++) {
                    SpeechTag st = new SpeechTag();
                    st.ParameterName = "SpeechTag" + j + sa.SpeechType;
                    st.Tag = "ST" + i.ToString() + j.ToString();
                    sa.Tags.Add(st);
                }
                speechExample.SpeechActs.Add(sa);
            }
            return speechExample;
        }
    }

    public class SpeechAct
    {
        public SpeechAct()
        {
            Tags = new List<SpeechTag>();
            Speech = new List<String>();
        }

        public String SpeechType { get; set; }
        public List<SpeechTag> Tags { get; set; }
        public List<String> Speech { get; set; }
    }

    public class SpeechTag
    {
        public String Tag { get; set; }
        public String ParameterName { get; set; }
    }
}
