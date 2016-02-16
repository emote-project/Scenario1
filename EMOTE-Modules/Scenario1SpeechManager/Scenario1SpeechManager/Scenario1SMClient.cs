using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Thalamus;

namespace Scenario1SpeechManager
{
    public class Scenario1SMClient : ThalamusClient
    {
        public SpeechManager SpeechManager;

        public Scenario1SMClient(string character = "")
            : base("Scenario1 Speech Manager", character)
        {
            SpeechManager = new SpeechManager();
        }
    }
}
