using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.FaceTracking;
using System.IO;
using System.Linq;
using System.Windows;
using System.Timers;
using System.Reflection;
using System.Threading;

namespace KinectModule
{


    public partial class Form1 :  Form

    {
        KinectSensor kinectSensor;
        FaceTracker faceTracker;
        private byte[] colorPixelData;
        private short[] depthPixelData;
        private DepthImagePixel[] depthPixels;
        private Skeleton[] skeletonData;
        public string directionfotis;
        public bool tracedF;
        public bool timenabled;
        public bool timenabled2;
        public Form1()
        {
            InitializeComponent();
            directionfotis = "screen";
            KinectSensor.KinectSensors.StatusChanged += KinectSensors_StatusChanged;
            // Or it's already plugged in, so we will look for it.
            var kinect = KinectSensor.KinectSensors.FirstOrDefault(k => k.Status == KinectStatus.Connected);
            tracedF = false;

            if (kinect != null)
            {
                OpenKinect(kinect);
            }
            else
            {       MessageBox.Show("Kinect sensor not found");
            Application.Exit();
      

        }

        }
      
       private void button1_Click(object sender, EventArgs e)
        {
            
            Application.Exit();

        }
       void KinectSensors_StatusChanged(object sender, StatusChangedEventArgs e)
       {
           if (e.Status == KinectStatus.Connected)
           {
               OpenKinect(e.Sensor);
           }
       }
       private void OpenKinect(KinectSensor newSensor)
       {
           kinectSensor = newSensor;
           kinectSensor.ColorStream.Enable();
           
           //  kinectSensor.DepthStream.Range = DepthRange.Near; //only for windows sensor}
               
           kinectSensor.DepthStream.Enable(DepthImageFormat.Resolution640x480Fps30);
           this.depthPixels = new DepthImagePixel[this.kinectSensor.DepthStream.FramePixelDataLength];
           this.kinectSensor.DepthFrameReady += this.SensorDepthFrameReady;
           kinectSensor.SkeletonStream.EnableTrackingInNearRange = true;
           kinectSensor.SkeletonStream.TrackingMode = SkeletonTrackingMode.Seated;
           kinectSensor.SkeletonStream.Enable(new TransformSmoothParameters() { Correction = 0.5f, JitterRadius = 0.05f, MaxDeviationRadius = 0.05f, Prediction = 0.5f, Smoothing = 0.5f });

           // Listen to the AllFramesReady event to receive KinectSensor's data.
           kinectSensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(kinectSensor_AllFramesReady);

           // Initialize data arrays
           colorPixelData = new byte[kinectSensor.ColorStream.FramePixelDataLength];
           depthPixelData = new short[kinectSensor.DepthStream.FramePixelDataLength];
           skeletonData = new Skeleton[6];
        
           try {
           // Starts the Sensor
           kinectSensor.Start();}
            catch(Exception e){
                kinectSensor.Stop();
                MessageBox.Show("Only Kinect for windows is supported. App will now exit");
                Application.Exit();
                    }
           // Initialize a new FaceTracker with the KinectSensor
           faceTracker = new FaceTracker(kinectSensor);
       }
       private void SensorDepthFrameReady(object sender, DepthImageFrameReadyEventArgs e)
       {
               using (DepthImageFrame depthFrame = e.OpenDepthImageFrame())
               {
                   if (depthFrame != null)
                   {
                       depthFrame.CopyDepthImagePixelDataTo(this.depthPixels);
                       int minDepth = depthFrame.MinDepth;
                       int maxDepth = depthFrame.MaxDepth;

                       float fotis;
                       fotis = 0;
                       for (int i = 0; i < this.depthPixels.Length; ++i)
                       {
                           short depth = depthPixels[i].Depth;
                           fotis = fotis + depth;
                       }
                       fotis = fotis / this.depthPixels.Length; //depth
                       textBox3.Text = fotis.ToString("0.00"); //depth

                      
                       

                   }
               }
           
       }
       void kinectSensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
       {
           // Retrieve each single frame and copy the data
           using (ColorImageFrame colorImageFrame = e.OpenColorImageFrame())
           {
               if (colorImageFrame == null)
                   return;
               colorImageFrame.CopyPixelDataTo(colorPixelData);
           }

           using (DepthImageFrame depthImageFrame = e.OpenDepthImageFrame())
           {
               if (depthImageFrame == null)
                   return;
               depthImageFrame.CopyPixelDataTo(depthPixelData);
           }

           using (SkeletonFrame skeletonFrame = e.OpenSkeletonFrame())
           {
               if (skeletonFrame == null)
                   return;
               skeletonFrame.CopySkeletonDataTo(skeletonData);
           }

           // Retrieve the first tracked skeleton if any. Otherwise, do nothing.
           var skeleton = skeletonData.FirstOrDefault(s => s.TrackingState == SkeletonTrackingState.Tracked);
           if (skeleton == null)
               return;
           
           // Make the faceTracker processing the data.
           FaceTrackFrame faceFrame = faceTracker.Track(kinectSensor.ColorStream.Format, colorPixelData,
                                             kinectSensor.DepthStream.Format, depthPixelData,
                                             skeleton);

           // If a face is tracked, then we can use it.
           if (faceFrame.TrackSuccessful)
           {
               if (tracedF == false)
               {
      //             mind.PerceptionKinectFace(true);
                   tracedF = true;
               }
               
               
               // Retrieve only the Animation Units coeffs.



               var a = new float();
               var b = new float();
              

               a = faceFrame.Rotation.X;
               b = faceFrame.Rotation.Y;

               label8.Text = "Face angle Y:" + a;
               label9.Text = "Face angle X:" + b;
              
                   //("Depth:" + textBox3.Text + ",X:" + faceFrame.Translation.X.ToString().Substring(0, 5) + ",Y:" + faceFrame.Translation.Y.ToString().Substring(0, 5) + ",Z:" + faceFrame.Translation.Z.ToString().Substring(0, 5) + " ," + a.ToString("0.0000") + "," + b.ToString("0.0000") + "," + jawLowerer.ToString("0.0000") + "," + uperlip.ToString("0.0000") + "," + lipcorner.ToString("0.0000") + "," + lipstretch.ToString("0.0000") + "," + browlow.ToString("0.0000") + "," + browup.ToString("0.0000"));
               if(button6.Enabled==false)
               textBox1.AppendText(faceFrame.Translation.Y.ToString().Substring(0, 5)+ "," + a + "," + b + "," + directionfotis + System.Environment.NewLine);
               
           }
           else
               if (tracedF == true)
               {
                   tracedF = false;
               }
       }
      

