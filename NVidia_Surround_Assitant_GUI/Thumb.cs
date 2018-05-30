using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace NVidia_Surround_Assistant
{
    public delegate void DelegateEditApplication(Thumb AppInfo);
    public delegate void DelegateEnableApplication(Thumb AppInfo);
    public delegate void DelegateDisableApplication(Thumb AppInfo);
    public delegate void DelegateRemoveApplication(Thumb AppInfo);

    public partial class Thumb : UserControl
    {
        public DelegateEditApplication editApplication;
        public DelegateEditApplication silentEditApplication;
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

        public ApplicationInfo ApplicationInfo
        {
            get { return appInfo; }
            set
            {
                appInfo = value;
                UpdateThumb();
            }
        }

        private void UpdateThumb()
        {
            if (appInfo.Image != null)
            {
                //if ((appInfo.Image.Size.Height < pbGameBoxCover.Height) && (appInfo.Image.Size.Width < pbGameBoxCover.Width))
                //{
                //    pbGameBoxCover.SizeMode = PictureBoxSizeMode.CenterImage;
                //}
                //else
                {
                    pbGameBoxCover.Image = MergeImages(appInfo.Image,
                        (appInfo.Enabled ? NVidia_Surround_Assistant.Properties.Resources.success_green_25x25
                        : NVidia_Surround_Assistant.Properties.Resources.delete_filled_red_25x25));
                }
            }
            else
                pbGameBoxCover.Image = NVidia_Surround_Assistant.Properties.Resources.close_50x50;

            lbGameName.Text = appInfo.DisplayName;
        }

        private Image MergeImages(Image cover, Image tickMark)
        {
            Image target;

            if ((cover.Height < pbGameBoxCover.Height) && (cover.Width < pbGameBoxCover.Width))
            {
                target = new Bitmap(pbGameBoxCover.Width, pbGameBoxCover.Height, PixelFormat.Format32bppArgb);
                var graphics = Graphics.FromImage(target);
                graphics.CompositingMode = CompositingMode.SourceOver; // this is the default, but just to be clear
                                
                graphics.DrawImage(cover, (int)Math.Floor((pbGameBoxCover.Width - cover.Width) / 2.0), (int)Math.Floor((pbGameBoxCover.Height - cover.Height) / 2.0));
                graphics.DrawImage(tickMark, 5, 5, 25, 25);
            }
            else
            {
                target = new Bitmap(cover.Width, cover.Height, PixelFormat.Format32bppArgb);
                var graphics = Graphics.FromImage(target);
                graphics.CompositingMode = CompositingMode.SourceOver; // this is the default, but just to be clear

                graphics.DrawImage(cover, 0, 0);
                graphics.DrawImage(tickMark, 5, 5, 25, 25);
            }
                        
            return target;
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

        private void pictureBoxEdit_Click(object sender, EventArgs e)
        {
            if (editApplication != null)
                editApplication(this);
        }

        private void pbGameBoxCover_MouseClick(object sender, MouseEventArgs e)
        {
            if((e.Location.X < 35) && (e.Location.X > pbGameBoxCover.Location.X)
                && (e.Location.Y < 35) && (e.Location.Y > pbGameBoxCover.Location.Y))
            {
                if(silentEditApplication != null)
                    silentEditApplication(this);
                UpdateThumb();
            }
        }

        private void pbGameBoxCover_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            Point mousePos = pbGameBoxCover.PointToClient(Control.MousePosition);            
            String toolMsg;

            if ((mousePos.X < 35) && (mousePos.X > pbGameBoxCover.Location.X)
                && (mousePos.Y < 35) && (mousePos.Y > pbGameBoxCover.Location.Y))
            {
                if (appInfo.Enabled)
                    toolMsg = "Auto Surround Enabled";
                else
                    toolMsg = "Auto Surround Disabled";
                toolTip.Show(toolMsg, this, mousePos, 1000);
            }
        }
    }
}
