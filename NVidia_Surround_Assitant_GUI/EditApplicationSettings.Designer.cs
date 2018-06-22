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
            this.timerWait = new System.Windows.Forms.Timer(this.components);
            this.labelWait = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
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
            this.pictureBoxCancel = new System.Windows.Forms.PictureBox();
            this.pictureBoxApply = new System.Windows.Forms.PictureBox();
            this.pictureBoxDisabled = new System.Windows.Forms.PictureBox();
            this.pictureBoxEnabled = new System.Windows.Forms.PictureBox();
            this.pictureBoxEditImage = new System.Windows.Forms.PictureBox();
            this.pictureBoxDeleteImage = new System.Windows.Forms.PictureBox();
            this.pictureBoxGameBoxCover = new System.Windows.Forms.PictureBox();
            this.pictureBoxSearch = new System.Windows.Forms.PictureBox();
            this.pictureBoxChangeFileLocation = new System.Windows.Forms.PictureBox();
            this.comboBoxNormalSetup = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxSurroundSetup = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxApply)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDisabled)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEnabled)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEditImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDeleteImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGameBoxCover)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxChangeFileLocation)).BeginInit();
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
            this.labelWait.Location = new System.Drawing.Point(94, 25);
            this.labelWait.Name = "labelWait";
            this.labelWait.Size = new System.Drawing.Size(94, 101);
            this.labelWait.TabIndex = 18;
            this.labelWait.Text = "...";
            this.labelWait.Visible = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label6.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label6.Location = new System.Drawing.Point(22, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 15);
            this.label6.TabIndex = 32;
            this.label6.Text = "Game List:";
            // 
            // comboBoxGameList
            // 
            this.comboBoxGameList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboBoxGameList.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.comboBoxGameList.FormattingEnabled = true;
            this.comboBoxGameList.Location = new System.Drawing.Point(91, 53);
            this.comboBoxGameList.Name = "comboBoxGameList";
            this.comboBoxGameList.Size = new System.Drawing.Size(121, 21);
            this.comboBoxGameList.TabIndex = 33;
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
            this.textBoxGameSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBoxGameSearch.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxGameSearch.Location = new System.Drawing.Point(91, 24);
            this.textBoxGameSearch.Name = "textBoxGameSearch";
            this.textBoxGameSearch.Size = new System.Drawing.Size(121, 20);
            this.textBoxGameSearch.TabIndex = 36;
            this.textBoxGameSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxGameSearch_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label5.Location = new System.Drawing.Point(308, 19);
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
            this.label4.Location = new System.Drawing.Point(22, 158);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 15);
            this.label4.TabIndex = 48;
            this.label4.Text = "Application Enabled:";
            // 
            // textBoxAppPath
            // 
            this.textBoxAppPath.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBoxAppPath.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxAppPath.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxAppPath.Location = new System.Drawing.Point(163, 220);
            this.textBoxAppPath.Name = "textBoxAppPath";
            this.textBoxAppPath.Size = new System.Drawing.Size(124, 21);
            this.textBoxAppPath.TabIndex = 47;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label3.Location = new System.Drawing.Point(22, 223);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 15);
            this.label3.TabIndex = 46;
            this.label3.Text = "Application Path:";
            // 
            // textBoxDisplayName
            // 
            this.textBoxDisplayName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.textBoxDisplayName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDisplayName.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.textBoxDisplayName.Location = new System.Drawing.Point(163, 193);
            this.textBoxDisplayName.Name = "textBoxDisplayName";
            this.textBoxDisplayName.Size = new System.Drawing.Size(124, 21);
            this.textBoxDisplayName.TabIndex = 45;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label2.Location = new System.Drawing.Point(22, 196);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 15);
            this.label2.TabIndex = 44;
            this.label2.Text = "Display Name:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // pictureBoxCancel
            // 
            this.pictureBoxCancel.Image = global::NVidia_Surround_Assistant.Properties.Resources.delete_filled_red_25x25;
            this.pictureBoxCancel.Location = new System.Drawing.Point(163, 310);
            this.pictureBoxCancel.Name = "pictureBoxCancel";
            this.pictureBoxCancel.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxCancel.TabIndex = 53;
            this.pictureBoxCancel.TabStop = false;
            this.pictureBoxCancel.Click += new System.EventHandler(this.pictureBoxCancel_Click);
            // 
            // pictureBoxApply
            // 
            this.pictureBoxApply.Image = global::NVidia_Surround_Assistant.Properties.Resources.success_green_25x25;
            this.pictureBoxApply.Location = new System.Drawing.Point(88, 311);
            this.pictureBoxApply.Name = "pictureBoxApply";
            this.pictureBoxApply.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxApply.TabIndex = 52;
            this.pictureBoxApply.TabStop = false;
            this.pictureBoxApply.Click += new System.EventHandler(this.pictureBoxApply_Click);
            // 
            // pictureBoxDisabled
            // 
            this.pictureBoxDisabled.Image = global::NVidia_Surround_Assistant.Properties.Resources.success_red_25x25;
            this.pictureBoxDisabled.Location = new System.Drawing.Point(175, 152);
            this.pictureBoxDisabled.Name = "pictureBoxDisabled";
            this.pictureBoxDisabled.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxDisabled.TabIndex = 51;
            this.pictureBoxDisabled.TabStop = false;
            this.pictureBoxDisabled.Click += new System.EventHandler(this.pictureBoxDisabled_Click);
            // 
            // pictureBoxEnabled
            // 
            this.pictureBoxEnabled.Image = global::NVidia_Surround_Assistant.Properties.Resources.success_green_25x25;
            this.pictureBoxEnabled.Location = new System.Drawing.Point(175, 152);
            this.pictureBoxEnabled.Name = "pictureBoxEnabled";
            this.pictureBoxEnabled.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxEnabled.TabIndex = 49;
            this.pictureBoxEnabled.TabStop = false;
            this.pictureBoxEnabled.Click += new System.EventHandler(this.pictureBoxEnabled_Click);
            // 
            // pictureBoxEditImage
            // 
            this.pictureBoxEditImage.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxEditImage.Image = global::NVidia_Surround_Assistant.Properties.Resources.edit_25x25;
            this.pictureBoxEditImage.Location = new System.Drawing.Point(491, 16);
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
            this.pictureBoxDeleteImage.Image = global::NVidia_Surround_Assistant.Properties.Resources.delete_25x25;
            this.pictureBoxDeleteImage.Location = new System.Drawing.Point(512, 16);
            this.pictureBoxDeleteImage.Name = "pictureBoxDeleteImage";
            this.pictureBoxDeleteImage.Size = new System.Drawing.Size(15, 15);
            this.pictureBoxDeleteImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDeleteImage.TabIndex = 40;
            this.pictureBoxDeleteImage.TabStop = false;
            // 
            // pictureBoxGameBoxCover
            // 
            this.pictureBoxGameBoxCover.ErrorImage = global::NVidia_Surround_Assistant.Properties.Resources.delete_50x50;
            this.pictureBoxGameBoxCover.InitialImage = global::NVidia_Surround_Assistant.Properties.Resources.delete_50x50;
            this.pictureBoxGameBoxCover.Location = new System.Drawing.Point(312, 37);
            this.pictureBoxGameBoxCover.Name = "pictureBoxGameBoxCover";
            this.pictureBoxGameBoxCover.Padding = new System.Windows.Forms.Padding(2);
            this.pictureBoxGameBoxCover.Size = new System.Drawing.Size(227, 320);
            this.pictureBoxGameBoxCover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxGameBoxCover.TabIndex = 39;
            this.pictureBoxGameBoxCover.TabStop = false;
            // 
            // pictureBoxSearch
            // 
            this.pictureBoxSearch.Image = global::NVidia_Surround_Assistant.Properties.Resources.search_grey_25x25;
            this.pictureBoxSearch.Location = new System.Drawing.Point(234, 21);
            this.pictureBoxSearch.Name = "pictureBoxSearch";
            this.pictureBoxSearch.Size = new System.Drawing.Size(25, 25);
            this.pictureBoxSearch.TabIndex = 38;
            this.pictureBoxSearch.TabStop = false;
            this.pictureBoxSearch.Click += new System.EventHandler(this.pictureBoxSearch_Click);
            // 
            // pictureBoxChangeFileLocation
            // 
            this.pictureBoxChangeFileLocation.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxChangeFileLocation.Image = global::NVidia_Surround_Assistant.Properties.Resources.edit_25x25;
            this.pictureBoxChangeFileLocation.Location = new System.Drawing.Point(291, 223);
            this.pictureBoxChangeFileLocation.Name = "pictureBoxChangeFileLocation";
            this.pictureBoxChangeFileLocation.Size = new System.Drawing.Size(15, 15);
            this.pictureBoxChangeFileLocation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxChangeFileLocation.TabIndex = 54;
            this.pictureBoxChangeFileLocation.TabStop = false;
            this.pictureBoxChangeFileLocation.Click += new System.EventHandler(this.pictureBoxChangeFileLocation_Click);
            // 
            // comboBoxNormalSetup
            // 
            this.comboBoxNormalSetup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboBoxNormalSetup.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.comboBoxNormalSetup.FormattingEnabled = true;
            this.comboBoxNormalSetup.Location = new System.Drawing.Point(163, 274);
            this.comboBoxNormalSetup.Name = "comboBoxNormalSetup";
            this.comboBoxNormalSetup.Size = new System.Drawing.Size(124, 21);
            this.comboBoxNormalSetup.TabIndex = 56;
            this.comboBoxNormalSetup.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label1.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label1.Location = new System.Drawing.Point(22, 275);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 15);
            this.label1.TabIndex = 55;
            this.label1.Text = "Normal Display Setup:";
            this.label1.Visible = false;
            // 
            // comboBoxSurroundSetup
            // 
            this.comboBoxSurroundSetup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.comboBoxSurroundSetup.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.comboBoxSurroundSetup.FormattingEnabled = true;
            this.comboBoxSurroundSetup.Location = new System.Drawing.Point(163, 247);
            this.comboBoxSurroundSetup.Name = "comboBoxSurroundSetup";
            this.comboBoxSurroundSetup.Size = new System.Drawing.Size(124, 21);
            this.comboBoxSurroundSetup.TabIndex = 58;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.label7.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.label7.Location = new System.Drawing.Point(22, 248);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(139, 15);
            this.label7.TabIndex = 57;
            this.label7.Text = "Surround Display Setup:";
            // 
            // EditApplicationSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(560, 382);
            this.Controls.Add(this.comboBoxSurroundSetup);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBoxNormalSetup);
            this.Controls.Add(this.label1);
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
            this.Controls.Add(this.label6);
            this.Controls.Add(this.labelWait);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "EditApplicationSettings";
            this.Text = "Edit Application";
            this.Load += new System.EventHandler(this.EditApplicationSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxApply)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDisabled)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEnabled)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEditImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDeleteImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGameBoxCover)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxChangeFileLocation)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Timer timerWait;
        private System.Windows.Forms.Label labelWait;
        private System.Windows.Forms.Label label6;
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
        private System.Windows.Forms.ComboBox comboBoxNormalSetup;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxSurroundSetup;
        private System.Windows.Forms.Label label7;
    }
}
