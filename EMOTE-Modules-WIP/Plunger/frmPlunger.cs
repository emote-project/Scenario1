using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using EmoteEvents;
using Thalamus;

namespace Plunger
{
    public partial class frmPlunger : Form
    {
        PlungerClient plunger;

        Image disabledButton = null;
        Image enabledButton = null;
        string utteranceFinishedIdText = "<UtteranceFinished ID>";

        public Dictionary<string, PML> LastMessages = new Dictionary<string, PML>();

        public frmPlunger(string character)
        {
            InitializeComponent();
            UnselectAction(radSkipTurn);
            plunger = new PlungerClient(character);
            plunger.EnercitiesGameStateTurnChanged += (PlungerClient.EnercitiesGameStateTurnChangedHandler)((egi) => UpdateEnercitiesStatus(egi));
            plunger.SpeakAction += (PlungerClient.SpeakActionHandler)((id, text) => UpdateSpeachStatus(id, text));
            plunger.UtteranceStarted += (PlungerClient.UtteranceStartedHandler)((id) => UpdateUtteranceStarted(id));
            plunger.UtteranceFinished += (PlungerClient.UtteranceFinishedHandler)((id) => UpdateUtteranceFinished(id));
            txtUtteranceFinishedId.Text = utteranceFinishedIdText;
        }

        private void tmrMonitor_Tick(object sender, EventArgs e)
        {
            if (plunger == null) return;
            if (dgvMonitor.RowCount-1 != plunger.Messages.Count) ResetDGV();
            for(int i=0;i<plunger.Messages.Count;i++)
            {
                string msg = plunger.Messages[i];
                if (plunger.MessagesTime.ContainsKey(msg))
                {
                    dgvMonitor.Rows[i].Cells[1].Value = (DateTime.Now - plunger.MessagesTime[msg]).ToString(@"mm\:ss");
                }
                if (plunger.LastMessages.ContainsKey(msg) && (!LastMessages.ContainsKey(msg) || LastMessages[msg] != plunger.LastMessages[msg]))
                {
                    LastMessages[msg] = plunger.LastMessages[msg];
                    dgvMonitor.Rows[i].Cells[2].Value = LastMessages[msg].ToStringSimple().Replace(";", ";  ");
                }
            }

            if (plunger.IsConnected) lblThalamusStatus.Text = "Connected.";
            else lblThalamusStatus.Text = "Searching for character '" + plunger.CharacterName + "'";

        }

        private void UpdateEnercitiesStatus(EnercitiesGameInfo egi)
        {
            lblEnercitiesStatus.Invoke((MethodInvoker)(()=>
                lblEnercitiesStatus.Text = String.Format("Level: {0}; Current Role: {1}", egi.Level, egi.CurrentRole)
            ));
        }

        private void UpdateUtteranceStarted(string id)
        {
            lblSpeechStatus.Invoke((MethodInvoker)(()=>
                lblSpeechStatus.Text = "Performing:" + id
            ));
        }

        private void UpdateUtteranceFinished(string id)
        {
            lblSpeechStatus.Invoke((MethodInvoker)(()=>
                lblSpeechStatus.Text = "Done:" + id
            ));
        }

        private void UpdateSpeachStatus(string id, string text)
        {
            lblSpeechStatus.Invoke((MethodInvoker)(()=> {
                lblLastSpeech.Text = text;
                lblSpeechStatus.Text = "Waiting:" + id;
            }));
        }

        private void ResetDGV()
        {
            dgvMonitor.Rows.Clear();
            for (int i = 0; i < plunger.Messages.Count; i++)
            {
                dgvMonitor.Rows.Add(plunger.Messages[i], "n/a", "");
            }
        }

        private void frmPlunger_Load(object sender, EventArgs e)
        {
            tmrMonitor.Enabled = true;
            enabledButton = btnAction.BackgroundImage;
            disabledButton = Image.FromHbitmap(MakeGrayscale(new Bitmap(btnAction.BackgroundImage)).GetHbitmap());
            btnAction.Image = disabledButton;
        }

        public void CollectMessages()
        {
            if (plunger == null) return;
            List<String> eventNames = new List<string>();
            foreach (string eventName in plunger.AllMessages)
            {
                if (!eventNames.Contains(eventName)) eventNames.Add(eventName);
            }

            lstEventNameFilters.SuspendLayout();
            lstEventNameFilters.Items.Clear();
            foreach (string e in eventNames)
            {
                lstEventNameFilters.Items.Add(e, plunger.Messages.Contains(e));
            }
            lstEventNameFilters.ResumeLayout();
        }


