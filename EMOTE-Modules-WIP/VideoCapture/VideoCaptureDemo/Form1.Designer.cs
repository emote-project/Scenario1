namespace VideoCaptureDemo
{
    partial class Form1
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Camera_Selection = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.Frame_lbl = new System.Windows.Forms.Label();
            this.modulenumber = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cntbutton = new System.Windows.Forms.Button();
            this.idtext = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.resolution = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.fpstext = new System.Windows.Forms.TextBox();
            this.realfps = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Camera_Selection);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Location = new System.Drawing.Point(4, 92);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(336, 64);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source";
            // 
            // Camera_Selection
            // 
            this.Camera_Selection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Camera_Selection.Enabled = false;
            this.Camera_Selection.FormattingEnabled = true;
            this.Camera_Selection.Location = new System.Drawing.Point(9, 23);
            this.Camera_Selection.Name = "Camera_Selection";
            this.Camera_Selection.Size = new System.Drawing.Size(240, 27);
            this.Camera_Selection.TabIndex = 26;
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(253, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 27);
            this.button1.TabIndex = 1;
            this.button1.Text = "Prepare";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pictureBox1);
            this.groupBox2.Location = new System.Drawing.Point(4, 153);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(336, 272);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Preview";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::VideoCaptureDemo.Properties.Resources.blank;
            this.pictureBox1.Location = new System.Drawing.Point(8, 24);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(320, 240);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Frame_lbl
            // 
            this.Frame_lbl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Frame_lbl.Location = new System.Drawing.Point(10, 429);
            this.Frame_lbl.Name = "Frame_lbl";
            this.Frame_lbl.Size = new System.Drawing.Size(322, 30);
            this.Frame_lbl.TabIndex = 3;
            this.Frame_lbl.Text = "Frame:";
            this.Frame_lbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // modulenumber
            // 
            this.modulenumber.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modulenumber.FormattingEnabled = true;
            this.modulenumber.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.modulenumber.Location = new System.Drawing.Point(265, 5);
            this.modulenumber.Name = "modulenumber";
            this.modulenumber.Size = new System.Drawing.Size(67, 27);
            this.modulenumber.TabIndex = 4;
            this.modulenumber.SelectedIndexChanged += new System.EventHandler(this.modulenumber_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 19);
            this.label1.TabIndex = 5;
            this.label1.Text = "Unique module number:";
            // 
            // cntbutton
            // 
            this.cntbutton.Enabled = false;
            this.cntbutton.Location = new System.Drawing.Point(13, 38);
            this.cntbutton.Name = "cntbutton";
            this.cntbutton.Size = new System.Drawing.Size(321, 30);
            this.cntbutton.TabIndex = 6;
            this.cntbutton.Text = "Connect to server";
            this.cntbutton.UseVisualStyleBackColor = true;
            this.cntbutton.Click += new System.EventHandler(this.cntbutton_Click);
            // 
            // idtext
            // 
            this.idtext.Enabled = false;
            this.idtext.Location = new System.Drawing.Point(234, 72);
            this.idtext.Name = "idtext";
            this.idtext.Size = new System.Drawing.Size(99, 27);
            this.idtext.TabIndex = 7;
            this.idtext.Text = "0";
            this.idtext.TextChanged += new System.EventHandler(this.idtext_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 19);
            this.label2.TabIndex = 8;
            this.label2.Text = "Participant ID:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 470);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 19);
            this.label3.TabIndex = 9;
            this.label3.Text = "Resolution";
            // 
            // resolution
            // 
            this.resolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resolution.Enabled = false;
            this.resolution.FormattingEnabled = true;
            this.resolution.Items.AddRange(new object[] {
            "Low Res",
            "High Res"});
            this.resolution.Location = new System.Drawing.Point(218, 462);
            this.resolution.Name = "resolution";
            this.resolution.Size = new System.Drawing.Size(113, 27);
            this.resolution.TabIndex = 10;
            this.resolution.SelectedIndexChanged += new System.EventHandler(this.resolution_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 496);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(135, 19);
            this.label4.TabIndex = 11;
            this.label4.Text = "Frames per second:";
            // 
            // fpstext
            // 
            this.fpstext.Enabled = false;
            this.fpstext.Location = new System.Drawing.Point(218, 493);
            this.fpstext.Name = "fpstext";
            this.fpstext.Size = new System.Drawing.Size(113, 27);
            this.fpstext.TabIndex = 12;
            this.fpstext.Text = "15";
            // 
            // realfps
            // 
            this.realfps.AutoSize = true;
            this.realfps.Location = new System.Drawing.Point(278, 435);
            this.realfps.Name = "realfps";
            this.realfps.Size = new System.Drawing.Size(17, 19);
            this.realfps.TabIndex = 13;
            this.realfps.Text = "0";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(204, 435);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 19);
            this.label5.TabIndex = 14;
            this.label5.Text = "Real FPS:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 527);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.realfps);
            this.Controls.Add(this.fpstext);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.resolution);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.idtext);
            this.Controls.Add(this.cntbutton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.modulenumber);
            this.Controls.Add(this.Frame_lbl);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VideoCapture";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox Camera_Selection;
        private System.Windows.Forms.Label Frame_lbl;
        private System.Windows.Forms.ComboBox modulenumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cntbutton;
        private System.Windows.Forms.TextBox idtext;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox resolution;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox fpstext;
        private System.Windows.Forms.Label realfps;
        private System.Windows.Forms.Label label5;
    }
}

