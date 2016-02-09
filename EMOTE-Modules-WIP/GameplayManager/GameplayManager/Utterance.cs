using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace GameplayManager
{
    /*
    public class QueuedUtterance
    {
        public string Utterance { private set; get; }
        public string Category { private set; get; }
        public double Duration { private set; get; }
        public QueuedUtterance(string utterance, string category, double duration = 4)
        {
            this.Category = category;
            this.Utterance = utterance;
            this.Duration = duration;
        }
    }

    public static class Utterance
    {

        static List<QueuedUtterance> utterancesQueue;
        static Thread pollingThread;

        static DateTime sayStartedTime;
        static bool speaking = false;

        /// <summary>
        /// Randomly selects one of the utterances in the list and performs it
        /// </summary>
        /// <param name="utterances">list of QueuedUtterance</param>
        static public void SayOneOf(List<QueuedUtterance> utterances)
        {
            int ran = (new Random()).Next(utterances.Count - 1);
            QueuedUtterance selected = utterances[ran];
            Perform(selected);
        }

        static public void Perform(QueuedUtterance utterance)
        {
            Perform(utterance.Utterance, utterance.Category, utterance.Duration);
        }

        static public void Perform(string utterance, string category, double duration = 4)
        {
            if (utterancesQueue == null) utterancesQueue = new List<QueuedUtterance>();
            if (pollingThread == null)
            {
                pollingThread = new Thread(new ThreadStart(Polling));
                pollingThread.Start();
            }
            utterancesQueue.Add(new QueuedUtterance(utterance, category, duration));
            Console.WriteLine("Adding utterance: " + utterance + " (" + duration + ")");
        }

        static private void Polling()
        {
            while (utterancesQueue != null && utterancesQueue.Count > 0)
            {
                //Console.WriteLine("Queue: "+utterancesQueue.Count+" - Speaking: "+speaking+" - Current Utt Duration: "+utterancesQueue[0].Duration);
                if (!speaking)
                {
                    Console.WriteLine("Sending utterance to perform: " + utterancesQueue[0].Utterance + " (" + utterancesQueue[0].Duration + ")");
                    GamePlayManager.GamePublisher.PerformUtterance(utterancesQueue[0].Utterance, utterancesQueue[0].Category);
                    sayStartedTime = DateTime.Now;
                    speaking = true;
                }
                else
                {
                    //Console.WriteLine(DateTime.Now.Millisecond + " - " + sayStartedTime + " > " + utterancesQueue[0].Duration * 1000);
                    if ((DateTime.Now - sayStartedTime).TotalMilliseconds > utterancesQueue[0].Duration * 1000)
                    {
                        Console.WriteLine("Utterance terminated: " + utterancesQueue[0].Utterance + " (" + utterancesQueue[0].Duration + ")");
                        speaking = false;
                        utterancesQueue.RemoveAt(0);
                    }
                    Thread.Sleep(100);
                }
            }
            pollingThread = null;
        }
    }*/
}
