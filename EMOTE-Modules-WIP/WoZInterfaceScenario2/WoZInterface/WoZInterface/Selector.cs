using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using System;
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

        public void SetSkillUtterances(participantDetails participant, int[] total, string[,] ssut, int activeStepID)
        {
            int stepCount = 0;
            int savedStepCount = 0;
            int utCount = 0;

            foreach (XmlNode node in nodes)
            {

                if (node.SelectSingleNode("skill") != null && int.Parse(node.SelectSingleNode("skill").InnerText) >= 8 && int.Parse(node.SelectSingleNode("skill").InnerText) <= 21)
                {
                    stepCount = int.Parse(node.SelectSingleNode("skill").InnerText) - 8;// GetTaskScriptOffset(activeStepID);

                    if (savedStepCount !=  stepCount)
                    {
                        utCount = 0;
                        savedStepCount = stepCount;
                    }

                    string utteranceString = node.SelectSingleNode("text").InnerText.Replace("/student/", participant.mName);

                    ssut[stepCount, utCount] = utteranceString;
                    utCount++;
                    total[stepCount]++;
                    
                }
            }
            
        }
        
        /*
        * Use the Skill and Strategy Selection to set the Utterances
        */
        public int SetUtterancesList(participantDetails participant, TaskStep taskstepdata, UtteranceType type, int strategy, int activeStepID, int firstActivityStep, SkillSelection skillSelection, List<UtteranceSet> ut)
        {

                      
           // listBox.Items.Clear();

            int counter = 0;

            int problemCounter = 0;

            foreach (XmlNode node in nodes)
            {

                if (node.SelectSingleNode("strategy") != null && node.SelectSingleNode("skill") != null && node.SelectSingleNode("dynamic") != null)
                {

                    if (type == UtteranceType.FEEDBACK)
                    {

                        if (int.Parse(node.SelectSingleNode("strategy").InnerText) == strategy)
                        {
                            
                            if (int.Parse(node.SelectSingleNode("strategy").InnerText) == 4) // Requestion
                            {
                                if (int.Parse(node.SelectSingleNode("skill").InnerText) == GetTaskScriptOffset(activeStepID))
                                {
                                    UtteranceSet utterance = new UtteranceSet();
                                    utterance = ProcessUtterance(participant, taskstepdata, node, skillSelection);

                                    ut.Add(utterance);

                                    //AddUtteranceItem(utterance, listBox, utterance.mText);
                                    counter++;

                                   

                                    

                                }

                            } 
                            
                            else if (int.Parse(node.SelectSingleNode("skill").InnerText) == skillSelection.mSkill || skillSelection.mSkill == 0 || strategy > 9) //Allow Generic Behaviours
                            {

                                UtteranceSet utterance = new UtteranceSet();
                                utterance = ProcessUtterance(participant, taskstepdata, node, skillSelection);

                                ut.Add(utterance);

                                //AddUtteranceItem(utterance, listBox, utterance.mText);
                                counter++;

                                
                            }
                        }

                    }
                }
                else
                {
                    string[] info = new string[3];
                    info[0] = "O.K";
                    info[1] = "O.K";
                    info[2] = "O.K";

                    if (node.SelectSingleNode("strategy") == null)
                        info[0] = "NULL";

                    if (node.SelectSingleNode("skill") == null)
                        info[1] = "NULL";
 
                    if (node.SelectSingleNode("dynamic") == null)
                        info[2] = "NULL";

                    Console.WriteLine("Something Missing == ID = " + problemCounter.ToString() + " TYPE = " + type + info[0] + "," + info[1] + "," + info[2]);
                }


                problemCounter++;

            }

            if (counter == 0)
            {
                UtteranceSet utterance = new UtteranceSet();
                utterance.mText = "There is nothing to choose from here?";
                utterance.mNonTaggedText = "There is nothing to choose from here?";

                ut.Add(utterance);
                //AddUtteranceItem(utterance, listBox, utterance.mText);
                counter++;
            }

            return counter;
        }

        ///*
        //* Use the Skill and Strategy Selection to set the Utterances
        //*/
        //public int SetUtterancesList(participantDetails participant, TaskStep taskstepdata, ListBox listBox, UtteranceType type, int strategy, int activeStepID, int firstActivityStep, SkillSelection skillSelection)
        //{
        //    listBox.Items.Clear();

        //    int counter = 0;

        //    int problemCounter = 0;

        //    foreach (XmlNode node in nodes)
        //    {

        //        if (node.SelectSingleNode("strategy") != null && node.SelectSingleNode("skill") != null && node.SelectSingleNode("dynamic") != null)
        //        {

        //            if (type == UtteranceType.FEEDBACK)
        //            {

        //                if (int.Parse(node.SelectSingleNode("strategy").InnerText) == strategy)
        //                {

        //                    if (int.Parse(node.SelectSingleNode("strategy").InnerText) == 4) // Requestion
        //                    {
        //                        if (int.Parse(node.SelectSingleNode("skill").InnerText) == GetTaskScriptOffset(activeStepID))
        //                        {
        //                            UtteranceSet utterance = new UtteranceSet();
        //                            utterance = ProcessUtterance(participant, taskstepdata, node, skillSelection);

        //                            AddUtteranceItem(listBox, utterance.mText);
        //                            counter++;
        //                        }

        //                    }

        //                    else if (int.Parse(node.SelectSingleNode("skill").InnerText) == skillSelection.mSkill || skillSelection.mSkill == 0 || strategy > 9) //Allow Generic Behaviours
        //                    {

        //                        UtteranceSet utterance = new UtteranceSet();
        //                        utterance = ProcessUtterance(participant, taskstepdata, node, skillSelection);

        //                        AddUtteranceItem(listBox, utterance.mText);
        //                        counter++;
        //                    }
        //                }

        //            }
        //        }
        //        else
        //        {
        //            string[] info = new string[3];
        //            info[0] = "O.K";
        //            info[1] = "O.K";
        //            info[2] = "O.K";

        //            if (node.SelectSingleNode("strategy") == null)
        //                info[0] = "NULL";

        //            if (node.SelectSingleNode("skill") == null)
        //                info[1] = "NULL";

        //            if (node.SelectSingleNode("dynamic") == null)
        //                info[2] = "NULL";

        //            Console.WriteLine("Something Missing == ID = " + problemCounter.ToString() + " TYPE = " + type + info[0] + "," + info[1] + "," + info[2]);
        //        }


        //        problemCounter++;

        //    }

        //    if (counter == 0)
        //    {
        //        UtteranceSet utterance = new UtteranceSet();
        //        utterance.mText = "There is nothing to choose from here?";
        //        AddUtteranceItem(listBox, utterance.mText);
        //    }

        //    return counter;
        //}

        private UtteranceSet ProcessUtterance(participantDetails participant, TaskStep taskstepdata, XmlNode node, SkillSelection skillSelection)
        {
            UtteranceSet tempUtteranceSet = new UtteranceSet();

            tempUtteranceSet.mText = node.SelectSingleNode("text").InnerText;
            tempUtteranceSet.mStrategy = int.Parse(node.SelectSingleNode("strategy").InnerText);
            tempUtteranceSet.mSkill = int.Parse(node.SelectSingleNode("skill").InnerText);
            tempUtteranceSet.mDynamic = int.Parse(node.SelectSingleNode("dynamic").InnerText);

            tempUtteranceSet.mText = tempUtteranceSet.mText.Replace("/distance/", taskstepdata.mDistanceValue.ToString() + " " + taskstepdata.mDistanceUnit);
            tempUtteranceSet.mText = tempUtteranceSet.mText.Replace("/interDirection/", taskstepdata.mDirectionValue);
            tempUtteranceSet.mText = tempUtteranceSet.mText.Replace("/interSecDirection/", taskstepdata.mDirectionValue);
            tempUtteranceSet.mText = tempUtteranceSet.mText.Replace("/direction/", taskstepdata.mDirectionValue);
            tempUtteranceSet.mText = tempUtteranceSet.mText.Replace("/symbol/", taskstepdata.mSymbolValue + " symbol");
            tempUtteranceSet.mText = tempUtteranceSet.mText.Replace("/student/", participant.mName);
            tempUtteranceSet.mText = tempUtteranceSet.mText.Replace("/skill/", skillSelection.mSkillText);


            tempUtteranceSet.mNonTaggedText = Regex.Replace(tempUtteranceSet.mText, @"\s*?(?:\(.*?\)|\<.*?\>|\{.*?\})", String.Empty);
            tempUtteranceSet.mNonTaggedText = Regex.Replace(tempUtteranceSet.mNonTaggedText, @"\s*?(?:\(.*?\)|\\.*?\\|\{.*?\})", String.Empty);

           

            return tempUtteranceSet;
        }



        /*
         * Add a Single Utterance item to the ListBox 
         */
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
