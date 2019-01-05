namespace NVidia_Surround_Assistant
{
    partial class EditApplicationSettings
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditApplicationSettings));
            this.timerWait = new System.Windows.Forms.Timer(this.components);
            this.labelWait = new System.Windows.Forms.Label();
            this.labelGameList = new System.Windows.Forms.Label();
            this.comboBoxGameList = new System.Windows.Forms.ComboBox();
            this.labelSearch = new System.Windows.Forms.Label();
            this.textBoxGameSearch = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxAppPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxDisplayName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.comboBoxSurroundSetup = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDownSwitchbackTimeout = new System.Windows.Forms.NumericUpDown();
            this.pictureBoxNotPauseOnDetect = new System.Windows.Forms.PictureBox();
            this.pictureBoxPauseOnDetect = new System.Windows.Forms.PictureBox();
            this.pictureBoxChangeFileLocation = new System.Windows.Forms.PictureBox();
            this.pictureBoxCancel = new System.Windows.Forms.PictureBox();
            this.pictureBoxApply = new System.Windows.Forms.PictureBox();
            this.pictureBoxDisabled = new System.Windows.Forms.PictureBox();
            this.pictureBoxEnabled = new System.Windows.Forms.PictureBox();
            this.pictureBoxEditImage = new System.Windows.Forms.PictureBox();
            this.pictureBoxDeleteImage = new System.Windows.Forms.PictureBox();
            this.pictureBoxGameBoxCover = new System.Windows.Forms.PictureBox();
            this.pictureBoxSearch = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownWaitStart = new System.Windows.Forms.NumericUpDown();
            this.labelHttpFoundApp = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSwitchbackTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNotPauseOnDetect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPauseOnDetect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxChangeFileLocation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxApply)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDisabled)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEnabled)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEditImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDeleteImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGameBoxCover)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWaitStart)).BeginInit();
            this.SuspendLayout();
            // 
            // timerWait
            // 
            this.timerWait.Interval = 300;
            this.timerWait.Tick += new System.EventHandler(this.timerWait_Tick);
            // 
            // labelWait
            // 
            this.labelWait.AutoSize = true;
            this.labelWait.Font = new System.Drawing.Font("Malgun Gothic Semilight", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Millimeter, ((byte)(0)));
            this.labelWait.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.labelWait.Location = new System.Drawing.Point(146, 25);
            this.labelWait.Name = "labelWait";
            this.labelWait.Size = new System.Drawing.Size(94, 101);
            this.labelWait.TabIndex = 18;
            this.labelWait.Text = "...";
            this.labelWait.Visible = false;
            // 
            // labelGameList
            // 
            this.labelGameList.AutoSize = true;
            this.labelGameList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelGameList.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.labelGameList.Location = new System.Drawing.Point(22, 56);
            this.labelGameList.Name = "labelGameList";
            this.labelGameList.Size = new System.Drawing.Size(66, 15);
            this.labelGameList.TabIndex = 32;
            this.labelGameList.Text = "Game List:";
            this.labelGameList.Visible = false;
            // 
            // comboBoxGameList
            // 
            this.comboBoxGameList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.comboBoxGameList.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.comboBoxGameList.FormattingEnabled = true;
            this.comboBoxGameList.Location = new System.Drawing.Point(91, 53);
            this.comboBoxGameList.Name = "comboBoxGameList";
            this.comboBoxGameList.Size = new System.Drawing.Size(214, 21);
            this.comboBoxGameList.TabIndex = 33;
            this.toolTip1.SetToolTip(this.comboBoxGameList, "Select application to auto ppopulate the info");
            this.comboBoxGameList.Visible = false;
            this.comboBoxGameList.SelectedIndexChanged += new System.EventHandler(this.comboBoxGameList_SelectedIndexChanged);
            // 
            // labelSearch
            // 
            this.labelSearch.AutoSize = true;
            this.labelSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.labelSearch.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.labelSearch.Location = new System.Drawing.Point(22, 25);
            this.labelSearch.Name = "labelSearch";
            this.labelSearch.Size = new System.Drawing.Size(49, 15);
            this.labelSearch.TabIndex = 35;
            this.labelSearch.Text = "Search:";
            // 
            // textBoxGameSearch
            // 
            this.textBoxGameSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBoxGameSearch.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxGameSearch.Location = new System.Drawing.Point(91, 24);
            this.textBoxGameSearch.Name = "textBoxGameSearch";
            this.textBoxGameSearch.Size = new System.Drawing.Size(214, 20);
            this.textBoxGameSearch.TabIndex = 36;
            this.textBoxGameSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxGameSearch_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label5.Location = new System.Drawing.Point(388, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 15);
            this.label5.TabIndex = 50;
            this.label5.Text = "Image:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label4.Location = new System.Drawing.Point(22, 111);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 15);
            this.label4.TabIndex = 48;
            this.label4.Text = "Application Enabled:";
            this.toolTip1.SetToolTip(this.label4, "Will the surround switch be executed when application is detected");
            // 
            // textBoxAppPath
            // 
            this.textBoxAppPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBoxAppPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAppPath.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxAppPath.Location = new System.Drawing.Point(163, 198);
            this.textBoxAppPath.Name = "textBoxAppPath";
            this.textBoxAppPath.Size = new System.Drawing.Size(205, 21);
            this.textBoxAppPath.TabIndex = 47;
            this.textBoxAppPath.TextChanged += new System.EventHandler(this.textBoxAppPath_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label3.Location = new System.Drawing.Point(22, 201);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 15);
            this.label3.TabIndex = 46;
            this.label3.Text = "Application Path:";
            this.toolTip1.SetToolTip(this.label3, "Full Path to the application executable");
            // 
            // textBoxDisplayName
            // 
            this.textBoxDisplayName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.textBoxDisplayName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDisplayName.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxDisplayName.Location = new System.Drawing.Point(163, 171);
            this.textBoxDisplayName.Name = "textBoxDisplayName";
            this.textBoxDisplayName.Size = new System.Drawing.Size(205, 21);
            this.textBoxDisplayName.TabIndex = 45;
            this.textBoxDisplayName.TextChanged += new System.EventHandler(this.textBoxDisplayName_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label2.Location = new System.Drawing.Point(22, 174);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 15);
            this.label2.TabIndex = 44;
            this.label2.Text = "Display Name:";
            this.toolTip1.SetToolTip(this.label2, "Name displayed on thumb view");
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // comboBoxSurroundSetup
            // 
            this.comboBoxSurroundSetup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.comboBoxSurroundSetup.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.comboBoxSurroundSetup.FormattingEnabled = true;
            this.comboBoxSurroundSetup.Location = new System.Drawing.Point(163, 225);
            this.comboBoxSurroundSetup.Name = "comboBoxSurroundSetup";
            this.comboBoxSurroundSetup.Size = new System.Drawing.Size(205, 21);
            this.comboBoxSurroundSetup.TabIndex = 58;
            this.toolTip1.SetToolTip(this.comboBoxSurroundSetup, "Select the surround configuration to use for the application");
            this.comboBoxSurroundSetup.SelectedIndexChanged += new System.EventHandler(this.comboBoxSurroundSetup_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label7.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label7.Location = new System.Drawing.Point(22, 226);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(139, 15);
            this.label7.TabIndex = 57;
            this.label7.Text = "Surround Display Setup:";
            this.toolTip1.SetToolTip(this.label7, "Select the surround configuration to use for the application");
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label9.Location = new System.Drawing.Point(22, 141);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(161, 15);
            this.label9.TabIndex = 61;
            this.label9.Text = "Pause application on detect:";
            this.toolTip1.SetToolTip(this.label9, "Should the application be paused before switching into surround mode");
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label8.Location = new System.Drawing.Point(22, 255);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 15);
            this.label8.TabIndex = 59;
            this.label8.Text = "Switchback Timeout:";
            this.toolTip1.SetToolTip(this.label8, "Timeout for how long to switch back after process exit");
            // 
            // numericUpDownSwitchbackTimeout
            // 
            this.numericUpDownSwitchbackTimeout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.numericUpDownSwitchbackTimeout.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.numericUpDownSwitchbackTimeout.Location = new System.Drawing.Point(163, 255);
            this.numericUpDownSwitchbackTimeout.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.numericUpDownSwitchbackTimeout.Name = "numericUpDownSwitchbackTimeout";
            this.numericUpDownSwitchbackTimeout.Size = new System.Drawing.Size(205, 20);
            this.numericUpDownSwitchbackTimeout.TabIndex = 64;
            this.toolTip1.SetToolTip(this.numericUpDownSwitchbackTimeout, "Timeout for how long to switch back after process exit. (Max 120 sec = 2 min)");
            this.numericUpDownSwitchbackTimeout.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDownSwitchbackTimeout.ValueChanged += new System.EventHandler(this.numericUpDownSwitchbackTimeout_ValueChanged);
            // 
            // pictureBoxNotPauseOnDetect
            // 
            this.pictureBoxNotPauseOnDetect.Image = global::NVidia_Surround_Assistant.Properties.Resources.delete_filled_red_24x24;
            this.pictureBoxNotPauseOnDetect.Location = new System.Drawing.Point(260, 141);
            this.pictureBoxNotPauseOnDetect.Name = "pictureBoxNotPauseOnDetect";
            this.pictureBoxNotPauseOnDetect.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxNotPauseOnDetect.TabIndex = 63;
            this.pictureBoxNotPauseOnDetect.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxNotPauseOnDetect, "Should the application be paused before switching into surround mode");
            this.pictureBoxNotPauseOnDetect.Click += new System.EventHandler(this.pictureBoxNotPauseOnDetect_Click);
            // 
            // pictureBoxPauseOnDetect
            // 
            this.pictureBoxPauseOnDetect.Image = global::NVidia_Surround_Assistant.Properties.Resources.success_green_24x24;
            this.pictureBoxPauseOnDetect.Location = new System.Drawing.Point(260, 141);
            this.pictureBoxPauseOnDetect.Name = "pictureBoxPauseOnDetect";
            this.pictureBoxPauseOnDetect.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxPauseOnDetect.TabIndex = 62;
            this.pictureBoxPauseOnDetect.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxPauseOnDetect, "Should the application be paused before switching into surround mode");
            this.pictureBoxPauseOnDetect.Click += new System.EventHandler(this.pictureBoxPauseOnDetect_Click);
            // 
            // pictureBoxChangeFileLocation
            // 
            this.pictureBoxChangeFileLocation.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxChangeFileLocation.Image = global::NVidia_Surround_Assistant.Properties.Resources.edit_24x24;
            this.pictureBoxChangeFileLocation.Location = new System.Drawing.Point(371, 201);
            this.pictureBoxChangeFileLocation.Name = "pictureBoxChangeFileLocation";
            this.pictureBoxChangeFileLocation.Size = new System.Drawing.Size(15, 15);
            this.pictureBoxChangeFileLocation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxChangeFileLocation.TabIndex = 54;
            this.pictureBoxChangeFileLocation.TabStop = false;
            this.pictureBoxChangeFileLocation.Click += new System.EventHandler(this.pictureBoxChangeFileLocation_Click);
            // 
            // pictureBoxCancel
            // 
            this.pictureBoxCancel.Image = global::NVidia_Surround_Assistant.Properties.Resources.delete_filled_red_24x24;
            this.pictureBoxCancel.Location = new System.Drawing.Point(211, 331);
            this.pictureBoxCancel.Name = "pictureBoxCancel";
            this.pictureBoxCancel.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxCancel.TabIndex = 53;
            this.pictureBoxCancel.TabStop = false;
            this.pictureBoxCancel.Click += new System.EventHandler(this.pictureBoxCancel_Click);
            // 
            // pictureBoxApply
            // 
            this.pictureBoxApply.Image = global::NVidia_Surround_Assistant.Properties.Resources.success_green_24x24;
            this.pictureBoxApply.Location = new System.Drawing.Point(136, 332);
            this.pictureBoxApply.Name = "pictureBoxApply";
            this.pictureBoxApply.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxApply.TabIndex = 52;
            this.pictureBoxApply.TabStop = false;
            this.pictureBoxApply.Click += new System.EventHandler(this.pictureBoxApply_Click);
            // 
            // pictureBoxDisabled
            // 
            this.pictureBoxDisabled.Image = global::NVidia_Surround_Assistant.Properties.Resources.delete_filled_red_24x24;
            this.pictureBoxDisabled.Location = new System.Drawing.Point(260, 111);
            this.pictureBoxDisabled.Name = "pictureBoxDisabled";
            this.pictureBoxDisabled.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDisabled.TabIndex = 51;
            this.pictureBoxDisabled.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxDisabled, "Will the surround switch be executed when application is detected");
            this.pictureBoxDisabled.Click += new System.EventHandler(this.pictureBoxDisabled_Click);
            // 
            // pictureBoxEnabled
            // 
            this.pictureBoxEnabled.Image = global::NVidia_Surround_Assistant.Properties.Resources.success_green_24x24;
            this.pictureBoxEnabled.Location = new System.Drawing.Point(260, 111);
            this.pictureBoxEnabled.Name = "pictureBoxEnabled";
            this.pictureBoxEnabled.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxEnabled.TabIndex = 49;
            this.pictureBoxEnabled.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxEnabled, "Will the surround switch be executed when application is detected");
            this.pictureBoxEnabled.Click += new System.EventHandler(this.pictureBoxEnabled_Click);
            // 
            // pictureBoxEditImage
            // 
            this.pictureBoxEditImage.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxEditImage.Image = global::NVidia_Surround_Assistant.Properties.Resources.edit_24x24;
            this.pictureBoxEditImage.Location = new System.Drawing.Point(571, 16);
            this.pictureBoxEditImage.Name = "pictureBoxEditImage";
            this.pictureBoxEditImage.Size = new System.Drawing.Size(15, 15);
            this.pictureBoxEditImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxEditImage.TabIndex = 41;
            this.pictureBoxEditImage.TabStop = false;
            this.pictureBoxEditImage.Click += new System.EventHandler(this.pictureBoxEditImage_Click);
            // 
            // pictureBoxDeleteImage
            // 
            this.pictureBoxDeleteImage.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxDeleteImage.Image = global::NVidia_Surround_Assistant.Properties.Resources.delete_24x24;
            this.pictureBoxDeleteImage.Location = new System.Drawing.Point(592, 16);
            this.pictureBoxDeleteImage.Name = "pictureBoxDeleteImage";
            this.pictureBoxDeleteImage.Size = new System.Drawing.Size(15, 15);
            this.pictureBoxDeleteImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDeleteImage.TabIndex = 40;
            this.pictureBoxDeleteImage.TabStop = false;
            // 
            // pictureBoxGameBoxCover
            // 
            this.pictureBoxGameBoxCover.ErrorImage = global::NVidia_Surround_Assistant.Properties.Resources.delete_48x48;
            this.pictureBoxGameBoxCover.InitialImage = global::NVidia_Surround_Assistant.Properties.Resources.delete_48x48;
            this.pictureBoxGameBoxCover.Location = new System.Drawing.Point(392, 37);
            this.pictureBoxGameBoxCover.Name = "pictureBoxGameBoxCover";
            this.pictureBoxGameBoxCover.Padding = new System.Windows.Forms.Padding(2);
            this.pictureBoxGameBoxCover.Size = new System.Drawing.Size(227, 320);
            this.pictureBoxGameBoxCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxGameBoxCover.TabIndex = 39;
            this.pictureBoxGameBoxCover.TabStop = false;
            // 
            // pictureBoxSearch
            // 
            this.pictureBoxSearch.Image = global::NVidia_Surround_Assistant.Properties.Resources.search_24x24;
            this.pictureBoxSearch.Location = new System.Drawing.Point(311, 19);
            this.pictureBoxSearch.Name = "pictureBoxSearch";
            this.pictureBoxSearch.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxSearch.TabIndex = 38;
            this.pictureBoxSearch.TabStop = false;
            this.pictureBoxSearch.Click += new System.EventHandler(this.pictureBoxSearch_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label1.Location = new System.Drawing.Point(22, 290);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 15);
            this.label1.TabIndex = 66;
            this.label1.Text = "Wait after start:";
            this.toolTip1.SetToolTip(this.label1, "How long to wait after application was detected to start checking for exit. Some " +
        "applications start and exit multiple instantces before actually launching the ap" +
        "plication.");
            // 
            // numericUpDownWaitStart
            // 
            this.numericUpDownWaitStart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.numericUpDownWaitStart.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.numericUpDownWaitStart.Location = new System.Drawing.Point(163, 290);
            this.numericUpDownWaitStart.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numericUpDownWaitStart.Name = "numericUpDownWaitStart";
            this.numericUpDownWaitStart.Size = new System.Drawing.Size(205, 20);
            this.numericUpDownWaitStart.TabIndex = 67;
            this.toolTip1.SetToolTip(this.numericUpDownWaitStart, "How long to wait after application was detected to start checking for exit. Some " +
        "applications start and exit multiple instantces before actually launching the ap" +
        "plication. (Max 600 sec = 10 min)");
            this.numericUpDownWaitStart.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            // 
            // labelHttpFoundApp
            // 
            this.labelHttpFoundApp.AutoEllipsis = true;
            this.labelHttpFoundApp.AutoSize = true;
            this.labelHttpFoundApp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHttpFoundApp.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.labelHttpFoundApp.Location = new System.Drawing.Point(88, 77);
            this.labelHttpFoundApp.MaximumSize = new System.Drawing.Size(222, 15);
            this.labelHttpFoundApp.Name = "labelHttpFoundApp";
            this.labelHttpFoundApp.Size = new System.Drawing.Size(222, 15);
            this.labelHttpFoundApp.TabIndex = 65;
            this.labelHttpFoundApp.Text = "Applicationdfss Enabled:dxsfasfsffsfsffdf";
            this.labelHttpFoundApp.Visible = false;
            // 
            // EditApplicationSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.ClientSize = new System.Drawing.Size(629, 382);
            this.Controls.Add(this.numericUpDownWaitStart);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelHttpFoundApp);
            this.Controls.Add(this.numericUpDownSwitchbackTimeout);
            this.Controls.Add(this.pictureBoxNotPauseOnDetect);
            this.Controls.Add(this.pictureBoxPauseOnDetect);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.comboBoxSurroundSetup);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pictureBoxChangeFileLocation);
            this.Controls.Add(this.pictureBoxCancel);
            this.Controls.Add(this.pictureBoxApply);
            this.Controls.Add(this.pictureBoxDisabled);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pictureBoxEnabled);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxAppPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxDisplayName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBoxEditImage);
            this.Controls.Add(this.pictureBoxDeleteImage);
            this.Controls.Add(this.pictureBoxGameBoxCover);
            this.Controls.Add(this.pictureBoxSearch);
            this.Controls.Add(this.textBoxGameSearch);
            this.Controls.Add(this.labelSearch);
            this.Controls.Add(this.comboBoxGameList);
            this.Controls.Add(this.labelGameList);
            this.Controls.Add(this.labelWait);
            this.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditApplicationSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Application";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditApplicationSettings_FormClosing);
            this.Load += new System.EventHandler(this.EditApplicationSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSwitchbackTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxNotPauseOnDetect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPauseOnDetect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxChangeFileLocation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxApply)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDisabled)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEnabled)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEditImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDeleteImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGameBoxCover)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownWaitStart)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timerWait;
        private System.Windows.Forms.Label labelWait;
        private System.Windows.Forms.Label labelGameList;
        private System.Windows.Forms.ComboBox comboBoxGameList;
        private System.Windows.Forms.Label labelSearch;
        private System.Windows.Forms.TextBox textBoxGameSearch;
        private System.Windows.Forms.PictureBox pictureBoxSearch;
        private System.Windows.Forms.PictureBox pictureBoxDisabled;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.PictureBox pictureBoxEnabled;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxAppPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxDisplayName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBoxEditImage;
        private System.Windows.Forms.PictureBox pictureBoxDeleteImage;
        private System.Windows.Forms.PictureBox pictureBoxGameBoxCover;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.PictureBox pictureBoxApply;
        private System.Windows.Forms.PictureBox pictureBoxCancel;
        private System.Windows.Forms.PictureBox pictureBoxChangeFileLocation;
        private System.Windows.Forms.ComboBox comboBoxSurroundSetup;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pictureBoxNotPauseOnDetect;
        private System.Windows.Forms.PictureBox pictureBoxPauseOnDetect;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDownSwitchbackTimeout;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label labelHttpFoundApp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDownWaitStart;
    }
}
