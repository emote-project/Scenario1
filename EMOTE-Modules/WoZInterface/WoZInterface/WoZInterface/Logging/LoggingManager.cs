using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace WOZInterface
{

    public class LogItem
    {
        public DateTime mDateTime;
        public int mStepID = 0;
        public int mStart = 0;

        public int mUtteranceCall = 0;
        public string mUtterance = "";

        public int mGradingType = 0;
        public int mGradingCall = 0;
        public double mGradingDistance = 0;
        public double mGradingDirection = 0;
        public double mGradingSymbol = 0;

        public int mResponseCall = 0;
        public int mResponseCorrect = 0;
        public string mResponseDistance = "";
        public string mResponseDirection = "";
        public string mResponseSymbol = "";
    }

    public class LoggingManager
    {
        
        List<LogItem> mLogItems = new List<LogItem>();
        string mFolder = "";
        string mFilename = "";

        public LoggingManager()
        {
        }


        public void AddLogItem  (
                                int stepID, int start, int utteranceCall, string utterance, int gradingType, int gradingCall, double gradingDistance, double gradingDirection,
                                double gradingSymbol, int responseCall, int responseCorrect, string responseDistance, string responseDirection, string responseSymbol
                                )
        {
 
            LogItem logItem = new LogItem();
            logItem.mDateTime = DateTime.Now;
            logItem.mStepID = stepID;
            logItem.mStart = start;
            logItem.mUtteranceCall = utteranceCall;
            logItem.mUtterance = utterance;
            
            logItem.mGradingType = gradingType;
            logItem.mGradingCall = gradingCall;
            logItem.mGradingDistance = gradingDistance;
            logItem.mGradingDirection = gradingDirection; 
            logItem.mGradingSymbol = gradingSymbol;
            
            logItem.mResponseCall = responseCall;
            logItem.mResponseCorrect = responseCorrect;
            logItem.mResponseDistance = responseDistance;
            logItem.mResponseDirection = responseDirection;
            logItem.mResponseSymbol = responseSymbol;
            
            mLogItems.Add(logItem);
        }

        public void Init(int id, string name)
        {

            mFolder = "C:\\WOZInterface\\" + id + "\\Logs\\"; ///"C:\\WOZInterface\\Logs\\" + id + "\\";

            if (Directory.Exists(mFolder) == false)
                Directory.CreateDirectory(mFolder);
            
            mFilename = "log.txt";
        }

        
        public void WriteLogFile()
        {

            using (StreamWriter sw = new StreamWriter(mFolder + mFilename))
            {
                foreach (LogItem item in mLogItems)
                {

                    string line = item.mDateTime.ToString("hh:mm:ss:fff") + "," +
                    item.mStepID.ToString() + "," +
                    item.mStart.ToString() + "," +
                    item.mUtteranceCall.ToString() + "," +
                    item.mUtterance.ToString() + "," +
                    item.mGradingType.ToString() + "," +

                    item.mGradingCall.ToString() + "," +
                    item.mGradingDistance.ToString() + "," +
                    item.mGradingDirection.ToString() + "," +
                    item.mGradingSymbol.ToString() + "," +

                    item.mResponseCall.ToString() + "," +
                    item.mResponseCorrect.ToString() + "," +
                    item.mResponseDistance.ToString() + "," +
                    item.mResponseDirection.ToString() + "," +
                    item.mResponseSymbol.ToString();

                    sw.WriteLine(line);
                }
            }


         // fs.Close();
        }

        public void ClearLog()
        {
            mLogItems.Clear();
        }
    }
}
