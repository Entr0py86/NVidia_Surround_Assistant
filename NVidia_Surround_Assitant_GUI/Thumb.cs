using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NVidia_Surround_Assistant
{
    public delegate void DelegateEditApplication(Thumb AppInfo);
    public delegate void DelegateEnableApplication(Thumb AppInfo);
    public delegate void DelegateDisableApplication(Thumb AppInfo);
    public delegate void DelegateRemoveApplication(Thumb AppInfo);

    public partial class Thumb : UserControl
    {
        public DelegateEditApplication editApplication;
        public DelegateRemoveApplication removeApplication;

        private ApplicationInfo appInfo;

        public Thumb(ApplicationInfo AppInfo, NLog.Logger logger)
        {
            InitializeComponent();
            if (AppInfo == null)
                return;
            //Allocate appInfo
            appInfo = AppInfo;
            UpdateThumb();

            try
            {
                this.Name = appInfo.DisplayName;
            }
            catch(Exception)
            {
                this.Name = appInfo.DisplayName + " 2";
            }
        }

        public int Id
        {
            get { return appInfo.Id; }
            set { appInfo.Id = value; }
        }

        public bool AppEnabled
        {
            get { return appInfo.Enabled; }
            set { appInfo.Enabled = value; }
        }

        public string FullPath
        {
            get { return appInfo.FullPath; }
            set { appInfo.FullPath = value; }
        }

        public string DisplayName
        {
            get { return appInfo.DisplayName; }
            set { appInfo.FullPath = value; }
        }

        public Bitmap Image
        {
            get { return appInfo.Image; }
            set { appInfo.Image = value; }
        }

        public ApplicationInfo applicationInfo
        {
            get { return appInfo; }
            set
            {
                appInfo = value;
                UpdateThumb();
            }
        }

        public void UpdateThumb()
        {
            if (appInfo.Image != null)
            {
                pbGameBoxCover.Image = MergeImages(appInfo.Image,
                    (appInfo.Enabled ? NVidia_Surround_Assistant.Properties.Resources.success_green_25x25
                    : NVidia_Surround_Assistant.Properties.Resources.success_red_25x25));
            }
            else
                pbGameBoxCover.Image = NVidia_Surround_Assistant.Properties.Resources.close_50x50;

            lbGameName.Text = appInfo.DisplayName;
        }

        private Image MergeImages(Image cover, Image tickMark)
        {

            //TODO complete function
            //Rectangle rect = new Rectangle(new Point(0,0), cover.Size);


            return cover;
        }

        private void lbGameName_ClientSizeChanged(object sender, EventArgs e)
        {
            SuspendLayout();
            
            if (lbGameName.Width >= Width)
            {
                lbGameName.AutoSize = false;
                lbGameName.Width = Width - 20;
                lbGameName.Left = 10;
                lbGameName.AutoEllipsis = true;
            }
            else
            {
                lbGameName.Left = (Width - lbGameName.Width) / 2;
            }
            ResumeLayout();
        }

        private void pictureBoxDelete_Click(object sender, EventArgs e)
        {
            if (removeApplication != null)
                removeApplication(this);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (editApplication != null)
                editApplication(this);
        }
    }
}
