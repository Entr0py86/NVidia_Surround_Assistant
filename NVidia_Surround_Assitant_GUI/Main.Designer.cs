namespace NVidia_Surround_Assistant
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.processCreatedCatcher = new System.ComponentModel.BackgroundWorker();
            this.saveSurroundFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.systemTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip_SystemTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_AddApp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem_ToggelSurround = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_LoadSurroundFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_SaveSurroundFile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem_Quit = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxLogs = new System.Windows.Forms.RichTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBoxSwitchSurround = new System.Windows.Forms.PictureBox();
            this.pictureBoxClose = new System.Windows.Forms.PictureBox();
            this.pictureBoxLoadConfig = new System.Windows.Forms.PictureBox();
            this.pictureBoxSaveConfig = new System.Windows.Forms.PictureBox();
            this.pictureBoxLogs = new System.Windows.Forms.PictureBox();
            this.pictureBoxSettings = new System.Windows.Forms.PictureBox();
            this.pictureBoxAddGame = new System.Windows.Forms.PictureBox();
            this.thumbGridView = new NVidia_Surround_Assistant.ThumbGridView();
            this.contextMenuStrip_SystemTray.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSwitchSurround)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoadConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSaveConfig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAddGame)).BeginInit();
            this.SuspendLayout();
            // 
            // processCreatedCatcher
            // 
            this.processCreatedCatcher.WorkerReportsProgress = true;
            this.processCreatedCatcher.WorkerSupportsCancellation = true;
            this.processCreatedCatcher.DoWork += new System.ComponentModel.DoWorkEventHandler(this.processCreatedCatcher_DoWork);
            this.processCreatedCatcher.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.processCreatedCatcher_ProgressChanged);
            // 
            // saveSurroundFileDialog
            // 
            this.saveSurroundFileDialog.Filter = "Surround Setup files (*.nvsa)|*.nvsa|All files (*.*)|*.*";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Surround Setup files (*.nvsa)|*.nvsa|Executable (*.exe)|*.exe|All files (*.*)|*.*" +
    "";
            // 
            // systemTrayIcon
            // 
            this.systemTrayIcon.ContextMenuStrip = this.contextMenuStrip_SystemTray;
            this.systemTrayIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("systemTrayIcon.Icon")));
            this.systemTrayIcon.Text = "NVidia Surround Assistant";
            this.systemTrayIcon.Visible = true;
            this.systemTrayIcon.DoubleClick += new System.EventHandler(this.SystemTrayIcon_DoubleClick);
            this.systemTrayIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SystemTrayIcon_MouseDoubleClick);
            // 
            // contextMenuStrip_SystemTray
            // 
            this.contextMenuStrip_SystemTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_AddApp,
            this.toolStripSeparator2,
            this.toolStripMenuItem_ToggelSurround,
            this.toolStripMenuItem_LoadSurroundFile,
            this.toolStripMenuItem_SaveSurroundFile,
            this.toolStripSeparator1,
            this.toolStripMenuItem_Quit});
            this.contextMenuStrip_SystemTray.Name = "contextMenuStrip_SystemTray";
            this.contextMenuStrip_SystemTray.Size = new System.Drawing.Size(212, 126);
            this.contextMenuStrip_SystemTray.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_SystemTray_Opening);
            // 
            // toolStripMenuItem_AddApp
            // 
            this.toolStripMenuItem_AddApp.Name = "toolStripMenuItem_AddApp";
            this.toolStripMenuItem_AddApp.Size = new System.Drawing.Size(211, 22);
            this.toolStripMenuItem_AddApp.Text = "Add Application";
            this.toolStripMenuItem_AddApp.Click += new System.EventHandler(this.toolStripMenuItem_AddApp_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(208, 6);
            // 
            // toolStripMenuItem_ToggelSurround
            // 
            this.toolStripMenuItem_ToggelSurround.Name = "toolStripMenuItem_ToggelSurround";
            this.toolStripMenuItem_ToggelSurround.Size = new System.Drawing.Size(211, 22);
            this.toolStripMenuItem_ToggelSurround.Text = "Switch To Surround Mode";
            this.toolStripMenuItem_ToggelSurround.Click += new System.EventHandler(this.toolStripMenuItem_ToggelSurround_Click);
            // 
            // toolStripMenuItem_LoadSurroundFile
            // 
            this.toolStripMenuItem_LoadSurroundFile.Name = "toolStripMenuItem_LoadSurroundFile";
            this.toolStripMenuItem_LoadSurroundFile.Size = new System.Drawing.Size(211, 22);
            this.toolStripMenuItem_LoadSurroundFile.Text = "Apply Surround File";
            this.toolStripMenuItem_LoadSurroundFile.Click += new System.EventHandler(this.toolStripMenuItem_LoadSurroundFile_Click);
            // 
            // toolStripMenuItem_SaveSurroundFile
            // 
            this.toolStripMenuItem_SaveSurroundFile.Name = "toolStripMenuItem_SaveSurroundFile";
            this.toolStripMenuItem_SaveSurroundFile.Size = new System.Drawing.Size(211, 22);
            this.toolStripMenuItem_SaveSurroundFile.Text = "Save Surround File";
            this.toolStripMenuItem_SaveSurroundFile.Click += new System.EventHandler(this.toolStripMenuItem_SaveSurroundFile_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(208, 6);
            // 
            // toolStripMenuItem_Quit
            // 
            this.toolStripMenuItem_Quit.Name = "toolStripMenuItem_Quit";
            this.toolStripMenuItem_Quit.Size = new System.Drawing.Size(211, 22);
            this.toolStripMenuItem_Quit.Text = "Quit";
            this.toolStripMenuItem_Quit.Click += new System.EventHandler(this.toolStripMenuItem_Quit_Click);
            // 
            // textBoxLogs
            // 
            this.textBoxLogs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBoxLogs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLogs.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxLogs.Location = new System.Drawing.Point(12, 541);
            this.textBoxLogs.Name = "textBoxLogs";
            this.textBoxLogs.Size = new System.Drawing.Size(1056, 141);
            this.textBoxLogs.TabIndex = 46;
            this.textBoxLogs.Text = "";
            this.textBoxLogs.VisibleChanged += new System.EventHandler(this.textBoxLogs_VisibleChanged);
            // 
            // pictureBoxSwitchSurround
            // 
            this.pictureBoxSwitchSurround.Image = global::NVidia_Surround_Assistant.Properties.Resources.help_50x50;
            this.pictureBoxSwitchSurround.Location = new System.Drawing.Point(180, 12);
            this.pictureBoxSwitchSurround.Name = "pictureBoxSwitchSurround";
            this.pictureBoxSwitchSurround.Size = new System.Drawing.Size(50, 50);
            this.pictureBoxSwitchSurround.TabIndex = 76;
            this.pictureBoxSwitchSurround.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxSwitchSurround, "Switch Surround Mode");
            this.pictureBoxSwitchSurround.Click += new System.EventHandler(this.pictureBoxSwitchSurround_Click);
            // 
            // pictureBoxClose
            // 
            this.pictureBoxClose.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxClose.Image = global::NVidia_Surround_Assistant.Properties.Resources.close_50x50;
            this.pictureBoxClose.Location = new System.Drawing.Point(1022, 12);
            this.pictureBoxClose.MaximumSize = new System.Drawing.Size(50, 50);
            this.pictureBoxClose.MinimumSize = new System.Drawing.Size(50, 50);
            this.pictureBoxClose.Name = "pictureBoxClose";
            this.pictureBoxClose.Size = new System.Drawing.Size(50, 50);
            this.pictureBoxClose.TabIndex = 75;
            this.pictureBoxClose.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxClose, "Exit");
            this.pictureBoxClose.Click += new System.EventHandler(this.pictureBoxClose_Click);
            // 
            // pictureBoxLoadConfig
            // 
            this.pictureBoxLoadConfig.Image = global::NVidia_Surround_Assistant.Properties.Resources.folder_50x50;
            this.pictureBoxLoadConfig.Location = new System.Drawing.Point(68, 12);
            this.pictureBoxLoadConfig.Name = "pictureBoxLoadConfig";
            this.pictureBoxLoadConfig.Size = new System.Drawing.Size(50, 50);
            this.pictureBoxLoadConfig.TabIndex = 74;
            this.pictureBoxLoadConfig.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxLoadConfig, "Load Display Setup");
            this.pictureBoxLoadConfig.Click += new System.EventHandler(this.pictureBoxLoadConfig_Click);
            // 
            // pictureBoxSaveConfig
            // 
            this.pictureBoxSaveConfig.Image = global::NVidia_Surround_Assistant.Properties.Resources.save_50x50;
            this.pictureBoxSaveConfig.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxSaveConfig.Name = "pictureBoxSaveConfig";
            this.pictureBoxSaveConfig.Size = new System.Drawing.Size(50, 50);
            this.pictureBoxSaveConfig.TabIndex = 73;
            this.pictureBoxSaveConfig.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxSaveConfig, "Save Display Setup");
            this.pictureBoxSaveConfig.Click += new System.EventHandler(this.pictureBoxSaveConfig_Click);
            // 
            // pictureBoxLogs
            // 
            this.pictureBoxLogs.Image = global::NVidia_Surround_Assistant.Properties.Resources.document_50x50;
            this.pictureBoxLogs.Location = new System.Drawing.Point(292, 12);
            this.pictureBoxLogs.Name = "pictureBoxLogs";
            this.pictureBoxLogs.Size = new System.Drawing.Size(50, 50);
            this.pictureBoxLogs.TabIndex = 47;
            this.pictureBoxLogs.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxLogs, "Show Log");
            this.pictureBoxLogs.Click += new System.EventHandler(this.pictureBoxLogs_Click);
            // 
            // pictureBoxSettings
            // 
            this.pictureBoxSettings.Image = global::NVidia_Surround_Assistant.Properties.Resources.settings_50x50;
            this.pictureBoxSettings.Location = new System.Drawing.Point(236, 12);
            this.pictureBoxSettings.Name = "pictureBoxSettings";
            this.pictureBoxSettings.Size = new System.Drawing.Size(50, 50);
            this.pictureBoxSettings.TabIndex = 1;
            this.pictureBoxSettings.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxSettings, "Settings");
            this.pictureBoxSettings.Click += new System.EventHandler(this.pictureBoxSettings_Click);
            // 
            // pictureBoxAddGame
            // 
            this.pictureBoxAddGame.Image = global::NVidia_Surround_Assistant.Properties.Resources.add_50x50;
            this.pictureBoxAddGame.Location = new System.Drawing.Point(124, 12);
            this.pictureBoxAddGame.Name = "pictureBoxAddGame";
            this.pictureBoxAddGame.Size = new System.Drawing.Size(50, 50);
            this.pictureBoxAddGame.TabIndex = 0;
            this.pictureBoxAddGame.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxAddGame, "Add Application");
            this.pictureBoxAddGame.Click += new System.EventHandler(this.PictureBoxAddGame_Click);
            // 
            // thumbGridView
            // 
            this.thumbGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.thumbGridView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.thumbGridView.Location = new System.Drawing.Point(12, 105);
            this.thumbGridView.MinimumSize = new System.Drawing.Size(260, 414);
            this.thumbGridView.Name = "thumbGridView";
            this.thumbGridView.Size = new System.Drawing.Size(1056, 417);
            this.thumbGridView.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(1084, 687);
            this.Controls.Add(this.pictureBoxSwitchSurround);
            this.Controls.Add(this.pictureBoxClose);
            this.Controls.Add(this.pictureBoxLoadConfig);
            this.Controls.Add(this.pictureBoxSaveConfig);
            this.Controls.Add(this.pictureBoxLogs);
            this.Controls.Add(this.textBoxLogs);
            this.Controls.Add(this.thumbGridView);
            this.Controls.Add(this.pictureBoxSettings);
            this.Controls.Add(this.pictureBoxAddGame);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "NVidia Surround Assistant";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.MainForm_Layout);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.contextMenuStrip_SystemTray.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSwitchSurround)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoadConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSaveConfig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLogs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAddGame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker processCreatedCatcher;
        private System.Windows.Forms.SaveFileDialog saveSurroundFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.PictureBox pictureBoxAddGame;
        private System.Windows.Forms.PictureBox pictureBoxSettings;
        private System.Windows.Forms.NotifyIcon systemTrayIcon;
        private ThumbGridView thumbGridView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip_SystemTray;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_LoadSurroundFile;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_SaveSurroundFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_Quit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_AddApp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_ToggelSurround;
        private System.Windows.Forms.RichTextBox textBoxLogs;
        private System.Windows.Forms.PictureBox pictureBoxLogs;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureBoxLoadConfig;
        private System.Windows.Forms.PictureBox pictureBoxSaveConfig;
        private System.Windows.Forms.PictureBox pictureBoxClose;
        private System.Windows.Forms.PictureBox pictureBoxSwitchSurround;
    }
}

