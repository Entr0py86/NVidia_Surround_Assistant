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
            this.processEventWorker = new System.ComponentModel.BackgroundWorker();
            this.saveSurroundFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.systemTrayIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip_SystemTray = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_AddApp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_LoadApp = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripLoadApp = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem_ToggelSurround = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_LoadSurroundFile = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripLoadSurroundConfig = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_SaveSurroundFile = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripSaveSurroundConfig = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.saveAsDefaultToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsDefaultSurroundToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSurroundProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripDeleteProfiles = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem_Quit = new System.Windows.Forms.ToolStripMenuItem();
            this.textBoxLogs = new System.Windows.Forms.RichTextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pictureBoxSwitchSurround = new System.Windows.Forms.PictureBox();
            this.pictureBoxClose = new System.Windows.Forms.PictureBox();
            this.pictureBoxSettings = new System.Windows.Forms.PictureBox();
            this.pictureBoxAddGame = new System.Windows.Forms.PictureBox();
            this.thumbGridView = new NVidia_Surround_Assistant.ThumbGridView();
            this.contextMenuStrip_SystemTray.SuspendLayout();
            this.contextMenuStripSaveSurroundConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSwitchSurround)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSettings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAddGame)).BeginInit();
            this.SuspendLayout();
            // 
            // processEventWorker
            // 
            this.processEventWorker.WorkerReportsProgress = true;
            this.processEventWorker.WorkerSupportsCancellation = true;
            this.processEventWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.processEventWorker_DoWork);
            this.processEventWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.processEventWorker_ProgressChanged);
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
            this.contextMenuStrip_SystemTray.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.contextMenuStrip_SystemTray.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_AddApp,
            this.toolStripMenuItem_LoadApp,
            this.toolStripSeparator2,
            this.toolStripMenuItem_ToggelSurround,
            this.toolStripMenuItem_LoadSurroundFile,
            this.toolStripMenuItem_SaveSurroundFile,
            this.deleteSurroundProfileToolStripMenuItem,
            this.toolStripSeparator1,
            this.toolStripMenuItem_Quit});
            this.contextMenuStrip_SystemTray.Name = "contextMenuStrip_SystemTray";
            this.contextMenuStrip_SystemTray.ShowImageMargin = false;
            this.contextMenuStrip_SystemTray.Size = new System.Drawing.Size(187, 170);
            this.contextMenuStrip_SystemTray.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_SystemTray_Opening);
            // 
            // toolStripMenuItem_AddApp
            // 
            this.toolStripMenuItem_AddApp.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripMenuItem_AddApp.Name = "toolStripMenuItem_AddApp";
            this.toolStripMenuItem_AddApp.Size = new System.Drawing.Size(186, 22);
            this.toolStripMenuItem_AddApp.Text = "Add Application";
            this.toolStripMenuItem_AddApp.Click += new System.EventHandler(this.toolStripMenuItem_AddApp_Click);
            // 
            // toolStripMenuItem_LoadApp
            // 
            this.toolStripMenuItem_LoadApp.DropDown = this.contextMenuStripLoadApp;
            this.toolStripMenuItem_LoadApp.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripMenuItem_LoadApp.Name = "toolStripMenuItem_LoadApp";
            this.toolStripMenuItem_LoadApp.Size = new System.Drawing.Size(186, 22);
            this.toolStripMenuItem_LoadApp.Text = "Start Application";
            // 
            // contextMenuStripLoadApp
            // 
            this.contextMenuStripLoadApp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.contextMenuStripLoadApp.Name = "contextMenuStripLoadSurroundConfig";
            this.contextMenuStripLoadApp.OwnerItem = this.toolStripMenuItem_LoadApp;
            this.contextMenuStripLoadApp.ShowImageMargin = false;
            this.contextMenuStripLoadApp.Size = new System.Drawing.Size(36, 4);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(183, 6);
            // 
            // toolStripMenuItem_ToggelSurround
            // 
            this.toolStripMenuItem_ToggelSurround.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripMenuItem_ToggelSurround.Name = "toolStripMenuItem_ToggelSurround";
            this.toolStripMenuItem_ToggelSurround.Size = new System.Drawing.Size(186, 22);
            this.toolStripMenuItem_ToggelSurround.Text = "Switch To Surround Mode";
            this.toolStripMenuItem_ToggelSurround.Click += new System.EventHandler(this.toolStripMenuItem_ToggelSurround_Click);
            // 
            // toolStripMenuItem_LoadSurroundFile
            // 
            this.toolStripMenuItem_LoadSurroundFile.DropDown = this.contextMenuStripLoadSurroundConfig;
            this.toolStripMenuItem_LoadSurroundFile.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripMenuItem_LoadSurroundFile.Name = "toolStripMenuItem_LoadSurroundFile";
            this.toolStripMenuItem_LoadSurroundFile.Size = new System.Drawing.Size(186, 22);
            this.toolStripMenuItem_LoadSurroundFile.Text = "Apply Surround Profile";
            // 
            // contextMenuStripLoadSurroundConfig
            // 
            this.contextMenuStripLoadSurroundConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.contextMenuStripLoadSurroundConfig.Name = "contextMenuStripLoadSurroundConfig";
            this.contextMenuStripLoadSurroundConfig.OwnerItem = this.toolStripMenuItem_LoadSurroundFile;
            this.contextMenuStripLoadSurroundConfig.ShowImageMargin = false;
            this.contextMenuStripLoadSurroundConfig.Size = new System.Drawing.Size(36, 4);
            // 
            // toolStripMenuItem_SaveSurroundFile
            // 
            this.toolStripMenuItem_SaveSurroundFile.DropDown = this.contextMenuStripSaveSurroundConfig;
            this.toolStripMenuItem_SaveSurroundFile.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripMenuItem_SaveSurroundFile.Name = "toolStripMenuItem_SaveSurroundFile";
            this.toolStripMenuItem_SaveSurroundFile.Size = new System.Drawing.Size(186, 22);
            this.toolStripMenuItem_SaveSurroundFile.Text = "Save Surround Profile";
            // 
            // contextMenuStripSaveSurroundConfig
            // 
            this.contextMenuStripSaveSurroundConfig.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.contextMenuStripSaveSurroundConfig.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveAsDefaultToolStripMenuItem,
            this.saveAsDefaultSurroundToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.contextMenuStripSaveSurroundConfig.Name = "contextMenuStripSaveSurroundConfig";
            this.contextMenuStripSaveSurroundConfig.OwnerItem = this.toolStripMenuItem_SaveSurroundFile;
            this.contextMenuStripSaveSurroundConfig.ShowImageMargin = false;
            this.contextMenuStripSaveSurroundConfig.Size = new System.Drawing.Size(181, 70);
            // 
            // saveAsDefaultToolStripMenuItem
            // 
            this.saveAsDefaultToolStripMenuItem.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.saveAsDefaultToolStripMenuItem.Name = "saveAsDefaultToolStripMenuItem";
            this.saveAsDefaultToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsDefaultToolStripMenuItem.Text = "Save as Default";
            this.saveAsDefaultToolStripMenuItem.Click += new System.EventHandler(this.saveAsDefaultToolStripMenuItem_Click);
            // 
            // saveAsDefaultSurroundToolStripMenuItem
            // 
            this.saveAsDefaultSurroundToolStripMenuItem.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.saveAsDefaultSurroundToolStripMenuItem.Name = "saveAsDefaultSurroundToolStripMenuItem";
            this.saveAsDefaultSurroundToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsDefaultSurroundToolStripMenuItem.Text = "Save as Surround Default";
            this.saveAsDefaultSurroundToolStripMenuItem.Click += new System.EventHandler(this.saveAsDefaultSurroundToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProfileToolStripMenuItem});
            this.saveAsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveAsToolStripMenuItem.Text = "Save as";
            // 
            // newProfileToolStripMenuItem
            // 
            this.newProfileToolStripMenuItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.newProfileToolStripMenuItem.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.newProfileToolStripMenuItem.Name = "newProfileToolStripMenuItem";
            this.newProfileToolStripMenuItem.Size = new System.Drawing.Size(135, 22);
            this.newProfileToolStripMenuItem.Text = "New Profile";
            this.newProfileToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // deleteSurroundProfileToolStripMenuItem
            // 
            this.deleteSurroundProfileToolStripMenuItem.DropDown = this.contextMenuStripDeleteProfiles;
            this.deleteSurroundProfileToolStripMenuItem.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.deleteSurroundProfileToolStripMenuItem.Name = "deleteSurroundProfileToolStripMenuItem";
            this.deleteSurroundProfileToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.deleteSurroundProfileToolStripMenuItem.Text = "Delete Surround Profile";
            // 
            // contextMenuStripDeleteProfiles
            // 
            this.contextMenuStripDeleteProfiles.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.contextMenuStripDeleteProfiles.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStripDeleteProfiles.Name = "contextMenuStripLoadSurroundConfig";
            this.contextMenuStripDeleteProfiles.OwnerItem = this.deleteSurroundProfileToolStripMenuItem;
            this.contextMenuStripDeleteProfiles.ShowImageMargin = false;
            this.contextMenuStripDeleteProfiles.Size = new System.Drawing.Size(36, 4);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(183, 6);
            // 
            // toolStripMenuItem_Quit
            // 
            this.toolStripMenuItem_Quit.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.toolStripMenuItem_Quit.Name = "toolStripMenuItem_Quit";
            this.toolStripMenuItem_Quit.Size = new System.Drawing.Size(186, 22);
            this.toolStripMenuItem_Quit.Text = "Quit";
            this.toolStripMenuItem_Quit.Click += new System.EventHandler(this.toolStripMenuItem_Quit_Click);
            // 
            // textBoxLogs
            // 
            this.textBoxLogs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLogs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBoxLogs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLogs.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLogs.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxLogs.Location = new System.Drawing.Point(12, 476);
            this.textBoxLogs.Name = "textBoxLogs";
            this.textBoxLogs.Size = new System.Drawing.Size(1056, 141);
            this.textBoxLogs.TabIndex = 46;
            this.textBoxLogs.Text = "";
            this.textBoxLogs.VisibleChanged += new System.EventHandler(this.textBoxLogs_VisibleChanged);
            // 
            // pictureBoxSwitchSurround
            // 
            this.pictureBoxSwitchSurround.Image = global::NVidia_Surround_Assistant.Properties.Resources.help_24x24;
            this.pictureBoxSwitchSurround.Location = new System.Drawing.Point(48, 12);
            this.pictureBoxSwitchSurround.Name = "pictureBoxSwitchSurround";
            this.pictureBoxSwitchSurround.Size = new System.Drawing.Size(30, 30);
            this.pictureBoxSwitchSurround.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxSwitchSurround.TabIndex = 76;
            this.pictureBoxSwitchSurround.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxSwitchSurround, "Toggle Surround Mode");
            this.pictureBoxSwitchSurround.Click += new System.EventHandler(this.pictureBoxSwitchSurround_Click);
            this.pictureBoxSwitchSurround.MouseEnter += new System.EventHandler(this.pictureBox_MouseEnter);
            this.pictureBoxSwitchSurround.MouseLeave += new System.EventHandler(this.pictureBox_MouseLeave);
            // 
            // pictureBoxClose
            // 
            this.pictureBoxClose.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxClose.Image = global::NVidia_Surround_Assistant.Properties.Resources.close24x24;
            this.pictureBoxClose.Location = new System.Drawing.Point(1042, 12);
            this.pictureBoxClose.MaximumSize = new System.Drawing.Size(30, 30);
            this.pictureBoxClose.MinimumSize = new System.Drawing.Size(30, 30);
            this.pictureBoxClose.Name = "pictureBoxClose";
            this.pictureBoxClose.Size = new System.Drawing.Size(30, 30);
            this.pictureBoxClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxClose.TabIndex = 75;
            this.pictureBoxClose.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxClose, "Exit");
            this.pictureBoxClose.Click += new System.EventHandler(this.pictureBoxClose_Click);
            this.pictureBoxClose.MouseEnter += new System.EventHandler(this.pictureBox_MouseEnter);
            this.pictureBoxClose.MouseLeave += new System.EventHandler(this.pictureBox_MouseLeave);
            // 
            // pictureBoxSettings
            // 
            this.pictureBoxSettings.Image = global::NVidia_Surround_Assistant.Properties.Resources.settings_24x24;
            this.pictureBoxSettings.Location = new System.Drawing.Point(84, 12);
            this.pictureBoxSettings.Name = "pictureBoxSettings";
            this.pictureBoxSettings.Size = new System.Drawing.Size(30, 30);
            this.pictureBoxSettings.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxSettings.TabIndex = 1;
            this.pictureBoxSettings.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxSettings, "Settings");
            this.pictureBoxSettings.Click += new System.EventHandler(this.pictureBoxSettings_Click);
            this.pictureBoxSettings.MouseEnter += new System.EventHandler(this.pictureBox_MouseEnter);
            this.pictureBoxSettings.MouseLeave += new System.EventHandler(this.pictureBox_MouseLeave);
            // 
            // pictureBoxAddGame
            // 
            this.pictureBoxAddGame.Image = global::NVidia_Surround_Assistant.Properties.Resources.add_24x24;
            this.pictureBoxAddGame.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxAddGame.Name = "pictureBoxAddGame";
            this.pictureBoxAddGame.Size = new System.Drawing.Size(30, 30);
            this.pictureBoxAddGame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxAddGame.TabIndex = 0;
            this.pictureBoxAddGame.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxAddGame, "Add Application");
            this.pictureBoxAddGame.Click += new System.EventHandler(this.PictureBoxAddGame_Click);
            this.pictureBoxAddGame.MouseEnter += new System.EventHandler(this.pictureBox_MouseEnter);
            this.pictureBoxAddGame.MouseLeave += new System.EventHandler(this.pictureBox_MouseLeave);
            // 
            // thumbGridView
            // 
            this.thumbGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.thumbGridView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.thumbGridView.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.thumbGridView.Location = new System.Drawing.Point(12, 48);
            this.thumbGridView.MinimumSize = new System.Drawing.Size(260, 414);
            this.thumbGridView.Name = "thumbGridView";
            this.thumbGridView.Size = new System.Drawing.Size(1056, 414);
            this.thumbGridView.TabIndex = 5;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(1084, 629);
            this.Controls.Add(this.pictureBoxSwitchSurround);
            this.Controls.Add(this.pictureBoxClose);
            this.Controls.Add(this.textBoxLogs);
            this.Controls.Add(this.thumbGridView);
            this.Controls.Add(this.pictureBoxSettings);
            this.Controls.Add(this.pictureBoxAddGame);
            this.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NVidia Surround Assistant";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.MainForm_Layout);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.contextMenuStrip_SystemTray.ResumeLayout(false);
            this.contextMenuStripSaveSurroundConfig.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSwitchSurround)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSettings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAddGame)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.ComponentModel.BackgroundWorker processEventWorker;
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
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureBoxClose;
        private System.Windows.Forms.PictureBox pictureBoxSwitchSurround;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_LoadApp;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripLoadApp;        
        private System.Windows.Forms.ContextMenuStrip contextMenuStripSaveSurroundConfig;
        private System.Windows.Forms.ToolStripMenuItem saveAsDefaultToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsDefaultSurroundToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripLoadSurroundConfig;
        private System.Windows.Forms.ToolStripMenuItem newProfileToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripDeleteProfiles;
        private System.Windows.Forms.ToolStripMenuItem deleteSurroundProfileToolStripMenuItem;
    }
}

