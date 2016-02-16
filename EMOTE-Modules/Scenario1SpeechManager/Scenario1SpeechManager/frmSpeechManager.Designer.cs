namespace Scenario1SpeechManager
{
    partial class frmSpeechManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtSMF = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnLoadSMF = new System.Windows.Forms.Button();
            this.lstSpeechTypes = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lstSpeech = new System.Windows.Forms.ListBox();
            this.dgvTags = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.btnRunSelected = new System.Windows.Forms.Button();
            this.btnCreateExample = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTags)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSMF
            // 
            this.txtSMF.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSMF.Enabled = false;
            this.txtSMF.Location = new System.Drawing.Point(129, 6);
            this.txtSMF.Name = "txtSMF";
            this.txtSMF.Size = new System.Drawing.Size(293, 20);
            this.txtSMF.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Speech Manager File:";
            // 
            // btnLoadSMF
            // 
            this.btnLoadSMF.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadSMF.Location = new System.Drawing.Point(428, 3);
            this.btnLoadSMF.Name = "btnLoadSMF";
            this.btnLoadSMF.Size = new System.Drawing.Size(45, 23);
            this.btnLoadSMF.TabIndex = 2;
            this.btnLoadSMF.Text = "Load";
            this.btnLoadSMF.UseVisualStyleBackColor = true;
            this.btnLoadSMF.Click += new System.EventHandler(this.btnLoadSMF_Click);
            // 
            // lstSpeechTypes
            // 
            this.lstSpeechTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstSpeechTypes.FormattingEnabled = true;
            this.lstSpeechTypes.Location = new System.Drawing.Point(15, 63);
            this.lstSpeechTypes.Name = "lstSpeechTypes";
            this.lstSpeechTypes.Size = new System.Drawing.Size(148, 199);
            this.lstSpeechTypes.Sorted = true;
            this.lstSpeechTypes.TabIndex = 3;
            this.lstSpeechTypes.SelectedIndexChanged += new System.EventHandler(this.lstSpeechTypes_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Speech Types:";
            // 
            // lstSpeech
            // 
            this.lstSpeech.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSpeech.FormattingEnabled = true;
            this.lstSpeech.Location = new System.Drawing.Point(3, 23);
            this.lstSpeech.Name = "lstSpeech";
            this.lstSpeech.Size = new System.Drawing.Size(247, 134);
            this.lstSpeech.TabIndex = 5;
            this.lstSpeech.SelectedIndexChanged += new System.EventHandler(this.lstSpeech_SelectedIndexChanged);
            // 
            // dgvTags
            // 
            this.dgvTags.AllowUserToAddRows = false;
            this.dgvTags.AllowUserToDeleteRows = false;
            this.dgvTags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTags.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvTags.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTags.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnKeystroke;
            this.dgvTags.Location = new System.Drawing.Point(256, 23);
            this.dgvTags.MultiSelect = false;
            this.dgvTags.Name = "dgvTags";
            this.dgvTags.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvTags.Size = new System.Drawing.Size(144, 143);
            this.dgvTags.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(256, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(34, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Tags:";
            // 
            // btnRunSelected
            // 
            this.btnRunSelected.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRunSelected.Location = new System.Drawing.Point(256, 173);
            this.btnRunSelected.Name = "btnRunSelected";
            this.btnRunSelected.Size = new System.Drawing.Size(144, 23);
            this.btnRunSelected.TabIndex = 10;
            this.btnRunSelected.Text = "Run Selected";
            this.btnRunSelected.UseVisualStyleBackColor = true;
            this.btnRunSelected.Click += new System.EventHandler(this.btnRunSelected_Click);
            // 
            // btnCreateExample
            // 
            this.btnCreateExample.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateExample.Location = new System.Drawing.Point(485, 3);
            this.btnCreateExample.Name = "btnCreateExample";
            this.btnCreateExample.Size = new System.Drawing.Size(89, 23);
            this.btnCreateExample.TabIndex = 11;
            this.btnCreateExample.Text = "Create Example";
            this.btnCreateExample.UseVisualStyleBackColor = true;
            this.btnCreateExample.Click += new System.EventHandler(this.btnCreateExample_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 62.77916F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 37.22084F));
            this.tableLayoutPanel1.Controls.Add(this.btnRunSelected, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.dgvTags, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lstSpeech, 0, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(169, 63);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(403, 199);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // frmSpeechManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 270);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnCreateExample);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lstSpeechTypes);
            this.Controls.Add(this.btnLoadSMF);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSMF);
            this.Name = "frmSpeechManager";
            this.Text = "Scenario1 Speech Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSpeechManager_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTags)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSMF;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnLoadSMF;
        private System.Windows.Forms.ListBox lstSpeechTypes;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox lstSpeech;
        private System.Windows.Forms.DataGridView dgvTags;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRunSelected;
        private System.Windows.Forms.Button btnCreateExample;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

