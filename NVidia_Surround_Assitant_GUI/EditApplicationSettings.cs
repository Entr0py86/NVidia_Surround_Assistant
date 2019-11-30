using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;
using NLog;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using MyStuff;
using IGDB;
using IGDB.Models;

namespace NVidia_Surround_Assistant
{
    public partial class EditApplicationSettings : Form
    {
        //gameList created from game database API
        List<ApplicationInfo> gameList_UserChoice = new List<ApplicationInfo>();
        
        public ApplicationInfo AppInfo;
                
        //Timer variables
        int timerTickCount = 0;

        int spacing = 40;
        int y_spacing;

        //Application Database API  interfaces
        IGDBApi igdbClient = Client.Create("0638759164b00165dcae1fafbe681834");

        //New app flag
        bool autoSearchNewApp;
        bool settingsNotSaved = false;

        public EditApplicationSettings(ApplicationInfo Appinfo, bool AutoSearchNewApp)
        {
            InitializeComponent();

            if (gameList_UserChoice.Count == 0)
            {
                AppInfo = Appinfo;
                gameList_UserChoice.Add(Appinfo);
                //Update all textboxes
                textBoxGameSearch.Text = Appinfo.DisplayName;
            }            

            autoSearchNewApp = AutoSearchNewApp;

            //Get nvsa file names
            PopulateGridComboBoxes();
            if (!autoSearchNewApp)
            {
                pictureBoxDisabled.Visible = !AppInfo.Enabled;
                pictureBoxEnabled.Visible = AppInfo.Enabled;
            }
        }

        private void UpdateDisplay(ApplicationInfo Appinfo)
        {
            if (Appinfo != null)
            {
                if (Appinfo.DisplayName != null)
                    textBoxDisplayName.Text = Appinfo.DisplayName;
                if (Appinfo.FullPath != null)
                    textBoxAppPath.Text = Appinfo.FullPath;

                if (Appinfo.Image != null)
                    pictureBoxGameBoxCover.Image = Appinfo.Image;
                else
                    pictureBoxGameBoxCover.Image = NVidia_Surround_Assistant.Properties.Resources.delete_48x48;

                pictureBoxNotPauseOnDetect.Visible = !Appinfo.PauseOnDetect;
                pictureBoxPauseOnDetect.Visible = Appinfo.PauseOnDetect;

                if (!autoSearchNewApp)
                {
                    numericUpDownSwitchbackTimeout.Value = Appinfo.SwitchbackTimeout;
                    numericUpDownWaitStart.Value = Appinfo.StartTimeout;
                }
                else
                {
                    numericUpDownWaitStart.Value = NVidia_Surround_Assistant.Properties.Settings.Default.WaitForStartTimeout;
                    numericUpDownSwitchbackTimeout.Value = NVidia_Surround_Assistant.Properties.Settings.Default.SwitchbackTImeout;
                }
                settingsNotSaved = false;
            }
        }

