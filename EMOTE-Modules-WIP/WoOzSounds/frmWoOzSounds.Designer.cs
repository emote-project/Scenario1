namespace WoOzSounds
{
    partial class frmWoOzSounds
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
            this.lstCategories = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lstFiles = new System.Windows.Forms.ListBox();
            this.btnPlayRandom = new System.Windows.Forms.Button();
            this.btnStopSound = new System.Windows.Forms.Button();
            this.txtNaoAddress = new System.Windows.Forms.TextBox();
            this.btnConnection = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstCategories
            // 
            this.lstCategories.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstCategories.FormattingEnabled = true;
            this.lstCategories.Location = new System.Drawing.Point(12, 28);
            this.lstCategories.Name = "lstCategories";
            this.lstCategories.Size = new System.Drawing.Size(169, 212);
            this.lstCategories.TabIndex = 0;
            this.lstCategories.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lstCategories_MouseClick);
            this.lstCategories.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyPressedHandler);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Categories:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 277);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Status:";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(56, 277);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(73, 13);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Disconnected";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(184, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Files:";
            // 
            // lstFiles
            // 
            this.lstFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFiles.FormattingEnabled = true;
            this.lstFiles.Location = new System.Drawing.Point(187, 28);
            this.lstFiles.Name = "lstFiles";
            this.lstFiles.Size = new System.Drawing.Size(174, 238);
            this.lstFiles.TabIndex = 4;
            this.lstFiles.DoubleClick += new System.EventHandler(this.lstFiles_DoubleClick);
            this.lstFiles.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyPressedHandler);
            // 
            // btnPlayRandom
            // 
            this.btnPlayRandom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlayRandom.Location = new System.Drawing.Point(273, 272);
            this.btnPlayRandom.Name = "btnPlayRandom";
            this.btnPlayRandom.Size = new System.Drawing.Size(87, 23);
            this.btnPlayRandom.TabIndex = 6;
            this.btnPlayRandom.Text = "Play Random";
            this.btnPlayRandom.UseVisualStyleBackColor = true;
            this.btnPlayRandom.Click += new System.EventHandler(this.btnPlayRandom_Click);
            this.btnPlayRandom.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyPressedHandler);
            // 
            // btnStopSound
            // 
            this.btnStopSound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStopSound.Enabled = false;
            this.btnStopSound.Location = new System.Drawing.Point(180, 272);
            this.btnStopSound.Name = "btnStopSound";
            this.btnStopSound.Size = new System.Drawing.Size(87, 23);
            this.btnStopSound.TabIndex = 7;
            this.btnStopSound.Text = "Stop Sound";
            this.btnStopSound.UseVisualStyleBackColor = true;
            this.btnStopSound.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyPressedHandler);
            // 
            // txtNaoAddress
            // 
            this.txtNaoAddress.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::WoOzSounds.Properties.Settings.Default, "RobotAddress", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.txtNaoAddress.Location = new System.Drawing.Point(15, 246);
            this.txtNaoAddress.Name = "txtNaoAddress";
            this.txtNaoAddress.Size = new System.Drawing.Size(91, 20);
            this.txtNaoAddress.TabIndex = 8;
            this.txtNaoAddress.Text = global::WoOzSounds.Properties.Settings.Default.RobotAddress;
            // 
            // btnConnection
            // 
            this.btnConnection.Location = new System.Drawing.Point(112, 244);
            this.btnConnection.Name = "btnConnection";
            this.btnConnection.Size = new System.Drawing.Size(69, 23);
            this.btnConnection.TabIndex = 9;
            this.btnConnection.Text = "Connect";
            this.btnConnection.UseVisualStyleBackColor = true;
            this.btnConnection.Click += new System.EventHandler(this.btnConnection_Click);
            this.btnConnection.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyPressedHandler);
            // 
            // frmWoOzSounds
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 300);
            this.Controls.Add(this.btnConnection);
            this.Controls.Add(this.txtNaoAddress);
            this.Controls.Add(this.btnStopSound);
            this.Controls.Add(this.btnPlayRandom);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lstFiles);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lstCategories);
            this.Name = "frmWoOzSounds";
            this.Text = "EMOTE Wizard-of-Oz Sound Player";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmWoOzSounds_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownHandler);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.KeyPressedHandler);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstCategories;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox lstFiles;
        private System.Windows.Forms.Button btnPlayRandom;
        private System.Windows.Forms.Button btnStopSound;
        private System.Windows.Forms.TextBox txtNaoAddress;
        private System.Windows.Forms.Button btnConnection;
    }
}