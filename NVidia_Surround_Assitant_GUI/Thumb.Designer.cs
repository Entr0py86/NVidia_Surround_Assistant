namespace NVidia_Surround_Assistant
{
    partial class Thumb
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
            this.lbGameName = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBoxDelete = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxLaunchApplication = new System.Windows.Forms.PictureBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.pbGameBoxCover = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLaunchApplication)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGameBoxCover)).BeginInit();
            this.SuspendLayout();
            // 
            // lbGameName
            // 
            this.lbGameName.AutoSize = true;
            this.lbGameName.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.lbGameName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbGameName.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lbGameName.Location = new System.Drawing.Point(41, 340);
            this.lbGameName.Name = "lbGameName";
            this.lbGameName.Size = new System.Drawing.Size(52, 17);
            this.lbGameName.TabIndex = 1;
            this.lbGameName.Text = "label1";
            this.lbGameName.ClientSizeChanged += new System.EventHandler(this.lbGameName_ClientSizeChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.panel1.Controls.Add(this.pictureBoxDelete);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.pictureBoxLaunchApplication);
            this.panel1.Controls.Add(this.lbGameName);
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(237, 391);
            this.panel1.TabIndex = 4;
            // 
            // pictureBoxDelete
            // 
            this.pictureBoxDelete.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.pictureBoxDelete.Image = global::NVidia_Surround_Assistant.Properties.Resources.delete_16x16;
            this.pictureBoxDelete.Location = new System.Drawing.Point(214, 368);
            this.pictureBoxDelete.Name = "pictureBoxDelete";
            this.pictureBoxDelete.Size = new System.Drawing.Size(20, 20);
            this.pictureBoxDelete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxDelete.TabIndex = 2;
            this.pictureBoxDelete.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxDelete, "Remove Application");
            this.pictureBoxDelete.Click += new System.EventHandler(this.pictureBoxDelete_Click);
            this.pictureBoxDelete.MouseEnter += new System.EventHandler(this.pictureBox_MouseEnter);
            this.pictureBoxDelete.MouseLeave += new System.EventHandler(this.pictureBox_MouseLeave);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.pictureBox1.Image = global::NVidia_Surround_Assistant.Properties.Resources.edit_16x16;
            this.pictureBox1.Location = new System.Drawing.Point(188, 368);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 20);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox1, "Edit Application");
            this.pictureBox1.Click += new System.EventHandler(this.pictureBoxEdit_Click);
            this.pictureBox1.MouseEnter += new System.EventHandler(this.pictureBox_MouseEnter);
            this.pictureBox1.MouseLeave += new System.EventHandler(this.pictureBox_MouseLeave);
            // 
            // pictureBoxLaunchApplication
            // 
            this.pictureBoxLaunchApplication.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.pictureBoxLaunchApplication.Image = global::NVidia_Surround_Assistant.Properties.Resources.download_16x16;
            this.pictureBoxLaunchApplication.Location = new System.Drawing.Point(3, 368);
            this.pictureBoxLaunchApplication.Name = "pictureBoxLaunchApplication";
            this.pictureBoxLaunchApplication.Size = new System.Drawing.Size(20, 20);
            this.pictureBoxLaunchApplication.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxLaunchApplication.TabIndex = 5;
            this.pictureBoxLaunchApplication.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxLaunchApplication, "Launch Application");
            this.pictureBoxLaunchApplication.Click += new System.EventHandler(this.pictureBoxLaunchApplication_Click);
            this.pictureBoxLaunchApplication.MouseEnter += new System.EventHandler(this.pictureBox_MouseEnter);
            this.pictureBoxLaunchApplication.MouseLeave += new System.EventHandler(this.pictureBox_MouseLeave);
            // 
            // pbGameBoxCover
            // 
            this.pbGameBoxCover.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.pbGameBoxCover.Location = new System.Drawing.Point(15, 15);
            this.pbGameBoxCover.Name = "pbGameBoxCover";
            this.pbGameBoxCover.Padding = new System.Windows.Forms.Padding(2);
            this.pbGameBoxCover.Size = new System.Drawing.Size(227, 320);
            this.pbGameBoxCover.TabIndex = 0;
            this.pbGameBoxCover.TabStop = false;
            this.pbGameBoxCover.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbGameBoxCover_MouseClick);
            this.pbGameBoxCover.MouseHover += new System.EventHandler(this.pbGameBoxCover_MouseHover);
            // 
            // Thumb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Controls.Add(this.pbGameBoxCover);
            this.Controls.Add(this.panel1);
            this.Name = "Thumb";
            this.Size = new System.Drawing.Size(257, 411);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLaunchApplication)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGameBoxCover)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbGameBoxCover;
        private System.Windows.Forms.Label lbGameName;
        private System.Windows.Forms.PictureBox pictureBoxDelete;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.PictureBox pictureBoxLaunchApplication;
    }
}
