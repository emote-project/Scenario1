using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Thalamus;
namespace UtteranceManager
{
    public partial class frmUtteranceTester : Form
    {

        private UtteranceClient client;
        public frmUtteranceTester(string character = "")
        {
            InitializeComponent();
            client = new UtteranceClient(character);

            client.ClientConnected += delegate() { StatusMessage("connected", System.Drawing.Color.Green); };
            client.ClientDisconnectedFromThalamus += delegate(string name, string oldClientId) { StatusMessage("NOT connected", System.Drawing.Color.Red); };
            client.Library.LibraryModified += ((UtteranceLibrary.LibraryModifiedHandler)(() => this.Invoke((MethodInvoker)(() => PopulateCategories()))));
        }

        public void StatusMessage(string msg, Color color)
        {
            MethodInvoker action = delegate
            {
                lblStatus.Text = msg;
                lblStatus.ForeColor = color;
            };
            lblStatus.BeginInvoke(action);
        }

        private void frmUtteranceTester_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Dispose();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string cat = (lstCategories.SelectedItem == null ? "-" : lstCategories.SelectedItem.ToString()) + "." + (lstSubcategories.SelectedItem == null ? "-" : lstSubcategories.SelectedItem.ToString());
            client.UtterancePublisher.PerformUtterance(DateTime.Now.Ticks+"", txtUtterance.Text, cat);
        }

        private void frmUtteranceTester_Load(object sender, EventArgs e)
        {
            dontUpdate = true;
            chkAutoLoadonStartup.Checked = Properties.Settings.Default.AutoLoadLibrary;
            if (chkAutoLoadonStartup.Checked && Properties.Settings.Default.LoadedLibrary!="")
            {
                if (!LoadLibrary(Properties.Settings.Default.LoadedLibrary))
                {
                    Properties.Settings.Default.LoadedLibrary = "";
                    Properties.Settings.Default.Save();
                }
            }
            txtPrependTags.Text = Properties.Settings.Default.PrependTags;
            dontUpdate = false;
        }

        private void btnLoadLibrary_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Microsoft Excel Worksheet|*.xls;*.xlsx|All Files|*.*";
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                LoadLibrary(openFileDialog.FileName);
            }
        }

        private void chkAutoLoadonStartup_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoLoadonStartup.Checked) Properties.Settings.Default.LoadedLibrary = txtLoadedLibrary.Text;
            Properties.Settings.Default.AutoLoadLibrary = chkAutoLoadonStartup.Checked;
            Properties.Settings.Default.Save();
        }



        private bool LoadLibrary(string filename)
        {
            try {
                client.Library.LoadLibrary(filename);
                txtLoadedLibrary.Text = filename;
                Properties.Settings.Default.LoadedLibrary = filename;
                Properties.Settings.Default.Save();
                PopulateCategories();
                return true;
            }
            catch(Exception e)
            {
                MessageBox.Show("Unable to load the utterance library '" + filename + "': " + e.Message, "Load Utterance Manager Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void lstCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateSubcategories();
        }

        private void PopulateCategories()
        {
            string selected = lstCategories.SelectedItem != null ? lstCategories.SelectedItem.ToString() : "";
            lstCategories.Items.Clear();
            foreach (string s in client.Library.Categories.Keys) lstCategories.Items.Add(s);

            lstSubcategories.Enabled = false;
            for (int i = 0; i < lstCategories.Items.Count; i++)
            {
                if (lstCategories.Items[i].ToString() == selected)
                {
                    lstCategories.SelectedIndex = i;
                    break;
                }
            }
            PopulateSubcategories();
        }


        private void PopulateSubcategories()
        {
            if (lstCategories.SelectedItem == null || !client.Library.Categories.ContainsKey(lstCategories.SelectedItem.ToString()))
            {
                lstSubcategories.Items.Clear();
                lstSubcategories.Enabled = false;
                return;
            }
            lstSubcategories.Enabled = true;
            string selected = lstSubcategories.SelectedItem != null ? lstSubcategories.SelectedItem.ToString() : "";
            lstSubcategories.Items.Clear();

            List<string> filteredSubcategories = client.Library.FilterSubcategories(lstCategories.SelectedItem.ToString());

            foreach (string s in client.Library.Categories[lstCategories.SelectedItem.ToString()]) lstSubcategories.Items.Add(s);

            for (int i = 0; i < lstSubcategories.Items.Count; i++)
            {
                if (lstSubcategories.Items[i].ToString() == selected)
                {
                    lstSubcategories.SelectedIndex = i;
                    break;
                }
            }

            if (lstSubcategories.Items.Count == 1) lstSubcategories.SelectedIndex = 0;
            PopulateUtterances();
        }

        private void PopulateUtterances()
        {
            txtUtterance.Text = "";
            if (lstCategories.SelectedItem == null
                || !client.Library.Categories.ContainsKey(lstCategories.SelectedItem.ToString())
                || lstSubcategories.SelectedItem == null
                || !client.Library.Categories[lstCategories.SelectedItem.ToString()].Contains(lstSubcategories.SelectedItem.ToString()))
            {
                lstUtterances.Items.Clear();
                lstUtterances.Enabled = false;
                return;
            }
            lstUtterances.Enabled = true;
            int selectedIndex = lstUtterances.SelectedIndex;
            string selected = lstUtterances.SelectedItem != null ? lstUtterances.SelectedItem.ToString() : "";
            lstUtterances.Items.Clear();

            List<string> filteredUtterances = client.Library.FilterUtterances(lstCategories.SelectedItem.ToString(), lstSubcategories.SelectedItem.ToString());
            foreach (string s in filteredUtterances) lstUtterances.Items.Add(s);

            bool found = false;
            for (int i = 0; i < lstUtterances.Items.Count; i++)
            {
                if (lstUtterances.Items[i].ToString() == selected)
                {
                    lstUtterances.SelectedIndex = i;
                    found = true;
                    break;
                }
            }
            if (!found) lstUtterances.SelectedIndex = selectedIndex;
        }

        private void lstUtterances_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtUtterance.Text = lstUtterances.SelectedItem.ToString();
        }

        private void lstSubcategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateUtterances();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        bool dontUpdate = false;
        private void txtPrependTags_TextChanged(object sender, EventArgs e)
        {
            if (dontUpdate) return;
            Properties.Settings.Default.PrependTags = txtPrependTags.Text;
            Properties.Settings.Default.Save();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void txtLoadedLibrary_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
