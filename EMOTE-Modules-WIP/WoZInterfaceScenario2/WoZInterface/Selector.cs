using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using System.Text.RegularExpressions;

using System.Windows.Controls;

namespace WOZInterface
{

    public enum UtteranceType : int
    {
        STRATEGIC = 0,
        FEEDBACK = 1
    };

    /* 
     * Simple container class for the currently selected utterance data
      */
    public class UtteranceSet
    {
        public string mText = "";
        public string mNonTaggedText = "";
        public int mStrategy;
        public int mSkill;
        public int mDynamic;
    }

    public class SkillSelection
    {
        public int mSkill = 1;
        public string mSkillText = "";
    }
   
    public class Selector
    {
        public XmlNodeList nodes;

        public String mStrategicUtterance = "";
        public String mFeedbackUtterance = "";

        public void ReadUtterances(string filename)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(filename);
            nodes = xmlDocument.SelectNodes("/data-set/utterance");
        }

        /*
         * Read in the utterances which are specific to the steps i.e. skills 8-21
         */


        private int GetTaskScriptOffset(int activeStepID)
        {
            int requestionIndex = 0;

            if (activeStepID < 16)
            {
                requestionIndex = activeStepID - 4;
            }
            else if (activeStepID < 21)
            {
                requestionIndex = activeStepID - 5;
            }
            else if (activeStepID < 27)
            {
                requestionIndex = activeStepID - 6;
            }

            return requestionIndex;
        }

        public void AddUtteranceItem(UtteranceSet ut, ListBox listBox, string item)
        {
            ListBoxItem listBoxItem = new ListBoxItem();
            listBoxItem.Content = ut.mText;// item;

            listBox.Items.Add(listBoxItem);
        }

        public void SetSelectedUtterance(int type, String utterance)
        {
            if (type == (int)UtteranceType.STRATEGIC)
                mStrategicUtterance = utterance;
            else if (type == (int)UtteranceType.FEEDBACK)
                mFeedbackUtterance = utterance;
        }

        public String GetSelectedUtterance(int type)
        {
            if (type == (int)UtteranceType.STRATEGIC)
                return mStrategicUtterance;
            else if (type == (int)UtteranceType.FEEDBACK)
                return mFeedbackUtterance;

            return "Wrong UtteranceType provided";
        }








    }
}