        private void ToggleWait(bool waitDisabled)
        {
            //show wait label
            labelWait.Visible = !waitDisabled;
            //set the timer to the correct state
            timerWait.Enabled = !waitDisabled;

            labelHttpFoundApp.Visible = !waitDisabled;

            //Hide rest of the controls
            label2.Visible = waitDisabled;
            label3.Visible = waitDisabled;
            label4.Visible = waitDisabled;
            label5.Visible = waitDisabled;
            label8.Visible = waitDisabled;
            label9.Visible = waitDisabled;            

            pictureBoxDeleteImage.Visible = waitDisabled;
            pictureBoxEditImage.Visible = waitDisabled;
            pictureBoxDisabled.Visible = waitDisabled && !AppInfo.Enabled;
            pictureBoxEnabled.Visible = waitDisabled && AppInfo.Enabled;
            pictureBoxGameBoxCover.Visible = waitDisabled;
            pictureBoxChangeFileLocation.Visible = waitDisabled;
            pictureBoxNotPauseOnDetect.Visible = waitDisabled && !AppInfo.PauseOnDetect;
            pictureBoxPauseOnDetect.Visible = waitDisabled && AppInfo.PauseOnDetect;
            comboBoxSurroundSetup.Visible = waitDisabled;

            textBoxAppPath.Visible = waitDisabled;
            numericUpDownSwitchbackTimeout.Visible = waitDisabled;
            textBoxDisplayName.Visible = waitDisabled;

            if (waitDisabled)
            {                
                pictureBoxApply.Image = NVidia_Surround_Assistant.Properties.Resources.success_green_24x24;
                pictureBoxApply.Location = new Point(pictureBoxApply.Left, 332);
                pictureBoxCancel.Location = new Point(pictureBoxCancel.Left, 332);

                Width = pictureBoxGameBoxCover.Right + spacing;
                Height = pictureBoxGameBoxCover.Bottom + y_spacing;
            }
            else
            {               
                pictureBoxApply.Image = NVidia_Surround_Assistant.Properties.Resources.success_24x24;
                pictureBoxApply.Location = new Point(pictureBoxApply.Left, labelWait.Bottom + spacing);
                pictureBoxCancel.Location = new Point(pictureBoxCancel.Left, labelWait.Bottom + spacing);

                Width = pictureBoxSearch.Right + spacing;
                Height = pictureBoxCancel.Bottom + y_spacing;
            }          
        }

        #region APIinterface
        private async void GetGameList()
        {
            if (gameList_UserChoice.Count > 0)
            {
                gameList_UserChoice.Clear();
                comboBoxGameList.DataSource = null;
                comboBoxGameList.Items.Clear();
            }
            pictureBoxSearch.Enabled = false;
            ToggleWait(false);
            await IGDB_GetGameList();

            comboBoxGameList.SuspendLayout();            
            comboBoxGameList.SelectedIndexChanged -= comboBoxGameList_SelectedIndexChanged;
            comboBoxGameList.DataSource = gameList_UserChoice;
            comboBoxGameList.DisplayMember = "DisplayName";            
            comboBoxGameList.SelectedIndexChanged += comboBoxGameList_SelectedIndexChanged;
            comboBoxGameList.Visible = true;
            labelGameList.Visible = true;
            comboBoxGameList.ResumeLayout();
            if (comboBoxGameList.Items.Count > 0)
            {
                comboBoxGameList.SelectedIndex = 0;
                UpdateDisplay((ApplicationInfo)comboBoxGameList.SelectedItem);
            }            
            ToggleWait(true);
            pictureBoxSearch.Enabled = true;
        }
        #endregion

