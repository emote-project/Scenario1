using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using DirectShowLib;
using PipeClient;
using System.IO;
namespace VideoCaptureDemo
{
    public partial class Form1 : Form
    {
        private Capture _capture = null;
        Image<Bgr, Byte> frame;
        private VideoWriter fotis;
        Client pipec;
        static int framesps = 0;
        string vidPathFinal;
        static int selectedHeight = 0;
        static int selectedWidth = 0;
        static int selectedfps = 0;
        System.Windows.Forms.Timer mytime = new System.Windows.Forms.Timer();
        double webcam_frm_cnt = 0;
        bool start = false;
        string vidPath;
        int CameraDevice = 0;
        Video_Device[] WebCams;
        public Form1()
        {
            InitializeComponent();
            this.pipec = new Client();
            vidPathFinal = @"C:\Videos";
            pipec.MessageReceived +=
                new Client.MessageReceivedHandler(pipeClient_MessageReceived);
            mytime.Interval = 1000;
            mytime.Tick += timing;
            if (Directory.Exists(vidPathFinal) == false)
            {
                Directory.CreateDirectory(vidPathFinal);
            }
            DsDevice[] _SystemCamereas = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            WebCams = new Video_Device[_SystemCamereas.Length];
            for (int i = 0; i < _SystemCamereas.Length; i++)
            {
                WebCams[i] = new Video_Device(i, _SystemCamereas[i].Name, _SystemCamereas[i].ClassID); //fill web cam array
                
                Camera_Selection.Items.Add(WebCams[i].ToString());
            }
            if (Camera_Selection.Items.Count > 0)
            {
                Camera_Selection.SelectedIndex = 0; //Set the selected device the default
                
            }
 
        }
        private void timing(object sender, EventArgs arg)
        {
            Invoke((Action)(() => realfps.Text = framesps.ToString()));
            framesps = 0;
        }
        private void ReleaseData()
        {
            if (_capture != null)
                _capture.Dispose();
        }
        private void SetupCapture(int Camera_Identifier)
        {
            //update the selected device
            CameraDevice = Camera_Identifier;


            //Dispose of Capture if it was created before
            if (_capture != null) _capture.Dispose();
            try
            {
                //Set up capture device
                _capture = new Capture(CameraDevice);
                _capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FPS, Int16.Parse(fpstext.Text));
                if (resolution.SelectedIndex == 0)
                {
                    _capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 480);
                    _capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 640);
                }

