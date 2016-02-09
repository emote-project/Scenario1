using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using Thalamus;

namespace SpeechSoundDataStructures
{
    
    public struct Speech
    {
        public string Id;
        public string[] Text;
        public string[] Bookmarks;
        public string FullText()
        {
            string str = "";
            foreach (string s in Text) str += s + " ";
            return str;
        }

        public Speech(string id, string[] text)
        {
            Id = id;
            Text = text;
            Bookmarks = new string[0];
        }
        public Speech(string[] text)
        {
            Id = "";
            Text = text;
            Bookmarks = new string[0];
        }
        public Speech(string id, string text)
        {
            Id = id;
            Text = new string[1] { text };
            Bookmarks = new string[0];
        }
        public Speech(string text)
        {
            Id = "";
            Text = new string[1] { text };
            Bookmarks = new string[0];
        }
        public Speech(string id, string[] text, string[] bookmarks)
        {
            Id = id;
            Text = text;
            Bookmarks = bookmarks;
        }
        public Speech(string[] text, string[] bookmarks)
        {
            Id = "";
            Text = text;
            Bookmarks = bookmarks;
        }
    }

    public class SpeechData
    {

        public abstract class SpeechCharacteristic
        {
            public enum Types { Bookmark, Phoneme, Viseme };

            public Types type;
            public double time { get; set; }

            public SpeechCharacteristic(Types type, double time)
            {
                this.type = type;
                this.time = time;
            }

            public override string ToString()
            {
                return type.ToString() + "| "  + "(" + time + ")";
            }
        }

        public class SpeechBookmark : SpeechCharacteristic
        {
            public string name { get; set; }

            public SpeechBookmark(string name, double time) : base (Types.Bookmark, time)
            {
                this.name = name;
            }

        }

        public class SpeechViseme : SpeechCharacteristic
        {
            public int value;
            public float percent;
            public int nextViseme;
            public float nextPercent;

            public SpeechViseme(int value, double time, float percent, int nextViseme, float nextPercent) : base(Types.Viseme, time)
            {
                this.percent = percent;
                this.nextViseme = nextViseme;
                this.nextPercent = nextPercent;
            }

        }
       

        public string id {get; set;}
        public string dataString { get; set; }
        public List<SpeechBookmark> bookmarks { get; set; }
        public List<SpeechViseme> visemes { get; set; }


        public SpeechData() 
        {
            this.visemes = new List<SpeechViseme>();
            this.bookmarks = new List<SpeechBookmark>();
        }
        public SpeechData(string id, string dataString, List<SpeechBookmark> bookmarks, List<SpeechViseme> visemes)
        {
            this.id = id;
            this.dataString = dataString;
            this.bookmarks = bookmarks;
            this.visemes = visemes;
            if (visemes == null) this.visemes = new List<SpeechViseme>();
            if (bookmarks == null) this.bookmarks = new List<SpeechBookmark>();
        }
         
        //TODO: Generalize this methods to a general serializable class
        public static SpeechData Deserialize(string serializedData)
        {
            TextReader reader = new StringReader(serializedData);
            JsonSerializer serializer = new JsonSerializer();
            return (SpeechData)serializer.Deserialize(reader, typeof(SpeechData));
        }

        public static string Serialize(SpeechData speechData)
        {
            TextWriter writer = new StringWriter();
            JsonSerializer serializer = new JsonSerializer();
            serializer.Serialize(writer, speechData);
            return writer.ToString();
        }

        public override string ToString()
        {
            return "Speech id: " + id + ", Bookmarks: " + bookmarks.Count;
        }
    }


}
