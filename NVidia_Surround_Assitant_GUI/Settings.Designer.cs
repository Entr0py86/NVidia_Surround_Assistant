namespace NVidia_Surround_Assistant
{
    partial class Settings
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
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxSurroundToNormal_OnClose = new System.Windows.Forms.ComboBox();
            this.comboBoxSurroundToNormal_OnExit = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBoxSaveWindowPositions_Yes = new System.Windows.Forms.PictureBox();
            this.pictureBoxSaveWindowPositions_No = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.pictureBoxLoadConfig = new System.Windows.Forms.PictureBox();
            this.pictureBoxSaveConfig = new System.Windows.Forms.PictureBox();
            this.pictureBoxStartOnStartup_No = new System.Windows.Forms.PictureBox();
            this.pictureBoxStartOnStartup_Yes = new System.Windows.Forms.PictureBox();
            this.pictureBoxCloseToTray = new System.Windows.Forms.PictureBox();
            this.pictureBoxCloseOnClose = new System.Windows.Forms.PictureBox();
            this.pictureBoxCancel = new System.Windows.Forms.PictureBox();
            this.pictureBoxApply = new System.Windows.Forms.PictureBox();
            this.pictureBoxStartMax = new System.Windows.Forms.PictureBox();
            this.pictureBoxStartMin = new System.Windows.Forms.PictureBox();
            this.pictureBoxShowLogs_Yes = new System.Windows.Forms.PictureBox();
            this.pictureBoxShowLogs_No = new System.Windows.Forms.PictureBox();
            this.label9 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSaveWindowPositions_Yes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSaveWindowPositions_No)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoadConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSaveConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStartOnStartup_No)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStartOnStartup_Yes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCloseToTray)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCloseOnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxApply)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStartMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStartMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxShowLogs_Yes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxShowLogs_No)).BeginInit();
            this.SuspendLayout();
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label4.Location = new System.Drawing.Point(12, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(159, 15);
            this.label4.TabIndex = 52;
            this.label4.Text = "Start application Minimized:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label1.Location = new System.Drawing.Point(12, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 15);
            this.label1.TabIndex = 57;
            this.label1.Text = "Close to tray:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label2.Location = new System.Drawing.Point(14, 198);
            this.label2.MaximumSize = new System.Drawing.Size(159, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 45);
            this.label2.TabIndex = 60;
            this.label2.Text = "Switch back to normal when in surround mode when exiting NVSA:";
            // 
            // comboBoxSurroundToNormal_OnClose
            // 
            this.comboBoxSurroundToNormal_OnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.comboBoxSurroundToNormal_OnClose.FormattingEnabled = true;
            this.comboBoxSurroundToNormal_OnClose.Items.AddRange(new object[] {
            "Always",
            "Ask",
            "Never"});
            this.comboBoxSurroundToNormal_OnClose.Location = new System.Drawing.Point(181, 209);
            this.comboBoxSurroundToNormal_OnClose.Name = "comboBoxSurroundToNormal_OnClose";
            this.comboBoxSurroundToNormal_OnClose.Size = new System.Drawing.Size(121, 23);
            this.comboBoxSurroundToNormal_OnClose.TabIndex = 61;
            this.comboBoxSurroundToNormal_OnClose.SelectedIndexChanged += new System.EventHandler(this.comboBoxSurroundToNormal_OnClose_SelectedIndexChanged);
            // 
            // comboBoxSurroundToNormal_OnExit
            // 
            this.comboBoxSurroundToNormal_OnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.comboBoxSurroundToNormal_OnExit.FormattingEnabled = true;
            this.comboBoxSurroundToNormal_OnExit.Items.AddRange(new object[] {
            "Always",
            "Ask",
            "Never"});
            this.comboBoxSurroundToNormal_OnExit.Location = new System.Drawing.Point(181, 268);
            this.comboBoxSurroundToNormal_OnExit.Name = "comboBoxSurroundToNormal_OnExit";
            this.comboBoxSurroundToNormal_OnExit.Size = new System.Drawing.Size(121, 23);
            this.comboBoxSurroundToNormal_OnExit.TabIndex = 63;
            this.comboBoxSurroundToNormal_OnExit.SelectedIndexChanged += new System.EventHandler(this.comboBoxSurroundToNormal_OnExit_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label3.Location = new System.Drawing.Point(14, 257);
            this.label3.MaximumSize = new System.Drawing.Size(159, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 60);
            this.label3.TabIndex = 62;
            this.label3.Text = "Switch back to normal when in surround mode when exiting detected program:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label5.Location = new System.Drawing.Point(12, 108);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(134, 15);
            this.label5.TabIndex = 64;
            this.label5.Text = "Save window positions:";
            this.toolTip1.SetToolTip(this.label5, "An attempt will be made to save all window positions before the surround switch a" +
        "nd will be restored after switching back");
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // pictureBoxSaveWindowPositions_Yes
            // 
            this.pictureBoxSaveWindowPositions_Yes.Image = global::NVidia_Surround_Assistant.Properties.Resources.success_green_25x25;
            this.pictureBoxSaveWindowPositions_Yes.Location = new System.Drawing.Point(216, 105);
            this.pictureBoxSaveWindowPositions_Yes.Name = "pictureBoxSaveWindowPositions_Yes";
            this.pictureBoxSaveWindowPositions_Yes.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxSaveWindowPositions_Yes.TabIndex = 66;
            this.pictureBoxSaveWindowPositions_Yes.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxSaveWindowPositions_Yes, "An attempt will be made to save all window positions before the surround switch a" +
        "nd will be restored after switching back");
            this.pictureBoxSaveWindowPositions_Yes.Click += new System.EventHandler(this.pictureBoxSaveWindowPositions_Yes_Click);
            // 
            // pictureBoxSaveWindowPositions_No
            // 
            this.pictureBoxSaveWindowPositions_No.Image = global::NVidia_Surround_Assistant.Properties.Resources.success_red_25x25;
            this.pictureBoxSaveWindowPositions_No.Location = new System.Drawing.Point(216, 105);
            this.pictureBoxSaveWindowPositions_No.Name = "pictureBoxSaveWindowPositions_No";
            this.pictureBoxSaveWindowPositions_No.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxSaveWindowPositions_No.TabIndex = 65;
            this.pictureBoxSaveWindowPositions_No.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxSaveWindowPositions_No, "An attempt will be made to save all window positions before the surround switch a" +
        "nd will be restored after switching back");
            this.pictureBoxSaveWindowPositions_No.Click += new System.EventHandler(this.pictureBoxSaveWindowPositions_No_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label6.Location = new System.Drawing.Point(12, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(159, 15);
            this.label6.TabIndex = 67;
            this.label6.Text = "Start application on start-up:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label7.Location = new System.Drawing.Point(14, 325);
            this.label7.MaximumSize = new System.Drawing.Size(159, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 30);
            this.label7.TabIndex = 71;
            this.label7.Text = "Save current display configuration to file:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label8.Location = new System.Drawing.Point(14, 367);
            this.label8.MaximumSize = new System.Drawing.Size(159, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(153, 30);
            this.label8.TabIndex = 73;
            this.label8.Text = "Load display configuration from file:";
            // 
            // pictureBoxLoadConfig
            // 
            this.pictureBoxLoadConfig.Image = global::NVidia_Surround_Assistant.Properties.Resources.folder_25x25;
            this.pictureBoxLoadConfig.Location = new System.Drawing.Point(216, 372);
            this.pictureBoxLoadConfig.Name = "pictureBoxLoadConfig";
            this.pictureBoxLoadConfig.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxLoadConfig.TabIndex = 72;
            this.pictureBoxLoadConfig.TabStop = false;
            this.pictureBoxLoadConfig.Click += new System.EventHandler(this.pictureBoxLoadConfig_Click);
            // 
            // pictureBoxSaveConfig
            // 
            this.pictureBoxSaveConfig.Image = global::NVidia_Surround_Assistant.Properties.Resources.save_25x25;
            this.pictureBoxSaveConfig.Location = new System.Drawing.Point(216, 330);
            this.pictureBoxSaveConfig.Name = "pictureBoxSaveConfig";
            this.pictureBoxSaveConfig.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxSaveConfig.TabIndex = 70;
            this.pictureBoxSaveConfig.TabStop = false;
            this.pictureBoxSaveConfig.Click += new System.EventHandler(this.pictureBoxSaveConfig_Click);
            // 
            // pictureBoxStartOnStartup_No
            // 
            this.pictureBoxStartOnStartup_No.Image = global::NVidia_Surround_Assistant.Properties.Resources.success_red_25x25;
            this.pictureBoxStartOnStartup_No.Location = new System.Drawing.Point(216, 12);
            this.pictureBoxStartOnStartup_No.Name = "pictureBoxStartOnStartup_No";
            this.pictureBoxStartOnStartup_No.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxStartOnStartup_No.TabIndex = 69;
            this.pictureBoxStartOnStartup_No.TabStop = false;
            this.pictureBoxStartOnStartup_No.Click += new System.EventHandler(this.pictureBoxStartOnStartup_No_Click);
            // 
            // pictureBoxStartOnStartup_Yes
            // 
            this.pictureBoxStartOnStartup_Yes.Image = global::NVidia_Surround_Assistant.Properties.Resources.success_green_25x25;
            this.pictureBoxStartOnStartup_Yes.Location = new System.Drawing.Point(216, 12);
            this.pictureBoxStartOnStartup_Yes.Name = "pictureBoxStartOnStartup_Yes";
            this.pictureBoxStartOnStartup_Yes.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxStartOnStartup_Yes.TabIndex = 68;
            this.pictureBoxStartOnStartup_Yes.TabStop = false;
            this.pictureBoxStartOnStartup_Yes.Click += new System.EventHandler(this.pictureBoxStartOnStartup_Yes_Click);
            // 
            // pictureBoxCloseToTray
            // 
            this.pictureBoxCloseToTray.Image = global::NVidia_Surround_Assistant.Properties.Resources.success_green_25x25;
            this.pictureBoxCloseToTray.Location = new System.Drawing.Point(216, 74);
            this.pictureBoxCloseToTray.Name = "pictureBoxCloseToTray";
            this.pictureBoxCloseToTray.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxCloseToTray.TabIndex = 59;
            this.pictureBoxCloseToTray.TabStop = false;
            this.pictureBoxCloseToTray.Click += new System.EventHandler(this.pictureBoxCloseToTray_Click);
            // 
            // pictureBoxCloseOnClose
            // 
            this.pictureBoxCloseOnClose.Image = global::NVidia_Surround_Assistant.Properties.Resources.success_red_25x25;
            this.pictureBoxCloseOnClose.Location = new System.Drawing.Point(216, 74);
            this.pictureBoxCloseOnClose.Name = "pictureBoxCloseOnClose";
            this.pictureBoxCloseOnClose.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxCloseOnClose.TabIndex = 58;
            this.pictureBoxCloseOnClose.TabStop = false;
            this.pictureBoxCloseOnClose.Click += new System.EventHandler(this.pictureBoxCloseOnClose_Click);
            // 
            // pictureBoxCancel
            // 
            this.pictureBoxCancel.Image = global::NVidia_Surround_Assistant.Properties.Resources.delete_filled_red_25x25;
            this.pictureBoxCancel.Location = new System.Drawing.Point(179, 440);
            this.pictureBoxCancel.Name = "pictureBoxCancel";
            this.pictureBoxCancel.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxCancel.TabIndex = 56;
            this.pictureBoxCancel.TabStop = false;
            this.pictureBoxCancel.Click += new System.EventHandler(this.pictureBoxCancel_Click);
            // 
            // pictureBoxApply
            // 
            this.pictureBoxApply.Image = global::NVidia_Surround_Assistant.Properties.Resources.success_green_25x25;
            this.pictureBoxApply.Location = new System.Drawing.Point(104, 441);
            this.pictureBoxApply.Name = "pictureBoxApply";
            this.pictureBoxApply.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxApply.TabIndex = 55;
            this.pictureBoxApply.TabStop = false;
            this.pictureBoxApply.Click += new System.EventHandler(this.pictureBoxApply_Click);
            // 
            // pictureBoxStartMax
            // 
            this.pictureBoxStartMax.Image = global::NVidia_Surround_Assistant.Properties.Resources.success_red_25x25;
            this.pictureBoxStartMax.Location = new System.Drawing.Point(216, 43);
            this.pictureBoxStartMax.Name = "pictureBoxStartMax";
            this.pictureBoxStartMax.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxStartMax.TabIndex = 54;
            this.pictureBoxStartMax.TabStop = false;
            this.pictureBoxStartMax.Click += new System.EventHandler(this.pictureBoxStartMax_Click);
            // 
            // pictureBoxStartMin
            // 
            this.pictureBoxStartMin.Image = global::NVidia_Surround_Assistant.Properties.Resources.success_green_25x25;
            this.pictureBoxStartMin.Location = new System.Drawing.Point(216, 43);
            this.pictureBoxStartMin.Name = "pictureBoxStartMin";
            this.pictureBoxStartMin.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxStartMin.TabIndex = 53;
            this.pictureBoxStartMin.TabStop = false;
            this.pictureBoxStartMin.Click += new System.EventHandler(this.pictureBoxStartMin_Click);
            // 
            // pictureBoxShowLogs_Yes
            // 
            this.pictureBoxShowLogs_Yes.Image = global::NVidia_Surround_Assistant.Properties.Resources.success_green_25x25;
            this.pictureBoxShowLogs_Yes.Location = new System.Drawing.Point(216, 136);
            this.pictureBoxShowLogs_Yes.Name = "pictureBoxShowLogs_Yes";
            this.pictureBoxShowLogs_Yes.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxShowLogs_Yes.TabIndex = 76;
            this.pictureBoxShowLogs_Yes.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxShowLogs_Yes, "An attempt will be made to save all window positions before the surround switch a" +
        "nd will be restored after switching back");
            this.pictureBoxShowLogs_Yes.Click += new System.EventHandler(this.pictureBoxShowLogs_Yes_Click);
            // 
            // pictureBoxShowLogs_No
            // 
            this.pictureBoxShowLogs_No.Image = global::NVidia_Surround_Assistant.Properties.Resources.success_red_25x25;
            this.pictureBoxShowLogs_No.Location = new System.Drawing.Point(216, 136);
            this.pictureBoxShowLogs_No.Name = "pictureBoxShowLogs_No";
            this.pictureBoxShowLogs_No.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxShowLogs_No.TabIndex = 75;
            this.pictureBoxShowLogs_No.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxShowLogs_No, "An attempt will be made to save all window positions before the surround switch a" +
        "nd will be restored after switching back");
            this.pictureBoxShowLogs_No.Click += new System.EventHandler(this.pictureBoxShowLogs_No_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label9.Location = new System.Drawing.Point(12, 139);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(106, 15);
            this.label9.TabIndex = 74;
            this.label9.Text = "Show log window:";
            this.toolTip1.SetToolTip(this.label9, "An attempt will be made to save all window positions before the surround switch a" +
        "nd will be restored after switching back");
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(319, 488);
            this.Controls.Add(this.pictureBoxShowLogs_Yes);
            this.Controls.Add(this.pictureBoxShowLogs_No);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.pictureBoxLoadConfig);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pictureBoxSaveConfig);
            this.Controls.Add(this.pictureBoxStartOnStartup_No);
            this.Controls.Add(this.pictureBoxStartOnStartup_Yes);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.pictureBoxSaveWindowPositions_Yes);
            this.Controls.Add(this.pictureBoxSaveWindowPositions_No);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxSurroundToNormal_OnExit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxSurroundToNormal_OnClose);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBoxCloseToTray);
            this.Controls.Add(this.pictureBoxCloseOnClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBoxCancel);
            this.Controls.Add(this.pictureBoxApply);
            this.Controls.Add(this.pictureBoxStartMax);
            this.Controls.Add(this.pictureBoxStartMin);
            this.Controls.Add(this.label4);
            this.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Settings";
            this.Text = "Settings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Settings_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSaveWindowPositions_Yes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSaveWindowPositions_No)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoadConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSaveConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStartOnStartup_No)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStartOnStartup_Yes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCloseToTray)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCloseOnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxApply)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStartMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStartMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxShowLogs_Yes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxShowLogs_No)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxStartMax;
        private System.Windows.Forms.PictureBox pictureBoxStartMin;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBoxCancel;
        private System.Windows.Forms.PictureBox pictureBoxApply;
        private System.Windows.Forms.PictureBox pictureBoxCloseToTray;
        private System.Windows.Forms.PictureBox pictureBoxCloseOnClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxSurroundToNormal_OnClose;
        private System.Windows.Forms.ComboBox comboBoxSurroundToNormal_OnExit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.PictureBox pictureBoxSaveWindowPositions_Yes;
        private System.Windows.Forms.PictureBox pictureBoxSaveWindowPositions_No;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureBoxStartOnStartup_No;
        private System.Windows.Forms.PictureBox pictureBoxStartOnStartup_Yes;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBoxSaveConfig;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.PictureBox pictureBoxLoadConfig;
        private System.Windows.Forms.PictureBox pictureBoxShowLogs_Yes;
        private System.Windows.Forms.PictureBox pictureBoxShowLogs_No;
        private System.Windows.Forms.Label label9;
    }
}