        #region http
        private string GetHttpPage(string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        private async Task<Bitmap> GetHttpImg(string uri)
        {
            Bitmap cover;
            try
            {   
                if(uri.StartsWith("//"))
                {
                    uri = "https:" + uri;
                }
                uri = uri.Replace("t_thumb", "t_cover_big");

                using (WebClient client = new WebClient())
                {
                    using (Stream stream = await client.OpenReadTaskAsync(new Uri(uri)))
                    {
                        if (stream != null)
                        {
                            cover = new Bitmap(Bitmap.FromStream(stream));
                            //compress image if required
                            if (CheckImageSizeBytes(cover, 20971520))
                            {
                                return ResizeImage(cover, pictureBoxGameBoxCover.ClientSize.Width, pictureBoxGameBoxCover.ClientSize.Height);
                            }
                            else
                                return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if(MainForm.logger != null)
                    MainForm.logger.Debug("Edit Application: {0} | URI: {1}", ex.Message, uri);
            }
            return null;
        }
        #endregion
                
        #region IGDB
        private async Task IGDB_GetGameList()
        {
            try
            {
                labelHttpFoundApp.Text = String.Format("Searching IGDN for {0}", textBoxGameSearch.Text);

                // Search[] gameList = await igdbClient.QueryAsync<Search>(Client.Endpoints.Search, query: String.Format("fields *; search \"{0}\"; limit 30;", textBoxGameSearch.Text));
                Game[] gameList = await igdbClient.QueryAsync<Game>(Client.Endpoints.Games, query: String.Format("search \"{0}\"; limit 30; fields name,cover,cover.*;", textBoxGameSearch.Text));
                ApplicationInfo appInfo;

                if (gameList != null)
                {
                    foreach (Game game in gameList)
                    {
                        if (MainForm.logger != null)
                            MainForm.logger.Info("Game found: {0}", game.Name);
                        appInfo = new ApplicationInfo
                        {
                            DisplayName = game.Name,
                        };
                        labelHttpFoundApp.Text = String.Format("Fetching Image for {0}", game.Name);

                        if (game.Cover != null)
                        {
                            try
                            {
                                appInfo.Image = await GetHttpImg(game.Cover.Value.Url);
                            }
                            catch (System.NullReferenceException)
                            {
                                appInfo.Image = null;
                            }
                        }
                        if (appInfo != null)
                            gameList_UserChoice.Add(appInfo);
                    }
                }
            }
            catch (System.Net.Http.HttpRequestException ex)
            {
                MainForm.logger.Info("Application Settings: {0}", ex.Message);
            }
        }

        #endregion

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private bool CheckImageSizeBytes(Bitmap img, long sizeRequired)
        {
            using (var mss = new MemoryStream())
            {
                img.Save(mss, ImageFormat.Png);
                if (mss.Length > sizeRequired)
                    return false;
                else
                    return true;
            }                       
        }

        private void timerWait_Tick(object sender, EventArgs e)
        {
            switch(timerTickCount % 4)
            {
                case 0:
                    labelWait.Text = ".";
                    break;
                case 1:
                    labelWait.Text = "..";
                    break;
                case 2:
                    labelWait.Text = "...";
                    break;
                default:
                    labelWait.Text = "";
                    break;
            }
            timerTickCount++;
        }

        private void EditApplicationSettings_Load(object sender, EventArgs e)
        {
            y_spacing = Height - pictureBoxGameBoxCover.Bottom;
            UpdateDisplay(AppInfo);
            //Populate the game list
            if (autoSearchNewApp || gameList_UserChoice.Count == 0)
            {
                GetGameList();
            }
        }

        private void pictureBoxSearch_Click(object sender, EventArgs e)
        {            
            GetGameList();
        }

        private void comboBoxGameList_SelectedIndexChanged(object sender, EventArgs e)
        {            
            UpdateDisplay((ApplicationInfo)comboBoxGameList.SelectedItem);
            settingsNotSaved = true;
        }

        private void pictureBoxEditImage_Click(object sender, EventArgs e)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            string sep = string.Empty;

            openFileDialog1.InitialDirectory = Path.GetPathRoot(Application.ExecutablePath);
            openFileDialog1.Filter = "";
            foreach (var c in codecs)
            {
                string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                openFileDialog1.Filter = String.Format("{0}{1}{2} ({3})|{3}", openFileDialog1.Filter, sep, codecName, c.FilenameExtension);
                sep = "|";
            }

            openFileDialog1.Filter = String.Format("{0}{1}{2} ({3})|{3}", openFileDialog1.Filter, sep, "All Files", "*.*");

            openFileDialog1.DefaultExt = ".Png"; // Default file extension 

            // Show open file dialog box 
            // Process open file dialog box results 
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Open document 
                    Image newImage = Image.FromFile(openFileDialog1.FileName);
                    if (CheckImageSizeBytes((Bitmap)newImage, 20971520))
                        pictureBoxGameBoxCover.Image = newImage;
                    else
                        MessageBox.Show("Image is to large for database. Limit is 20MB.", "Image Size Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    settingsNotSaved = true;
                }
                catch(OutOfMemoryException ex)
                {
                    MyMessageBox.Show("Image size to large");
                    MainForm.logger.Info("Edit Application: {0}", ex.Message);
                }
            }
        }

        private void pictureBoxChangeFileLocation_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Executable (*.exe)|*.exe| All files (*.*)|*.*";
            openFileDialog1.FileName = "";
            openFileDialog1.InitialDirectory = Path.GetDirectoryName(textBoxAppPath.Text);
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxAppPath.Text = openFileDialog1.FileName;                
            }            
        }

