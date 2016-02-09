using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scenario1SpeechManager
{
    public partial class frmSpeechManager : Form
    {
        Scenario1SMClient speechClient;
        string selectedSpeechType = "";

        public frmSpeechManager(string character = "")
        {
            InitializeComponent();
            SetupControls();
            speechClient = new Scenario1SMClient(character);
        }


        private void btnLoadSMF_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Speech Manager Database|*.smdb|All Files|*.*";
            openFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string error = "";
                if ((error = speechClient.SpeechManager.LoadDB(openFileDialog.FileName)) != "")
                {
                    MessageBox.Show("Unable to load the file '" + txtSMF.Text + "': " + error, "Load Speech Manager Database", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    updateSpeechList();
                    txtSMF.Text = openFileDialog.FileName;
                }
            }
        }

        internal void updateSpeechList()
        {
            lstSpeechTypes.Items.Clear();
            foreach (string sa in speechClient.SpeechManager.SpeechDB.Keys)
            {
                lstSpeechTypes.Items.Add(sa);
            }
            if (speechClient.SpeechManager.SpeechDB.ContainsKey(selectedSpeechType))
            {
                for (int i = 0; i < lstSpeechTypes.Items.Count; i++)
                {
                    if (lstSpeechTypes.Items[i].ToString() == selectedSpeechType)
                    {
                        lstSpeechTypes.SelectedIndex = i;
                        break;
                    }
                }
            }
            else selectedSpeechType = "";
        }

        private void lstSpeechTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedSpeechType = lstSpeechTypes.SelectedItem.ToString();
            PopulateSpeechActs();
        }

        private void PopulateSpeechActs()
        {
            lstSpeech.Items.Clear();
            if (selectedSpeechType == "") return;
            else
            {
                SpeechAct selectedSpeech = speechClient.SpeechManager.SpeechDB[selectedSpeechType];
                foreach (string s in selectedSpeech.Speech)
                {
                    lstSpeech.Items.Add(s);
                }
                PopulateDataView(dgvTags, selectedSpeech);
            }
        }

        private void SetupControls()
        {
            dgvTags.ColumnCount = 3;
            dgvTags.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgvTags.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgvTags.GridColor = Color.Black;
            dgvTags.RowHeadersVisible = false;
            dgvTags.Columns[0].Name = "Tag";
            dgvTags.Columns[0].ReadOnly = true;
            dgvTags.Columns[1].Name = "ParameterName";
            dgvTags.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dgvTags.Columns[1].ReadOnly = true;
            dgvTags.Columns[2].Name = "Value";
            dgvTags.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvTags.MultiSelect = false;
            dgvTags.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            lstSpeech.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            lstSpeech.MeasureItem += lst_MeasureItem;
            lstSpeech.DrawItem += lst_DrawItem;
        }

        

        private void lst_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)e.Graphics.MeasureString(lstSpeech.Items[e.Index].ToString(), lstSpeech.Font, lstSpeech.Width).Height;
        }

        private void lst_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            e.Graphics.DrawString(lstSpeech.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
        }

        private void PopulateDataView(DataGridView dgv, SpeechAct speechAct)
        {
            dgv.Rows.Clear();
            foreach (SpeechTag t in speechAct.Tags)
            {
                string[] row = { t.Tag, t.ParameterName, "" };
                dgv.Rows.Add(row);
            }
        }

        private void frmSpeechManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            speechClient.Dispose();
        }

        private void btnCreateExample_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Speech Manager Database|*.smdb|All Files|*.*";
            saveFileDialog.InitialDirectory = Directory.GetCurrentDirectory();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SpeechDB.SaveSpeechDB(saveFileDialog.FileName, SpeechDB.CreateExampleFile());
            }
        }

        private void lstSpeech_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectedSpeechType=="") return;
            SpeechAct selectedSpeech = speechClient.SpeechManager.SpeechDB[selectedSpeechType];
        }

        private void btnRunSelected_Click(object sender, EventArgs e)
        {
            if (lstSpeech.SelectedItem != null)
            {
                string textBase = lstSpeech.SelectedItem.ToString();
                SpeechAct selectedSpeech = speechClient.SpeechManager.SpeechDB[selectedSpeechType];
                Dictionary<string, string> parameters = new Dictionary<string, string>();

                foreach (DataGridViewRow row in dgvTags.Rows)
                {
                    string paramName = row.Cells[1].Value == null ? "" : row.Cells[1].Value.ToString();
                    string value = row.Cells[2].Value == null ? "" : row.Cells[2].Value.ToString();
                    parameters[paramName] = value;
                }
                Console.WriteLine(speechClient.SpeechManager.BuildText(textBase, selectedSpeech, parameters));
            }
        }
    }
}
