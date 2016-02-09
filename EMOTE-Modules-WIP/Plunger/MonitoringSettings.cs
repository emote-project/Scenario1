using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Plunger
{
    public class MonitoringSettings
    {
        public List<String> Messages { get; set; }
        private String Filename { get; set; }

        public MonitoringSettings()
        {
            Messages = new List<String>();
        }

        public MonitoringSettings(List<String> msgs)
        {
            Messages = msgs;
        }

        public static MonitoringSettings Load(string filename)
        {
            using (StreamReader file = File.OpenText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                MonitoringSettings ms = (MonitoringSettings)serializer.Deserialize(file, typeof(MonitoringSettings));
                ms.Filename = filename;
                return ms;
            }
        }

        public void Save()
        {
            if (Filename != "") Save(Filename, this);
        }
        public static void Save(string filename, MonitoringSettings ms)
        {
            using (StreamWriter file = File.CreateText(filename))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, ms);
            }
        }
    }
}
