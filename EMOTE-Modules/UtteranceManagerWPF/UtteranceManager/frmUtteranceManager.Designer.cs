namespace UtteranceManager
{
    partial class frmUtteranceTester
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
            this.btnSend = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUtterance = new System.Windows.Forms.TextBox();
            this.lstUtterances = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lstCategories = new System.Windows.Forms.ListBox();
            this.lstSubcategories = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPrependTags = new System.Windows.Forms.TextBox();
            this.chkAutoLoadonStartup = new System.Windows.Forms.CheckBox();
            this.txtLoadedLibrary = new System.Windows.Forms.TextBox();
            this.btnLoadLibrary = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Location = new System.Drawing.Point(461, 490);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(140, 23);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "Perform";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(95, 495);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(69, 13);
            this.lblStatus.TabIndex = 7;
            this.lblStatus.Text = "connecting...";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 495);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Client status:";
            // 
            // txtUtterance
            // 
            this.txtUtterance.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUtterance.Location = new System.Drawing.Point(12, 316);
            this.txtUtterance.Multiline = true;
            this.txtUtterance.Name = "txtUtterance";
            this.txtUtterance.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtUtterance.Size = new System.Drawing.Size(680, 168);
            this.txtUtterance.TabIndex = 9;
            // 
            // lstUtterances
            // 
            this.lstUtterances.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstUtterances.FormattingEnabled = true;
            this.lstUtterances.Location = new System.Drawing.Point(269, 23);
            this.lstUtterances.Name = "lstUtterances";
            this.lstUtterances.Size = new System.Drawing.Size(393, 225);
            this.lstUtterances.TabIndex = 10;
            this.lstUtterances.SelectedIndexChanged += new System.EventHandler(this.lstUtterances_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Categories:";
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(680, 298);
            this.tabControl1.TabIndex = 12;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.tableLayoutPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(672, 272);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Library";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tableLayoutPanel1.Controls.Add(this.lstCategories, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lstSubcategories, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lstUtterances, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label4, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 6);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(665, 260);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // lstCategories
            // 
            this.lstCategories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstCategories.FormattingEnabled = true;
            this.lstCategories.Location = new System.Drawing.Point(3, 23);
            this.lstCategories.Name = "lstCategories";
            this.lstCategories.Size = new System.Drawing.Size(127, 225);
            this.lstCategories.TabIndex = 13;
            this.lstCategories.SelectedIndexChanged += new System.EventHandler(this.lstCategories_SelectedIndexChanged);
            // 
            // lstSubcategories
            // 
            this.lstSubcategories.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSubcategories.FormattingEnabled = true;
            this.lstSubcategories.Location = new System.Drawing.Point(136, 23);
            this.lstSubcategories.Name = "lstSubcategories";
            this.lstSubcategories.Size = new System.Drawing.Size(127, 225);
            this.lstSubcategories.TabIndex = 12;
            this.lstSubcategories.SelectedIndexChanged += new System.EventHandler(this.lstSubcategories_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(136, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(78, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Subcategories:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(269, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 15;
            this.label4.Text = "Utterances:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.txtPrependTags);
            this.tabPage2.Controls.Add(this.chkAutoLoadonStartup);
            this.tabPage2.Controls.Add(this.txtLoadedLibrary);
            this.tabPage2.Controls.Add(this.btnLoadLibrary);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(672, 272);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Prepend tags:";
            // 
            // txtPrependTags
            // 
            this.txtPrependTags.Location = new System.Drawing.Point(92, 59);
            this.txtPrependTags.Name = "txtPrependTags";
            this.txtPrependTags.Size = new System.Drawing.Size(574, 20);
            this.txtPrependTags.TabIndex = 5;
            this.txtPrependTags.TextChanged += new System.EventHandler(this.txtPrependTags_TextChanged);
            // 
            // chkAutoLoadonStartup
            // 
            this.chkAutoLoadonStartup.AutoSize = true;
            this.chkAutoLoadonStartup.Location = new System.Drawing.Point(92, 34);
            this.chkAutoLoadonStartup.Name = "chkAutoLoadonStartup";
            this.chkAutoLoadonStartup.Size = new System.Drawing.Size(124, 17);
            this.chkAutoLoadonStartup.TabIndex = 4;
            this.chkAutoLoadonStartup.Text = "Auto load on start-up";
            this.chkAutoLoadonStartup.UseVisualStyleBackColor = true;
            this.chkAutoLoadonStartup.CheckedChanged += new System.EventHandler(this.chkAutoLoadonStartup_CheckedChanged);
            // 
            // txtLoadedLibrary
            // 
            this.txtLoadedLibrary.Location = new System.Drawing.Point(92, 8);
            this.txtLoadedLibrary.Name = "txtLoadedLibrary";
            this.txtLoadedLibrary.ReadOnly = true;
            this.txtLoadedLibrary.Size = new System.Drawing.Size(493, 20);
            this.txtLoadedLibrary.TabIndex = 3;
            this.txtLoadedLibrary.TextChanged += new System.EventHandler(this.txtLoadedLibrary_TextChanged);
            // 
            // btnLoadLibrary
            // 
            this.btnLoadLibrary.Location = new System.Drawing.Point(591, 6);
            this.btnLoadLibrary.Name = "btnLoadLibrary";
            this.btnLoadLibrary.Size = new System.Drawing.Size(75, 23);
            this.btnLoadLibrary.TabIndex = 2;
            this.btnLoadLibrary.Text = "Load Library";
            this.btnLoadLibrary.UseVisualStyleBackColor = true;
            this.btnLoadLibrary.Click += new System.EventHandler(this.btnLoadLibrary_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 11);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Loaded Library:";
            this.label5.Click += new System.EventHandler(this.label5_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(623, 490);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(69, 23);
            this.btnClose.TabIndex = 13;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmUtteranceTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 516);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.txtUtterance);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSend);
            this.Name = "frmUtteranceTester";
            this.Text = "EMOTE Utterance Manager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmUtteranceTester_FormClosing);
            this.Load += new System.EventHandler(this.frmUtteranceTester_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUtterance;
        private System.Windows.Forms.ListBox lstUtterances;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox lstCategories;
        private System.Windows.Forms.ListBox lstSubcategories;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLoadedLibrary;
        private System.Windows.Forms.Button btnLoadLibrary;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkAutoLoadonStartup;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPrependTags;
    }
}