        private void pictureBoxDisabled_Click(object sender, EventArgs e)
        {
            AppInfo.Enabled = true;
            pictureBoxDisabled.Visible = !AppInfo.Enabled;
            pictureBoxEnabled.Visible = AppInfo.Enabled;
        }

        private void pictureBoxEnabled_Click(object sender, EventArgs e)
        {
            AppInfo.Enabled = false;
            pictureBoxDisabled.Visible = !AppInfo.Enabled;
            pictureBoxEnabled.Visible = AppInfo.Enabled;
        }

        private void pictureBoxNotPauseOnDetect_Click(object sender, EventArgs e)
        {
            AppInfo.PauseOnDetect = true;
            pictureBoxNotPauseOnDetect.Visible = !AppInfo.PauseOnDetect;
            pictureBoxPauseOnDetect.Visible = AppInfo.PauseOnDetect;
        }

        private void pictureBoxPauseOnDetect_Click(object sender, EventArgs e)
        {
            AppInfo.PauseOnDetect = false;
            pictureBoxNotPauseOnDetect.Visible = !AppInfo.PauseOnDetect;
            pictureBoxPauseOnDetect.Visible = AppInfo.PauseOnDetect;
        }

        private void pictureBoxApply_Click(object sender, EventArgs e)
        {
            //Copy values 
            AppInfo.Image = (Bitmap)pictureBoxGameBoxCover.Image;
            AppInfo.DisplayName = textBoxDisplayName.Text;
            AppInfo.FullPath = textBoxAppPath.Text;
            AppInfo.ProcessName = Path.GetFileNameWithoutExtension(textBoxAppPath.Text);
            AppInfo.SwitchbackTimeout = (int)numericUpDownSwitchbackTimeout.Value;
            AppInfo.StartTimeout = (int)numericUpDownWaitStart.Value;

            AppInfo.SurroundGrid = (int)comboBoxSurroundSetup.SelectedValue;

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void pictureBoxCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void PopulateGridComboBoxes()
        {
            if (comboBoxSurroundSetup.DataSource != null)
            {
                comboBoxSurroundSetup.DataSource = null;
                comboBoxSurroundSetup.Items.Clear();
            }
            comboBoxSurroundSetup.ValueMember = "id";
            comboBoxSurroundSetup.DisplayMember = "Name";
            comboBoxSurroundSetup.DataSource = MainForm.sqlInterface.GetSurroundConfigList();

            if (!autoSearchNewApp)
            {
                //Find item id
                foreach(SurroundConfig item in comboBoxSurroundSetup.Items)
                {
                    if (item.Id.Equals(AppInfo.SurroundGrid))
                    {
                        comboBoxSurroundSetup.SelectedItem = item;
                    }
                }
            }
            else
            {
                comboBoxSurroundSetup.SelectedIndex = comboBoxSurroundSetup.FindStringExact("Default Surround");
            }
        }

        private void textBoxGameSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                pictureBoxSearch_Click(null, null);
        }

        private void textBoxDisplayName_TextChanged(object sender, EventArgs e)
        {
            settingsNotSaved = true;
        }

        private void textBoxAppPath_TextChanged(object sender, EventArgs e)
        {
            settingsNotSaved = true;
        }

        private void comboBoxSurroundSetup_SelectedIndexChanged(object sender, EventArgs e)
        {
            settingsNotSaved = true;
        }

        private void numericUpDownSwitchbackTimeout_ValueChanged(object sender, EventArgs e)
        {
            settingsNotSaved = true;
        }

        private void EditApplicationSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DialogResult == DialogResult.Cancel && settingsNotSaved)
            {
                if (MessageBox.Show("Would you like to save your changed settings?", "Unsaved Settings", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DialogResult = DialogResult.OK;
                }
            }
        }
    }
}

