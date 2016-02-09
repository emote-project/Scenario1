using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Microsoft.Expression.Encoder.Devices;

//using WebcamControl;
using System.Drawing.Imaging;
using System.IO;

namespace WoZInterface
{
    public partial class frmMainWindow : Form
    {

        WOZInterfaceClient thalamusClient;

        public frmMainWindow()
        {
            InitializeComponent();
            thalamusClient = new WOZInterfaceClient(this);
        }

        public void ChangeTitle(bool running)
        {
            this.Text = "Control Panel " + (running ? "(running)" : "(stopped)");
        }

        public void SetParticipantDetails(int participantId, string participantName)
        {
            this.txtParticipantName.Text = participantName;
            this.txtParticipantId.Text = participantId.ToString();
        }


        public void SetGameStateBox(bool running)
        {
            if (running == true)
                this.txtGameState.Text = (string)"true";
            else
                this.txtGameState.Text = "false";
        }


        private void frmMainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            thalamusClient.Dispose();
        }

        private void ShowCompass_Click(object sender, EventArgs e)
        {
            int i;
            if (!int.TryParse(txtParticipantId.Text, out i)) i = 0;

            thalamusClient.WOZPublisher.CompassTool(true);
        }
    }
}
