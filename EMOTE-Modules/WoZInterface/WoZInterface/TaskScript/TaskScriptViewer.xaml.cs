﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace WOZInterface
{

    /// <summary>
    /// Interaction logic for ScriptViewer.xaml
    /// </summary>
    public partial class TaskScriptViewer : UserControl
    {
        public TaskScriptManager mTaskScriptManager = null; 
        int rowCount = 0;

        String mSavedTrailID = "z";
        int mRowOffSet = 0;
        
        
        
        public TaskScriptViewer(TaskScriptManager taskScript)
        {
            mTaskScriptManager = taskScript;

            try
            {
                InitializeComponent();
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }

        }

        public void IncrementRowCount()
        {
            this.rowCount++;
        }

        public int GetRowCount()
        {
            return this.rowCount;
        }

        
        public void SetTaskSteps()
        {
            this.ScriptGrid.Children.Clear(); 
            mRowOffSet = -1;

            foreach (TaskStep step in mTaskScriptManager.mTaskSteps)
            {
                CreateLabelDynamically(step);
            }
        }
        
        public void CreateLabelDynamically(TaskStep step)// String content, int rowId, int colId, bool active, bool heading = false)
        {

            bool printStepLabel = true;
            /* Check if this is the start of a new trail */
            if (!String.Equals(mSavedTrailID, step.mTrailId))
            {
                mRowOffSet++;
                
                Label trailLabel = new Label();
                trailLabel.Name = "McLabel";
                trailLabel.Width = 125;
                trailLabel.Height = 25;
                trailLabel.FontSize = 11;

                printStepLabel = true;


                if (step.mId == mTaskScriptManager.GetActiveStepID())
                {
                    trailLabel.Foreground = new SolidColorBrush(Colors.Green);
                }
                else
                    trailLabel.Foreground = new SolidColorBrush(Colors.White);


                
                if (step.mTrailId.Equals("X"))
                {
                    trailLabel.Content = "Introduction";
                    printStepLabel = false;
                }
                else if (step.mTrailId.Equals("Y"))
                {
                    trailLabel.Content = "Tool Introduction";
                }
                else if (step.mTrailId.Equals("Z"))
                {
                    trailLabel.Content = "Activity Debrief";
                    printStepLabel = false;
                }
                else if (step.mTrailId.Equals("D"))
                {
                    trailLabel.Content = "Object Placement";
                    printStepLabel = false;
                }
                else
                    trailLabel.Content = "Map Trail " + step.mTrailId;

                Grid.SetRow(trailLabel, step.mRowID + mRowOffSet);
                Grid.SetColumn(trailLabel, 2);

                this.ScriptGrid.Children.Add(trailLabel);

                mSavedTrailID = step.mTrailId;
            }

            if (printStepLabel == true)
            {
                Label stepLabel = new Label();
                stepLabel.Name = "McLabel";
                stepLabel.Width = 125;
                stepLabel.Height = 25;
                stepLabel.FontSize = 10;

                if (step.mId == mTaskScriptManager.GetActiveStepID())
                {
                    stepLabel.Foreground = new SolidColorBrush(Colors.Green);
                }
                else
                    stepLabel.Foreground = new SolidColorBrush(Colors.White);

                stepLabel.HorizontalAlignment = HorizontalAlignment.Left;
                stepLabel.Content = step.mLabel;

                Grid.SetRow(stepLabel, step.mRowID + mRowOffSet);
                Grid.SetColumn(stepLabel, 4);

                this.ScriptGrid.Children.Add(stepLabel);
            }

            if (step.mId == mTaskScriptManager.GetActiveStepID())
            {
                // Restart Button
                Button restartButton = new Button();

                Grid.SetRow(restartButton, step.mRowID + mRowOffSet);
                Grid.SetColumn(restartButton, 5);

                var resource = FindResource("SmallResetButton") as ControlTemplate;

                restartButton.Template = resource;
                restartButton.Click += new RoutedEventHandler(ResetStep);
                this.ScriptGrid.Children.Add(restartButton);

                // Arrow Button
                Image arrowImage = new Image();

                var uri = new Uri("pack://application:,,,/Images/Generic/ArrowGreen.png");
                var bitmap = new BitmapImage(uri);
                arrowImage.Source = bitmap;

                Grid.SetRow(arrowImage, step.mRowID + mRowOffSet);
                Grid.SetColumn(arrowImage, 0);

                this.ScriptGrid.Children.Add(arrowImage);

            }

        }


        public void ResetStep(object sender, RoutedEventArgs e)
        {
            mTaskScriptManager.mWOZManager.RestartTaskStep();
        }

        /* 
         * This is called when the task script -- start button ->> which changes to the next step button is pressed
         */
        public void NextTaskStep(object sender, RoutedEventArgs e)
        {
            e.Handled = true; 
            
            if (mTaskScriptManager.mTaskStarted == false)
            {
                NextTaskStepButton.Content = "Next Step";
                mTaskScriptManager.mTaskStarted = true;
                mTaskScriptManager.mWOZManager.StartTask();
            }
            else
            {
                mTaskScriptManager.AdvanceTaskStep();
                SetTaskSteps();
            }
            
            bool test = e.Handled;
        }

        private void AutoRequestion(object sender, RoutedEventArgs e)
        {
            mTaskScriptManager.mWOZManager.ReQuestion();
        }

        private void AutoRepeatQuestion(object sender, RoutedEventArgs e)
        {
            mTaskScriptManager.mWOZManager.RepeatQuestion();
        }

        private void SendProbeEvent(object sender, RoutedEventArgs e)
        {
            mTaskScriptManager.mWOZManager.RequestProbeEvent();
        }

        
    }
}