                if (resolution.SelectedIndex == 1)
                {
                    _capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_HEIGHT, 1000);
                    _capture.SetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FRAME_WIDTH, 2000);
                    
                }
                selectedWidth= _capture.Width;
                selectedHeight = _capture.Height;
                
                selectedfps = Convert.ToInt16(_capture.GetCaptureProperty(Emgu.CV.CvEnum.CAP_PROP.CV_CAP_PROP_FPS));
               // _capture.ImageGrabbed += ProcessFrame;
              
            }
            catch (NullReferenceException excpt)
            {MessageBox.Show(excpt.Message);}

        }

        private void ProcessFrame(object sender, EventArgs arg)
        {
            try
            {
                

                frame = _capture.QueryFrame();
                if (frame != null)
                {
                    pictureBox1.Image = frame.ToBitmap();
                    Frame_lbl.Text = "Frame: " + (webcam_frm_cnt++).ToString();
                    framesps++;
                    if (start == true)
                        fotis.WriteFrame(frame);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Prepare")
                try
                {
                    if (pipec.Connected == true)
                        pipec.SendMessage("Cam" + modulenumber.Text);

                    SetupCapture(Camera_Selection.SelectedIndex);
                    
                    
                    fpstext.Text = selectedfps.ToString();
                    Frame_lbl.Text = "Frame: ";
                    webcam_frm_cnt = 0;
                    Application.Idle += ProcessFrame;
                    button1.Text = "Stop";
                    Camera_Selection.Enabled = false;
                    resolution.Enabled = false;
                    fpstext.Enabled = false;
                    
                    mytime.Enabled = true;
                }
                catch (NullReferenceException excpt)
                {
                    MessageBox.Show(excpt.Message);
                }
            else
            {
                _capture.Stop();
                try
                {

                    fotis.Dispose();
                }
                catch(Exception f) { }
                ReleaseData();
                button1.Text = "Prepare";
                Camera_Selection.Enabled = true;
                pictureBox1.Image = Properties.Resources.blank;
                resolution.Enabled = true;
                _capture.Dispose();
                pipeClient_MessageReceived("stop0");
                mytime.Enabled = false;
            }
        }

        private void idtext_TextChanged(object sender, EventArgs e)
        {
            
        }
        void pipeClient_MessageReceived(string message)
        {
            if (message.Equals("start")) 
            {
                Invoke(new Action(()=>{
                if (resolution.SelectedIndex > -1)
                    fotis = new VideoWriter(vidPath + @"\temp.avi", CvInvoke.CV_FOURCC('D', 'I', 'V', 'X'), Int16.Parse(realfps.Text), selectedWidth, selectedHeight, true);
            }));
                start=true;
                Invoke((Action)(() =>{
                    this.Text = "Capturing...";
             Frame_lbl.Text = "Frame: ";
             webcam_frm_cnt = 0;
                }));
            }
            if (message.StartsWith("stop"))
            {
                start = false;
               this.Invoke(new Action(() => this.Text = "File saved"));
                string participant = message.Substring(4);
                this.Invoke(new Action(() => idtext.Text = participant));
                string path = vidPath;
                string path2 =vidPathFinal;
                string searchPattern = "t*";
                fotis.Dispose();
                DirectoryInfo di = new DirectoryInfo(path);
                FileInfo[] files = di.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);

                foreach (FileInfo file in files)
                {
                    this.Invoke(new Action(() =>{
                    //if (File.Exists(path2))
                    //{
                    //    System.IO.File.Delete(path2);
                    //}
                    if (File.Exists(vidPathFinal + @"\Cam_" + modulenumber.SelectedItem.ToString() + "_ID_" + idtext.Text + ".avi"))
                    {
                        System.IO.File.Delete(vidPathFinal + @"\Cam_" + modulenumber.SelectedItem.ToString() + "_ID_" + idtext.Text + ".avi");
                    }
                    for (int i = 0; i < 3;i++ ) //try 3 times to move the video in case the writter still writes images
                        try
                        {
                            file.MoveTo(vidPathFinal + @"\Cam_" + modulenumber.SelectedItem.ToString() + "_ID_" + idtext.Text + ".avi");
                            break;
                        }
                        catch (Exception e)
                        { Thread.Sleep(1000); }
                    
                }));
                }
                 Invoke((Action)(() =>
                {
                    if (resolution.SelectedIndex >-1)
                        fotis = new VideoWriter(vidPath + @"\temp.avi", CvInvoke.CV_FOURCC('D', 'I', 'V', 'X'), Int16.Parse(realfps.Text), selectedWidth, selectedHeight, true);

                    Frame_lbl.Text = "Frame: ";
                    webcam_frm_cnt = 0;
                }));
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (pipec.Connected == true) pipec.Stop();
        }

        private void cntbutton_Click(object sender, EventArgs e)
        {
            
            if (!pipec.Connected)
            {
                pipec.PipeName = "\\\\.\\pipe\\serverpipe";
                pipec.Connect();

                if (pipec.Connected == true)
                {
                    this.cntbutton.Enabled = false;
                    fpstext.Enabled = true;
                    resolution.Enabled = true;
                }
            }
            else
                MessageBox.Show("Already connected");
        }

        private void modulenumber_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (modulenumber.SelectedIndex != -1)
            {
                cntbutton.Enabled = true;
                vidPath = @"C:\tempvideo" + modulenumber.SelectedItem.ToString();

                if (Directory.Exists(vidPath) == false)
                {
                    Directory.CreateDirectory(vidPath);
                }
            }
        }

        private void resolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(resolution.SelectedIndex!=-1)
            {
                Camera_Selection.Enabled = true;
                button1.Enabled = true;
            }
        }



       

       
    }
}
