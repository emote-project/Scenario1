using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Controls;

using WebcamControl;
using System.Drawing.Imaging;
using System.IO;

using System.Windows;

namespace WOZInterface
{
    public enum VIEWTYPE
    {
        SETTINGS, CAMERA
    }

    public enum CameraStatus
    {
        OFF,
        IDLE,
        READY,
        RECORDING
    }
    
    public class CameraManager
    {
        public SettingsView settingsView = null;
        public CameraView cameraView = null;
        public Grid MyHolder = null;
        
        public string mMessage = "";
        int mCameraID = 0;

        public CameraStatus mCameraStatus;

        public string mTempPath;

        public CameraManager(int cameraID,  Grid holder, int height = 240, int width = 320)
        {
            mCameraID = cameraID;  
            MyHolder = holder;

            ChangeViewTo(VIEWTYPE.SETTINGS);

            InitCamera(height, width);

            mCameraStatus = CameraStatus.IDLE;
 
        }
        

        /* */
        public void StartCamera()
        {

            try
            {
                cameraView.WebCamCtrl.StartCapture();
            }
            catch (Exception)
            {
                SetMessage("Camera failed to Start");
            }


            mCameraStatus = CameraStatus.READY;
        }

        public void StopCapture()
        {
            StopRecording();
            cameraView.WebCamCtrl.StopCapture();
            mCameraStatus = CameraStatus.OFF;
        }

        public void StartRecording()
        {
            SetMessage("Recording!");

            try
            {
                cameraView.WebCamCtrl.StartRecording();
            }
            catch (Exception)
            {
                SetMessage("Camera failed to Record!");
            }


            mCameraStatus = CameraStatus.RECORDING;
        }

        public void StopRecording()
        {
            //if (mCameraStatus == CameraStatus.RECORDING)
            //{
            //    cameraView.WebCamCtrl.StopRecording();
            //    mCameraStatus = CameraStatus.READY;

            //    //string tempPath = @"C:\tempvideo" + mCameraID;
            //    string finalPath = "C:\\WOZInterface\\" + mWOZManager.mparticipantDetails.mID + "\\Video\\";   //@"C:\videos\";
            //    string searchPattern = "W*";

            //    DirectoryInfo di = new DirectoryInfo(mTempPath);
            //    FileInfo[] files = di.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
                
            //    // If the folder folder doesn't exist create it
            //    Directory.CreateDirectory(finalPath);
                
            //    foreach (FileInfo file in files)
            //    {
            //        if (File.Exists(finalPath))
            //        {
            //            System.IO.File.Delete(finalPath);
            //        }
            //        if (File.Exists(finalPath + mCameraID + "_PName_" + mWOZManager.mparticipantDetails.mName + "_PID_" + mWOZManager.mparticipantDetails.mID + ".wmv"))
            //        {
            //            System.IO.File.Delete(finalPath + mCameraID + "_PName_" + mWOZManager.mparticipantDetails.mName + "_PID_" + mWOZManager.mparticipantDetails.mID + ".wmv");
            //        }

            //        file.MoveTo(finalPath + mCameraID + "_PName_" + mWOZManager.mparticipantDetails.mName + "_PID_" + mWOZManager.mparticipantDetails.mID + ".wmv");

            //    } 
            //}

        }


        /* */
        private void InitCamera(int height, int width)
        {
            cameraView = new CameraView(this);
            
            settingsView.FindDevices();
            
            settingsView.mVideoBinding = new Binding("SelectedValue");
            settingsView.mAudioBinding = new Binding("SelectedValue");

            settingsView.mVideoBinding.Source = settingsView.VideoDevices;
            settingsView.mAudioBinding.Source = settingsView.AudioDevices;

            cameraView.WebCamCtrl.SetBinding(Webcam.VideoDeviceProperty, settingsView.mVideoBinding);
            cameraView.WebCamCtrl.SetBinding(Webcam.AudioDeviceProperty, settingsView.mAudioBinding);

            /* Create directory for saving video files. */
            //string vidPath = @"C:\tempvideo1";
            mTempPath = @"C:\tempvideo" + mCameraID;

            if (Directory.Exists(mTempPath) == false)
                Directory.CreateDirectory(mTempPath);

            // Set some properties of the Webcam control
            cameraView.WebCamCtrl.VideoDirectory = mTempPath;
            cameraView.WebCamCtrl.VidFormat = VideoFormat.wmv;
            cameraView.WebCamCtrl.PictureFormat = ImageFormat.Jpeg;
            cameraView.WebCamCtrl.FrameRate = 30;
            cameraView.WebCamCtrl.FrameSize = new System.Drawing.Size(width, height);
        }

        /// <summary>
        /// Changes view to the passed VIEWTYPE
        /// </summary>
        public void ChangeViewTo(VIEWTYPE viewType)
        {
            MyHolder.Children.Clear();

            switch (viewType)
            {
                case VIEWTYPE.SETTINGS:
                    if (this.settingsView == null)
                    {
                        this.settingsView = new SettingsView(this);
                    }
                    MyHolder.Children.Add(settingsView);
                    break;

                case VIEWTYPE.CAMERA:
                    MyHolder.Children.Remove(settingsView);
                    MyHolder.Children.Add(cameraView);
                    StartCamera();
                    break;

                default:
                    break;
            }
        }

        public void SetMessage(string message)
        {
            mMessage = message;
        }

        public string GetMessage()
        {
            return mMessage;
        }
    }
}
