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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxDelete = new System.Windows.Forms.PictureBox();
            this.pbGameBoxCover = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGameBoxCover)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbGameName
            // 
            this.lbGameName.AutoSize = true;
            this.lbGameName.BackColor = System.Drawing.SystemColors.GrayText;
            this.lbGameName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbGameName.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lbGameName.Location = new System.Drawing.Point(45, 345);
            this.lbGameName.Name = "lbGameName";
            this.lbGameName.Size = new System.Drawing.Size(52, 17);
            this.lbGameName.TabIndex = 1;
            this.lbGameName.Text = "label1";
            this.lbGameName.ClientSizeChanged += new System.EventHandler(this.lbGameName_ClientSizeChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.SystemColors.GrayText;
            this.pictureBox1.Image = global::NVidia_Surround_Assistant.Properties.Resources.edit_25x25;
            this.pictureBox1.Location = new System.Drawing.Point(201, 381);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(15, 15);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBox1, "Edit");
            this.pictureBox1.Click += new System.EventHandler(this.pictureBoxEdit_Click);
            // 
            // pictureBoxDelete
            // 
            this.pictureBoxDelete.BackColor = System.Drawing.SystemColors.GrayText;
            this.pictureBoxDelete.Image = global::NVidia_Surround_Assistant.Properties.Resources.delete_25x25;
            this.pictureBoxDelete.Location = new System.Drawing.Point(222, 381);
            this.pictureBoxDelete.Name = "pictureBoxDelete";
            this.pictureBoxDelete.Size = new System.Drawing.Size(15, 15);
            this.pictureBoxDelete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDelete.TabIndex = 2;
            this.pictureBoxDelete.TabStop = false;
            this.toolTip1.SetToolTip(this.pictureBoxDelete, "Remove");
            this.pictureBoxDelete.Click += new System.EventHandler(this.pictureBoxDelete_Click);
            // 
            // pbGameBoxCover
            // 
            this.pbGameBoxCover.BackColor = System.Drawing.SystemColors.GrayText;
            this.pbGameBoxCover.Location = new System.Drawing.Point(15, 15);
            this.pbGameBoxCover.Name = "pbGameBoxCover";
            this.pbGameBoxCover.Padding = new System.Windows.Forms.Padding(2);
            this.pbGameBoxCover.Size = new System.Drawing.Size(227, 320);
            this.pbGameBoxCover.TabIndex = 0;
            this.pbGameBoxCover.TabStop = false;
            this.pbGameBoxCover.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbGameBoxCover_MouseClick);
            this.pbGameBoxCover.MouseHover += new System.EventHandler(this.pbGameBoxCover_MouseHover);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.GrayText;
            this.panel1.Controls.Add(this.lbGameName);
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(237, 391);
            this.panel1.TabIndex = 4;
            // 
            // Thumb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBoxDelete);
            this.Controls.Add(this.pbGameBoxCover);
            this.Controls.Add(this.panel1);
            this.Name = "Thumb";
            this.Size = new System.Drawing.Size(257, 411);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGameBoxCover)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pbGameBoxCover;
        private System.Windows.Forms.Label lbGameName;
        private System.Windows.Forms.PictureBox pictureBoxDelete;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
