namespace NVidia_Surround_Assistant
{
    partial class ThumbGridView
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
            this.panelApplicationListView = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelApplicationListView
            // 
            this.panelApplicationListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelApplicationListView.AutoScroll = true;
            this.panelApplicationListView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.panelApplicationListView.Location = new System.Drawing.Point(3, 3);
            this.panelApplicationListView.MinimumSize = new System.Drawing.Size(257, 411);
            this.panelApplicationListView.Name = "panelApplicationListView";
            this.panelApplicationListView.Size = new System.Drawing.Size(540, 411);
            this.panelApplicationListView.TabIndex = 0;
            this.panelApplicationListView.Resize += new System.EventHandler(this.panelApplicationListView_Resize);
            // 
            // ThumbGridView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.Controls.Add(this.panelApplicationListView);
            this.MinimumSize = new System.Drawing.Size(260, 414);
            this.Name = "ThumbGridView";
            this.Size = new System.Drawing.Size(546, 414);
            this.Layout += new System.Windows.Forms.LayoutEventHandler(this.panelApplicationListView_Layout);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelApplicationListView;
    }
}