       private void connectbtn_Click(object sender, EventArgs e)
       {
           directionfotis = "screen";
       }



       private void textBox1_KeyDown(object sender, KeyEventArgs e)
       {
           if (radioButton1.Checked == true | radioButton2.Checked == true)
           {
               if (e.KeyCode == Keys.Up)
                   directionfotis = "robot";
               if (e.KeyCode == Keys.Right)
                   directionfotis = "robot";
               if (e.KeyCode == Keys.Left)
                   directionfotis = "else";
               if (e.KeyCode == Keys.Down)
                   directionfotis = "screen";
           }
           else
           {
               if (e.KeyCode == Keys.Up)
                   directionfotis = "robot";
               if (e.KeyCode == Keys.Right)
                   directionfotis = "screenR";
               if (e.KeyCode == Keys.Left)
                   directionfotis = "screenL";
               if (e.KeyCode == Keys.Down)
                   directionfotis = "else";
           }

       }

       private void button3_Click(object sender, EventArgs e)
       {
           directionfotis = "robot";
       }

       private void button4_Click(object sender, EventArgs e)
       {
           directionfotis = "else";
       }

       private void button2_Click(object sender, EventArgs e)
       {
           directionfotis = "robot";
       }

       private void button5_Click(object sender, EventArgs e)
       {
           button6.Enabled = true;
           button5.Enabled = false;
           if(radioButton1.Checked==true)
               File.WriteAllText("logFront.csv", textBox1.Text);
           if (radioButton2.Checked == true)
               File.WriteAllText("logLateral.csv", textBox1.Text);
           if (radioButton3.Checked == true)
               File.WriteAllText("logFinal.csv", textBox1.Text);

           textBox1.Text = "";
       }

       private void button6_Click(object sender, EventArgs e)
       {
           button6.Enabled = false;
           button5.Enabled = true;

       }

       private void textBox1_TextChanged(object sender, EventArgs e)
       {

       }

       private void radioButton1_CheckedChanged(object sender, EventArgs e)
       {
           if (radioButton1.Checked == true)
           {
               connectbtn.Text="Gaze at screen (DOWN)";
                   button3.Text="Gaze at robot (UP)";
                   button4.Text="Gaze elsewhere (LEFT)";
                   button2.Text = "Gaze at robot (RIGHT)";
           }
       }

       private void radioButton2_CheckedChanged(object sender, EventArgs e)
       {
           if (radioButton2.Checked == true) //lateral
           {
               connectbtn.Text = "Gaze at screen (DOWN)";
               button3.Text = "Gaze at robot (UP)";
               button4.Text = "Gaze elsewhere (LEFT)";
               button2.Text = "Gaze at robot (RIGHT)";
           }
       }

       private void radioButton3_CheckedChanged(object sender, EventArgs e)
       {
           if (radioButton3.Checked == true)
           {
               connectbtn.Text = "Gaze elsewhere (DOWN)";
               button3.Text = "Gaze at robot (UP)";
               button4.Text = "Gaze screen (LEFT)";
               button2.Text = "Gaze at screen (RIGHT)";
           }
       }




   
    }
}