        private void btnAction_Click(object sender, EventArgs e)
        {
            if (radSkipTurn.Checked)
            {
                UnselectAction(radSkipTurn);
                plunger.PlungePublisher.SkipTurn();
                MessageBox.Show("Desentupimento done: SkipTurn()", "Desentupidor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (radUtteranceFinished.Checked)
            {
                UnselectAction(radUtteranceFinished);
                string id = txtUtteranceFinishedId.Text == utteranceFinishedIdText ? "" : txtUtteranceFinishedId.Text;
                plunger.PlungePublisher.UtteranceFinished(id);
                MessageBox.Show("Desentupimento done: UtteranceFinished(id=\"" + id + "\")", "Desentupidor", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void radSkipTurn_CheckedChanged(object sender, EventArgs e)
        {
            if (plunger == null || !plunger.IsConnected) UnselectAction(radSkipTurn);
            if (radSkipTurn.Checked) SelectAction(radSkipTurn);
        }
        RadioButton selectedCtrl = null;

        void SelectAction(RadioButton ctrl)
        {
            if (selectedCtrl != null) UnselectAction(selectedCtrl);
            btnAction.Enabled = true;
            btnAction.Text = "Click to Desentupir";
            btnAction.Image = enabledButton;
            ctrl.BackColor = Color.IndianRed;
            selectedCtrl = ctrl;
            (new Thread(new ThreadStart(() =>
            {
                Thread.Sleep(1500);
                if (selectedCtrl == ctrl) ctrl.Invoke((MethodInvoker)(()=>UnselectAction(ctrl)));
            }))).Start();
        }

        void UnselectAction(RadioButton ctrl)
        {
            selectedCtrl = null;
            ctrl.BackColor = SystemColors.Control;
            ctrl.Update();
            ctrl.Checked = false;
            btnAction.Enabled = false;
            btnAction.Image = disabledButton;
            btnAction.Text = "";
        }

        // function found on http://www.switchonthecode.com/tutorials/csharp-tutorial-convert-a-color-image-to-grayscale
        public static Bitmap MakeGrayscale(Bitmap original)
        {
            //make an empty bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            for (int i = 0; i < original.Width; i++)
            {
                for (int j = 0; j < original.Height; j++)
                {
                    //get the pixel from the original image
                    Color originalColor = original.GetPixel(i, j);

                    //create the grayscale version of the pixel
                    int grayScale = (int)((originalColor.R * .3) + (originalColor.G * .59)
                        + (originalColor.B * .11));

                    //create the color object
                    Color newColor = Color.FromArgb(originalColor.A, grayScale, grayScale, grayScale);

                    //set the new image's pixel to the grayscale version
                    newBitmap.SetPixel(i, j, newColor);
                }
            }

            return newBitmap;
        }

        private void btnRefreshMonitoredMessagesList_Click(object sender, EventArgs e)
        {
            CollectMessages();
        }

        private void lstEventNameFilters_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void btnSetMonitoredMessages_Click(object sender, EventArgs e)
        {
            List<string> msgs = new List<string>();
            for (int i = 0; i < lstEventNameFilters.Items.Count; i++)
            {
                if (lstEventNameFilters.GetItemChecked(i)) msgs.Add(lstEventNameFilters.Items[i].ToString());
            }
            plunger.MonitorSettings.Messages = msgs;
            plunger.MonitorSettings.Save();
            plunger.RegisterMessages(msgs);
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 1) CollectMessages();
        }

        private void radNothing_CheckedChanged(object sender, EventArgs e)
        {
            if (radUtteranceFinished.Checked) SelectAction(radUtteranceFinished);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> msgs = new List<string>();
            for (int i = 0; i < lstEventNameFilters.Items.Count; i++)
            {
                if (lstEventNameFilters.GetItemChecked(i)) msgs.Add(lstEventNameFilters.Items[i].ToString());
            }
            plunger.MonitorSettings.Messages = msgs;
            plunger.MonitorSettings.Save();
        }

        private void frmPlunger_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (plunger!=null) plunger.Dispose();
        }

        private void txtUtteranceFinishedId_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUtteranceFinishedId_Enter(object sender, EventArgs e)
        {
            if (txtUtteranceFinishedId.Text == utteranceFinishedIdText)
            {
                txtUtteranceFinishedId.Text = "";
                txtUtteranceFinishedId.Font = new System.Drawing.Font(txtUtteranceFinishedId.Font, FontStyle.Regular);
            }

        }

        private void txtUtteranceFinishedId_Leave(object sender, EventArgs e)
        {
            if (txtUtteranceFinishedId.Text == "")
            {
                txtUtteranceFinishedId.Text = utteranceFinishedIdText;
                txtUtteranceFinishedId.Font = new System.Drawing.Font(txtUtteranceFinishedId.Font, FontStyle.Italic); ;
            }
        }
    }
}
