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
using PipeClient;

namespace KinectModule
{


    public partial class Form1 :  Form

    {
        private static System.Timers.Timer aTimer;
        Client pipec;
        KinectSensor kinectSensor;
        FaceTracker faceTracker;
        private byte[] colorPixelData;
        private short[] depthPixelData;
        private DepthImagePixel[] depthPixels;
        private Skeleton[] skeletonData;
        public bool tracedF;
        public bool timenabled;
        public bool timenabled2;
        public Form1()
        {
            InitializeComponent();
            aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = 200;
            this.pipec = new Client();
            pipec.MessageReceived +=
                new Client.MessageReceivedHandler(pipeClient_MessageReceived);

            timenabled = false; //timer for depth
            timenabled2 = false; //timer for rest kinect info

            aTimer.Enabled = true;
            GC.KeepAlive(aTimer);
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
        public  void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (timenabled == true)
            {
                timenabled = false;
                timenabled2 = false;
            }
            else
            {
                timenabled = true;
                timenabled2 = true;
            }

        }
       private void button1_Click(object sender, EventArgs e)
        {
            if (pipec.Connected == true)
            pipec.Stop(); 
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
           
           //    kinectSensor.DepthStream.Range = DepthRange.Near; //only for windows sensor}
               
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
           if (timenabled == true)
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

                      
                       timenabled = false;

                   }
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
               if (timenabled2 == true)
               {
               // Retrieve only the Animation Units coeffs.
               var AUCoeff = faceFrame.GetAnimationUnitCoefficients();
               //var fotis = faceFrame.GetProjected3DShape();
               //var fotis1 = faceFrame.GetAnimationUnitCoefficients();
               var jawLowerer = AUCoeff[AnimationUnit.JawLower];
               var uperlip = AUCoeff[AnimationUnit.LipRaiser];
               var lipcorner = AUCoeff[AnimationUnit.LipCornerDepressor];
               var lipstretch = AUCoeff[AnimationUnit.LipStretcher];
               var browlow = AUCoeff[AnimationUnit.BrowLower];
               var browup = AUCoeff[AnimationUnit.BrowRaiser];

               var a = new float();
               var b = new float();
               a = faceFrame.Rotation.X;
               b = faceFrame.Rotation.Y;
               label1.Text = "Jaw:" + jawLowerer;
               label2.Text = "Lip:" + uperlip;
               label3.Text = "Lip corn:" + lipcorner;
               label4.Text = "Lip stre:" + lipstretch;
               label5.Text = "Brow low:" + browlow;
               label6.Text = "Brow out:" + browup;
               label8.Text = "Face angle Y:" + a;
               label9.Text = "Face angle X:" + b;
               if (pipec.Connected == true)
                   try
                   {
                       pipec.SendMessage("1Depth:" + textBox3.Text.Substring(0, 7) + ",X:" + faceFrame.Translation.X.ToString().Substring(0, 5) + ",Y:" + faceFrame.Translation.Y.ToString().Substring(0, 5) + ",Z:" + faceFrame.Translation.Z.ToString().Substring(0, 5) + " ," + a.ToString("0.0000").Substring(0, 6) + "," + b.ToString("0.0000").Substring(0, 6) + "," + jawLowerer.ToString("0.0000").Substring(0, 6) + "," + uperlip.ToString("0.0000").Substring(0, 6) + "," + lipcorner.ToString("0.0000").Substring(0, 6) + "," + lipstretch.ToString("0.0000").Substring(0, 6) + "," + browlow.ToString("0.0000").Substring(0, 6) + "," + browup.ToString("0.0000").Substring(0, 6));
                   }
                   catch (Exception d) { }
                   timenabled2 = false;
               }
           }
           else
               if (tracedF == true)
               {
                   tracedF = false;
               }
       }
       void pipeClient_MessageReceived(string message)
       {
           Invoke(new PipeClient.Client.MessageReceivedHandler(DisplayReceivedMessage),
           new object[] { message });
       }
       void DisplayReceivedMessage(string message)
       {
           //MessageBox.Show("meesage" + message);

           
       }

       private void connectbtn_Click(object sender, EventArgs e)
       {
           if (!pipec.Connected)
           {
               pipec.PipeName = "\\\\.\\pipe\\serverpipe";
               pipec.Connect();
               this.connectbtn.Enabled = false;

           }
           else
               MessageBox.Show("Already connected");
       }

       private void Form1_FormClosing(object sender, FormClosingEventArgs e)
       {
           if(pipec.Connected==true)
                          pipec.Stop();

       }

       private void Form1_Load(object sender, EventArgs e)
       {

       }


   
    }
}
