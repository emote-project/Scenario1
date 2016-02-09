namespace Plunger
{
    partial class frmPlunger
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPlunger));
            this.tmrMonitor = new System.Windows.Forms.Timer(this.components);
            this.radSkipTurn = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.radUtteranceFinished = new System.Windows.Forms.RadioButton();
            this.btnAction = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.lblSpeechStatus = new System.Windows.Forms.Label();
            this.lblLastSpeech = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblEnercitiesStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvMonitor = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.btnRefreshMonitoredMessagesList = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSetMonitoredMessages = new System.Windows.Forms.Button();
            this.lstEventNameFilters = new System.Windows.Forms.CheckedListBox();
            this.Message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LastReceived = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Details = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtUtteranceFinishedId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblThalamusStatus = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonitor)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tmrMonitor
            // 
            this.tmrMonitor.Interval = 1000;
            this.tmrMonitor.Tick += new System.EventHandler(this.tmrMonitor_Tick);
            // 
            // radSkipTurn
            // 
            this.radSkipTurn.Appearance = System.Windows.Forms.Appearance.Button;
            this.radSkipTurn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radSkipTurn.Location = new System.Drawing.Point(6, 19);
            this.radSkipTurn.Name = "radSkipTurn";
            this.radSkipTurn.Size = new System.Drawing.Size(200, 36);
            this.radSkipTurn.TabIndex = 1;
            this.radSkipTurn.TabStop = true;
            this.radSkipTurn.Text = "Skip Turn";
            this.radSkipTurn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radSkipTurn.UseVisualStyleBackColor = true;
            this.radSkipTurn.CheckedChanged += new System.EventHandler(this.radSkipTurn_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.txtUtteranceFinishedId);
            this.groupBox2.Controls.Add(this.radUtteranceFinished);
            this.groupBox2.Controls.Add(this.btnAction);
            this.groupBox2.Controls.Add(this.radSkipTurn);
            this.groupBox2.Location = new System.Drawing.Point(548, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(212, 338);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Action";
            // 
            // radUtteranceFinished
            // 
            this.radUtteranceFinished.Appearance = System.Windows.Forms.Appearance.Button;
            this.radUtteranceFinished.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radUtteranceFinished.Location = new System.Drawing.Point(6, 61);
            this.radUtteranceFinished.Name = "radUtteranceFinished";
            this.radUtteranceFinished.Size = new System.Drawing.Size(200, 30);
            this.radUtteranceFinished.TabIndex = 3;
            this.radUtteranceFinished.TabStop = true;
            this.radUtteranceFinished.Text = "Utterance Finished";
            this.radUtteranceFinished.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.radUtteranceFinished.UseVisualStyleBackColor = true;
            this.radUtteranceFinished.CheckedChanged += new System.EventHandler(this.radNothing_CheckedChanged);
            // 
            // btnAction
            // 
            this.btnAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAction.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnAction.BackgroundImage")));
            this.btnAction.Enabled = false;
            this.btnAction.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAction.Location = new System.Drawing.Point(6, 137);
            this.btnAction.Name = "btnAction";
            this.btnAction.Size = new System.Drawing.Size(200, 197);
            this.btnAction.TabIndex = 2;
            this.btnAction.UseVisualStyleBackColor = true;
            this.btnAction.Click += new System.EventHandler(this.btnAction_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(5, 25);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(537, 325);
            this.tabControl1.TabIndex = 3;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.lblSpeechStatus);
            this.tabPage1.Controls.Add(this.lblLastSpeech);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.lblEnercitiesStatus);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.dgvMonitor);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(529, 299);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Monitoring";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(307, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Speech Status:";
            // 
            // lblSpeechStatus
            // 
            this.lblSpeechStatus.AutoSize = true;
            this.lblSpeechStatus.Location = new System.Drawing.Point(393, 7);
            this.lblSpeechStatus.Name = "lblSpeechStatus";
            this.lblSpeechStatus.Size = new System.Drawing.Size(52, 13);
            this.lblSpeechStatus.TabIndex = 6;
            this.lblSpeechStatus.Text = "[Standby]";
            this.lblSpeechStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblLastSpeech
            // 
            this.lblLastSpeech.AutoSize = true;
            this.lblLastSpeech.Location = new System.Drawing.Point(104, 20);
            this.lblLastSpeech.Name = "lblLastSpeech";
            this.lblLastSpeech.Size = new System.Drawing.Size(24, 13);
            this.lblLastSpeech.TabIndex = 5;
            this.lblLastSpeech.Text = "n/a";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Last Speech:";
            // 
            // lblEnercitiesStatus
            // 
            this.lblEnercitiesStatus.AutoSize = true;
            this.lblEnercitiesStatus.Location = new System.Drawing.Point(104, 7);
            this.lblEnercitiesStatus.Name = "lblEnercitiesStatus";
            this.lblEnercitiesStatus.Size = new System.Drawing.Size(43, 13);
            this.lblEnercitiesStatus.TabIndex = 3;
            this.lblEnercitiesStatus.Text = "Waiting";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Enercities Status:";
            // 
            // dgvMonitor
            // 
            this.dgvMonitor.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMonitor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvMonitor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMonitor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Message,
            this.LastReceived,
            this.Details});
            this.dgvMonitor.Location = new System.Drawing.Point(9, 56);
            this.dgvMonitor.Name = "dgvMonitor";
            this.dgvMonitor.Size = new System.Drawing.Size(514, 237);
            this.dgvMonitor.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Controls.Add(this.btnRefreshMonitoredMessagesList);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.btnSetMonitoredMessages);
            this.tabPage2.Controls.Add(this.lstEventNameFilters);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(529, 312);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            this.tabPage2.Click += new System.EventHandler(this.tabPage2_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.Location = new System.Drawing.Point(193, 285);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(95, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "Just Save";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnRefreshMonitoredMessagesList
            // 
            this.btnRefreshMonitoredMessagesList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefreshMonitoredMessagesList.Location = new System.Drawing.Point(9, 285);
            this.btnRefreshMonitoredMessagesList.Name = "btnRefreshMonitoredMessagesList";
            this.btnRefreshMonitoredMessagesList.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshMonitoredMessagesList.TabIndex = 13;
            this.btnRefreshMonitoredMessagesList.Text = "Refresh";
            this.btnRefreshMonitoredMessagesList.UseVisualStyleBackColor = true;
            this.btnRefreshMonitoredMessagesList.Click += new System.EventHandler(this.btnRefreshMonitoredMessagesList_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Monitored messages:";
            // 
            // btnSetMonitoredMessages
            // 
            this.btnSetMonitoredMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSetMonitoredMessages.Location = new System.Drawing.Point(90, 285);
            this.btnSetMonitoredMessages.Name = "btnSetMonitoredMessages";
            this.btnSetMonitoredMessages.Size = new System.Drawing.Size(97, 23);
            this.btnSetMonitoredMessages.TabIndex = 11;
            this.btnSetMonitoredMessages.Text = "Set and Save";
            this.btnSetMonitoredMessages.UseVisualStyleBackColor = true;
            this.btnSetMonitoredMessages.Click += new System.EventHandler(this.btnSetMonitoredMessages_Click);
            // 
            // lstEventNameFilters
            // 
            this.lstEventNameFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstEventNameFilters.CheckOnClick = true;
            this.lstEventNameFilters.FormattingEnabled = true;
            this.lstEventNameFilters.Location = new System.Drawing.Point(6, 36);
            this.lstEventNameFilters.Name = "lstEventNameFilters";
            this.lstEventNameFilters.Size = new System.Drawing.Size(517, 214);
            this.lstEventNameFilters.TabIndex = 10;
            this.lstEventNameFilters.SelectedIndexChanged += new System.EventHandler(this.lstEventNameFilters_SelectedIndexChanged);
            // 
            // Message
            // 
            this.Message.HeaderText = "Message";
            this.Message.Name = "Message";
            this.Message.ReadOnly = true;
            this.Message.Width = 75;
            // 
            // LastReceived
            // 
            this.LastReceived.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.LastReceived.FillWeight = 70F;
            this.LastReceived.HeaderText = "Received (seconds ago)";
            this.LastReceived.Name = "LastReceived";
            this.LastReceived.ReadOnly = true;
            // 
            // Details
            // 
            this.Details.HeaderText = "Details";
            this.Details.Name = "Details";
            this.Details.Width = 64;
            // 
            // txtUtteranceFinishedId
            // 
            this.txtUtteranceFinishedId.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUtteranceFinishedId.Location = new System.Drawing.Point(7, 98);
            this.txtUtteranceFinishedId.Name = "txtUtteranceFinishedId";
            this.txtUtteranceFinishedId.Size = new System.Drawing.Size(199, 20);
            this.txtUtteranceFinishedId.TabIndex = 4;
            this.txtUtteranceFinishedId.Text = "<UtteranceFinished ID>";
            this.txtUtteranceFinishedId.TextChanged += new System.EventHandler(this.txtUtteranceFinishedId_TextChanged);
            this.txtUtteranceFinishedId.Enter += new System.EventHandler(this.txtUtteranceFinishedId_Enter);
            this.txtUtteranceFinishedId.Leave += new System.EventHandler(this.txtUtteranceFinishedId_Leave);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Thalamus Status:";
            // 
            // lblThalamusStatus
            // 
            this.lblThalamusStatus.AutoSize = true;
            this.lblThalamusStatus.Location = new System.Drawing.Point(101, 9);
            this.lblThalamusStatus.Name = "lblThalamusStatus";
            this.lblThalamusStatus.Size = new System.Drawing.Size(73, 13);
            this.lblThalamusStatus.TabIndex = 5;
            this.lblThalamusStatus.Text = "Disconnected";
            // 
            // frmPlunger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(768, 362);
            this.Controls.Add(this.lblThalamusStatus);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.groupBox2);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(541, 389);
            this.Name = "frmPlunger";
            this.Text = "Desentupidor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPlunger_FormClosing);
            this.Load += new System.EventHandler(this.frmPlunger_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMonitor)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmrMonitor;
        private System.Windows.Forms.RadioButton radSkipTurn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnAction;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dgvMonitor;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSetMonitoredMessages;
        private System.Windows.Forms.CheckedListBox lstEventNameFilters;
        private System.Windows.Forms.Button btnRefreshMonitoredMessagesList;
        private System.Windows.Forms.RadioButton radUtteranceFinished;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblEnercitiesStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSpeechStatus;
        private System.Windows.Forms.Label lblLastSpeech;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUtteranceFinishedId;
        private System.Windows.Forms.DataGridViewTextBoxColumn Message;
        private System.Windows.Forms.DataGridViewTextBoxColumn LastReceived;
        private System.Windows.Forms.DataGridViewTextBoxColumn Details;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblThalamusStatus;
    }
}

