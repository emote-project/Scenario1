using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Aldebaran.Proxies;
using Renci.SshNet;

namespace WoOzSounds
{
    public partial class frmWoOzSounds : Form
    {
        private enum NaoStatus
        {
            Disconnected,
            Connecting,
            Standby,
            PlayingSound
        }

        AudioPlayerProxy audioProxy;
        BehaviorManagerProxy behaviorProxy;
        NaoStatus status = NaoStatus.Disconnected;
        private NaoStatus Status
        {
            get { return status; }
            set { 
                status = value;
                lblStatus.Invoke((MethodInvoker)(() => UpdateStatus()));
            }
        }
        bool shutdown = false;
        Thread tConnect;

        string idleBehavior = "activeIdle";

        bool isPlaying = false;
        bool IsPlaying
        {
            get { return isPlaying; }
            set { 
                isPlaying = value;
                if (isPlaying)
                {
                    Status = NaoStatus.PlayingSound;
                }
                else {
                    Status = NaoStatus.Standby;
                }
            }
        }

        string robotAddress = Properties.Settings.Default.RobotAddress;
        string ipAddress = "";
        int[] keyIndexes = new int[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0', 'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', 'A', 'S', 'D', 'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' };

        public frmWoOzSounds()
        {
            InitializeComponent();
            string ipAddress = robotAddress;
        }


        bool shiftPressed = false;
        private void KeyDownHandler(object sender, KeyEventArgs k)
        {
            if (k.Shift) shiftPressed = true;
        }

        private void KeyPressedHandler(object sender, KeyEventArgs k)
        {
            int keyIndex;
            if (k.KeyCode >= Keys.NumPad1 && k.KeyCode <= Keys.NumPad9)
            {
                if (k.KeyCode == Keys.NumPad9) keyIndex = Array.IndexOf(keyIndexes, (int)'9');
                else keyIndex = Array.IndexOf(keyIndexes, k.KeyValue-48);
            }
            else
            {
                keyIndex = Array.IndexOf(keyIndexes, k.KeyValue);
            }
            Console.WriteLine(String.Format("code: {0} shift: {1} key: {2}", (char)k.KeyCode, k.Shift, keyIndex));
            if (k.Shift || shiftPressed) //Change category
            {
                if (keyIndex != -1 && lstCategories.Items.Count > 0 && keyIndex < lstCategories.Items.Count)
                {
                    SwitchCategory(lstCategories.Items[keyIndex] as string);
                }
            }
            else
            { //Play a file
                if (keyIndex != -1 && lstCategories.SelectedItem != null && lstFiles.Items.Count > 0 && keyIndex < lstFiles.Items.Count)
                {
                    lstFiles.SelectedIndex = keyIndex;

                    string file = lstFiles.Items[keyIndex] as string;
                    PlaySound(SelectedCategory, file);
                }
            }
            shiftPressed = k.Shift;
        }

        private void UpdateStatus()
        {
            lblStatus.Text = Status.ToString();
            if (Status == NaoStatus.Disconnected) btnConnection.Text = "Connect";
            else btnConnection.Text = "Disconnect";
            btnConnection.Enabled = true;
        }

        /*void KeepConnected()
        {
            while (!shutdown)
            {
                if (Status == NaoStatus.Disconnected || Status==NaoStatus.Connecting)
                {
                    Connect();
                }
                else if (Status == NaoStatus.Standby)
                {

                    try {
                        if (audioProxy.ping())
                        {
                            Thread.Sleep(500);
                        }
                        else
                        {
                            Status = NaoStatus.Connecting;
                        }
                    }catch {
                        Status = NaoStatus.Connecting;
                    }
                }
                Thread.Sleep(1000);
            }
            Status = NaoStatus.Disconnected;
        }*/


        Dictionary<string, Dictionary<string, int>> sounds = new Dictionary<string, Dictionary<string, int>>();

        private void Connect() {
            Console.WriteLine("Looking for NAO's host '" + robotAddress + "'...");
            IPAddress[] ip = Dns.GetHostAddresses(robotAddress);
            if (ip.Length == 0)
            {
                Console.WriteLine("Unable to find host '" + robotAddress + "'");
                Thread.Sleep(2000);
            }
            else
            {
                foreach (IPAddress i in ip)
                {
                    if (i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ipAddress = i.ToString();
                        break;
                    }
                }
                Console.WriteLine("Resolved '" + robotAddress + "' as " + ipAddress + "!");
                audioProxy = new AudioPlayerProxy(ipAddress, 9559);
                Console.WriteLine("Created AudioPlayer proxy.");
                

                var files = Directory.EnumerateFiles(@"wav", "*.mp3", SearchOption.AllDirectories);
                sounds.Clear();
                lstCategories.Invoke((MethodInvoker)(() =>
                {
                    lstCategories.Items.Clear();
                    lstFiles.Items.Clear();
                    lstCategories.SelectedItem = null;
                    lstFiles.Enabled = false;
                    btnPlayRandom.Enabled = false;
                }));
                foreach (string file in files)
                {
                    string category = Path.GetDirectoryName(file).Substring(Path.GetDirectoryName(file).LastIndexOf(Path.DirectorySeparatorChar)+1);
                    string sound = Path.GetFileNameWithoutExtension(file);
                    if (!sounds.ContainsKey(category))
                    {
                        sounds[category] = new Dictionary<string, int>();
                        lstCategories.Invoke((MethodInvoker)(() =>
                        {
                            lstCategories.Items.Add(String.Format("[Shift+{0}]:{1}", (char) keyIndexes[lstCategories.Items.Count], category));
                        }));
                    }
                    int soundId = -1;
                    try {
                        soundId = audioProxy.loadFile("/var/persistent/home/nao/wav/" + category + "/" + sound + ".mp3");
                        Console.WriteLine("Loaded file '" + category + "/" + sound + ".mp3'");
                    }catch (Exception e) {
                        Console.WriteLine("Unable to load file '" + category + "/" + sound + ".mp3': " + e.Message + (e.InnerException!=null?": " + e.InnerException.Message:""));
                    }
                    if (soundId != -1)
                    {
                        sounds[category][sound] = soundId;
                    }
                    else if (sounds[category].ContainsKey(sound))
                    {
                        sounds[category].Remove(sound);
                    }
                }

                behaviorProxy = new BehaviorManagerProxy(ipAddress, 9559);
                Console.WriteLine("Created BehaviorManager proxy.");
                if (behaviorProxy.isBehaviorPresent(idleBehavior))
                {
                    if (!behaviorProxy.isBehaviorRunning(idleBehavior))
                    {   
                        behaviorProxy.post.runBehavior(idleBehavior);
                    }
                }

                Status = NaoStatus.Standby;
            }
        }

        private void frmWoOzSounds_FormClosing(object sender, FormClosingEventArgs e)
        {
            Disconnect();
            Application.Exit();
        }

        private void Disconnect()
        {
            shutdown = true;

            try
            {
                audioProxy.Dispose();
            }
            catch { }

            if (status != NaoStatus.Disconnected && status != NaoStatus.Connecting)
            {
                Console.WriteLine("Stopping idle behavior...");
                try
                {
                    if (behaviorProxy == null) behaviorProxy = new BehaviorManagerProxy(ipAddress, 9559);
                    behaviorProxy.post.stopAllBehaviors();
                }
                catch { }

                Console.WriteLine("Shutting down motors...");
                try
                {
                    MotionProxy motionProxy = new MotionProxy(ipAddress, 9559);
                    motionProxy.post.setStiffnesses("Body", 0);
                }
                catch { }
            }
            Console.WriteLine("Waiting for threads to finish...");
            lstCategories.Items.Clear();
            lstFiles.Items.Clear();
            if (tConnect != null) tConnect.Abort();
            Status = NaoStatus.Disconnected;
            Console.WriteLine("Done. Exiting.");
        }

        private void btnConnection_Click(object sender, EventArgs e)
        {
            if (Status == NaoStatus.Disconnected)
            {
                shutdown = false;
                robotAddress = txtNaoAddress.Text;
                Properties.Settings.Default.RobotAddress = robotAddress;
                Properties.Settings.Default.Save();
                btnConnection.Enabled = false;
                Status = NaoStatus.Connecting;
                tConnect = new Thread(new ThreadStart(Connect));
                tConnect.Start();
            }
            else
            {
                Disconnect();
            }
        }

        private string SelectedCategory
        {
            get {
                if (lstCategories.SelectedItem == null) return "";
                string c = lstCategories.SelectedItem as string;
                return c.Substring(c.IndexOf(":") + 1); 
            }
        }

        private string SelectedFile
        {
            get
            {
                if (lstFiles.SelectedItem == null) return "";
                string c = lstFiles.SelectedItem as string;
                return c.Substring(c.IndexOf(":") + 1);
            }
        }


        private void SwitchCategory(string category)
        {
            lstFiles.Items.Clear();
            lstCategories.SelectedItem = category;
            category = category.Substring(category.IndexOf(":") + 1);
            if (sounds.ContainsKey(category) && sounds[category].Count > 0)
            {
                lstFiles.Enabled = true;
                btnPlayRandom.Enabled = true;
                foreach (string file in sounds[category].Keys)
                {
                    lstFiles.Items.Add(String.Format("[{0}]:{1}", (char) keyIndexes[lstFiles.Items.Count], file));
                }
            }
            else
            {
                lstFiles.Enabled = false;
                btnPlayRandom.Enabled = false;
            }
        }

        Random rand = new Random();
        private void btnPlayRandom_Click(object sender, EventArgs e)
        {
            if (lstCategories.SelectedItem!=null && lstFiles.Items.Count > 0)
            {
                string file = lstFiles.Items[rand.Next(0, lstFiles.Items.Count)] as string;
                PlaySound(SelectedCategory, file);
            }
        }

        void PlaySound(string category, string file)
        {
            file = file.Substring(file.IndexOf(":") + 1);
            if (sounds.ContainsKey(category) && sounds[category].ContainsKey(file))
            {
                (new Thread(new ParameterizedThreadStart(PlaySound))).Start(sounds[category][file]);
            }
        }
        void PlaySound(object oFileId)
        {
            int fileId = (int)oFileId;
            lock (audioProxy)
            {
                try
                {
                    IsPlaying = true;
                    audioProxy.play(fileId);
                    IsPlaying = false;
                }
                catch
                {
                    Console.WriteLine("Lost connection with NAO.");
                    Disconnect();
                    IsPlaying = false;
                }
            }
        }

        private void lstFiles_DoubleClick(object sender, EventArgs e)
        {
            if (lstCategories.SelectedItem != null && lstFiles.SelectedItem != null)
            {
                PlaySound(SelectedCategory, SelectedFile);
            }
        }

        private void lstCategories_MouseClick(object sender, MouseEventArgs e)
        {
            SwitchCategory(SelectedCategory);
        }

    }
}
