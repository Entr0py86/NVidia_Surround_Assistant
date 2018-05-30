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
            this.lbGameName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBoxDelete = new System.Windows.Forms.PictureBox();
            this.pbGameBoxCover = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGameBoxCover)).BeginInit();
            this.SuspendLayout();
            // 
            // lbGameName
            // 
            this.lbGameName.AutoSize = true;
            this.lbGameName.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbGameName.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.lbGameName.Location = new System.Drawing.Point(24, 337);
            this.lbGameName.Name = "lbGameName";
            this.lbGameName.Size = new System.Drawing.Size(52, 17);
            this.lbGameName.TabIndex = 1;
            this.lbGameName.Text = "label1";
            this.lbGameName.ClientSizeChanged += new System.EventHandler(this.lbGameName_ClientSizeChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::NVidia_Surround_Assistant.Properties.Resources.edit_25x25;
            this.pictureBox1.Location = new System.Drawing.Point(171, 356);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(15, 15);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBoxEdit_Click);
            // 
            // pictureBoxDelete
            // 
            this.pictureBoxDelete.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxDelete.Image = global::NVidia_Surround_Assistant.Properties.Resources.delete_25x25;
            this.pictureBoxDelete.Location = new System.Drawing.Point(192, 356);
            this.pictureBoxDelete.Name = "pictureBoxDelete";
            this.pictureBoxDelete.Size = new System.Drawing.Size(15, 15);
            this.pictureBoxDelete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxDelete.TabIndex = 2;
            this.pictureBoxDelete.TabStop = false;
            this.pictureBoxDelete.Click += new System.EventHandler(this.pictureBoxDelete_Click);
            // 
            // pbGameBoxCover
            // 
            this.pbGameBoxCover.Location = new System.Drawing.Point(3, 3);
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
            this.BackColor = System.Drawing.SystemColors.GrayText;
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pictureBoxDelete);
            this.Controls.Add(this.lbGameName);
            this.Controls.Add(this.pbGameBoxCover);
            this.Name = "Thumb";
            this.Size = new System.Drawing.Size(234, 374);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbGameBoxCover)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbGameBoxCover;
        private System.Windows.Forms.Label lbGameName;
        private System.Windows.Forms.PictureBox pictureBoxDelete;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}
