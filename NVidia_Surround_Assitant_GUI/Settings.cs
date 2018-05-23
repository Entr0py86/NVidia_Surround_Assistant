using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;


namespace NVidia_Surround_Assistant
{
    public partial class Settings : Form
    {
        //TODO add tooltips
        //Load all settings to local class
        bool StartOnStartup = NVidia_Surround_Assistant.Properties.Settings.Default.StartOnStartup;
        bool StartMinimized = NVidia_Surround_Assistant.Properties.Settings.Default.StartMinimized;
        bool CloseToTray = NVidia_Surround_Assistant.Properties.Settings.Default.CloseToTray;
        bool SaveWindowPositions = NVidia_Surround_Assistant.Properties.Settings.Default.SaveWindowPositions;
        bool ShowLogs = NVidia_Surround_Assistant.Properties.Settings.Default.ShowLogs;
        int SurroundToNormal_OnClose = NVidia_Surround_Assistant.Properties.Settings.Default.SurroundToNormal_OnClose;
        int SurroundToNormal_OnExit = NVidia_Surround_Assistant.Properties.Settings.Default.SurroundToNormal_OnExit;

        bool settingsNotSaved = false;

        public Settings()
        {
            InitializeComponent();

            if(StartMinimized)
            {
                pictureBoxStartMin.Visible = true;
                pictureBoxStartMax.Visible = false;
            }
            else
            {
                pictureBoxStartMin.Visible = false;
                pictureBoxStartMax.Visible = true;
            }

            if (CloseToTray)
            {
                pictureBoxCloseToTray.Visible = true;
                pictureBoxCloseOnClose.Visible = false;
            }
            else
            {
                pictureBoxCloseToTray.Visible = false;
                pictureBoxCloseOnClose.Visible = true;
            }

            if (StartOnStartup)
            {
                pictureBoxStartOnStartup_No.Visible = false;
                pictureBoxStartOnStartup_Yes.Visible = true;
            }
            else
            {
                pictureBoxStartOnStartup_No.Visible = true;
                pictureBoxStartOnStartup_Yes.Visible = false;
            }

            if (SaveWindowPositions)
            {
                pictureBoxSaveWindowPositions_No.Visible = false;
                pictureBoxSaveWindowPositions_Yes.Visible = true;
            }
            else
            {
                pictureBoxSaveWindowPositions_No.Visible = true;
                pictureBoxSaveWindowPositions_Yes.Visible = false;
            }

            if (ShowLogs)
            {
                pictureBoxShowLogs_No.Visible = false;
                pictureBoxShowLogs_Yes.Visible = true;
            }
            else
            {
                pictureBoxShowLogs_No.Visible = true;
                pictureBoxShowLogs_Yes.Visible = false;
            }

            DialogResult = DialogResult.None;
            comboBoxSurroundToNormal_OnClose.SelectedIndex = SurroundToNormal_OnClose;
            comboBoxSurroundToNormal_OnExit.SelectedIndex = SurroundToNormal_OnExit;
        }       
        
        void CreateTask()
        {
            string fileName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            // Get the service on the local machine
            using (TaskService ts = new TaskService())
            {
                // Create a new task definition and assign properties
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "Start Nvidia Surround Assitant at Logon with admin privilages";
                td.Principal.RunLevel = TaskRunLevel.Highest;
                
                // Create a trigger that will fire the task at this time every other day
                td.Triggers.Add(new LogonTrigger { Delay = new TimeSpan(0),
                                                   UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name
                });

                // Create an action that will launch Notepad whenever the trigger fires
                td.Actions.Add(new ExecAction(fileName, null, System.IO.Path.GetDirectoryName(fileName)));

                // Register the task in the root folder
                ts.RootFolder.RegisterTaskDefinition(@"Start_NVSA", td);
                settingsNotSaved = false;
                NVidia_Surround_Assistant.Properties.Settings.Default.Save();
            }
        }

        void DeleteTask()
        {
            try
            {
                using (TaskService ts = new TaskService())
                {
                    // Remove the task we just created
                    ts.RootFolder.DeleteTask("Start_NVSA");
                    settingsNotSaved = false;
                    NVidia_Surround_Assistant.Properties.Settings.Default.Save();
                }
            }
            catch(System.IO.FileNotFoundException)
            {

            }
        }

        private void pictureBoxApply_Click(object sender, EventArgs e)
        {
            NVidia_Surround_Assistant.Properties.Settings.Default.StartOnStartup = StartOnStartup;
            NVidia_Surround_Assistant.Properties.Settings.Default.StartMinimized = StartMinimized;
            NVidia_Surround_Assistant.Properties.Settings.Default.CloseToTray = CloseToTray;
            NVidia_Surround_Assistant.Properties.Settings.Default.SaveWindowPositions = SaveWindowPositions;
            NVidia_Surround_Assistant.Properties.Settings.Default.ShowLogs = ShowLogs;
            NVidia_Surround_Assistant.Properties.Settings.Default.SurroundToNormal_OnClose = SurroundToNormal_OnClose;
            NVidia_Surround_Assistant.Properties.Settings.Default.SurroundToNormal_OnExit = SurroundToNormal_OnExit;

            //Save settings
            NVidia_Surround_Assistant.Properties.Settings.Default.Save();

            settingsNotSaved = false;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void pictureBoxCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void pictureBoxStartMin_Click(object sender, EventArgs e)
        {
            pictureBoxStartMin.Visible = false;
            pictureBoxStartMax.Visible = true;
            StartMinimized = false;
            settingsNotSaved = true;
        }

        private void pictureBoxStartMax_Click(object sender, EventArgs e)
        {
            pictureBoxStartMin.Visible = true;
            pictureBoxStartMax.Visible = false;
            StartMinimized = true;
            settingsNotSaved = true;
        }

        private void pictureBoxCloseToTray_Click(object sender, EventArgs e)
        {
            pictureBoxCloseToTray.Visible = false;
            pictureBoxCloseOnClose.Visible = true;
            CloseToTray = false;
            settingsNotSaved = true;
        }

        private void pictureBoxCloseOnClose_Click(object sender, EventArgs e)
        {
            pictureBoxCloseToTray.Visible = true;
            pictureBoxCloseOnClose.Visible = false;
            CloseToTray = true;
            settingsNotSaved = true;
        }

        private void pictureBoxSaveWindowPositions_Yes_Click(object sender, EventArgs e)
        {
            pictureBoxSaveWindowPositions_Yes.Visible = false;
            pictureBoxSaveWindowPositions_No.Visible = true;
            SaveWindowPositions = false;
            settingsNotSaved = true;
        }

        private void pictureBoxSaveWindowPositions_No_Click(object sender, EventArgs e)
        {
            pictureBoxSaveWindowPositions_Yes.Visible = true;
            pictureBoxSaveWindowPositions_No.Visible = false;
            SaveWindowPositions = true;
            settingsNotSaved = true;
        }

        private void pictureBoxStartOnStartup_Yes_Click(object sender, EventArgs e)
        {
            pictureBoxStartOnStartup_Yes.Visible = false;
            pictureBoxStartOnStartup_No.Visible = true;
            StartOnStartup = false;
            DeleteTask();
        }

        private void pictureBoxStartOnStartup_No_Click(object sender, EventArgs e)
        {
            pictureBoxStartOnStartup_Yes.Visible = true;
            pictureBoxStartOnStartup_No.Visible = false;
            StartOnStartup = true;
            CreateTask();
        }        

        private void comboBoxSurroundToNormal_OnClose_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SurroundToNormal_OnClose != comboBoxSurroundToNormal_OnClose.SelectedIndex)
            {
                SurroundToNormal_OnClose = comboBoxSurroundToNormal_OnClose.SelectedIndex;
                settingsNotSaved = true;
            }
        }

        private void comboBoxSurroundToNormal_OnExit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SurroundToNormal_OnExit != comboBoxSurroundToNormal_OnExit.SelectedIndex)
            {
                SurroundToNormal_OnExit = comboBoxSurroundToNormal_OnExit.SelectedIndex;
                settingsNotSaved = true;
            }
        }

        private void pictureBoxSaveConfig_Click(object sender, EventArgs e)
        {
            MainForm.surroundManager.SM_SaveCurrentSetupToFile();
        }

        private void pictureBoxLoadConfig_Click(object sender, EventArgs e)
        {
            MainForm.surroundManager.SM_ApplySetupFromFile();
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(DialogResult == DialogResult.Cancel && settingsNotSaved)
            {
                if(MessageBox.Show("Would you like to save your chnaged settings?", "Unsaved Settings", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    NVidia_Surround_Assistant.Properties.Settings.Default.Save();
                }
            }
        }

        private void pictureBoxShowLogs_Yes_Click(object sender, EventArgs e)
        {
            pictureBoxShowLogs_Yes.Visible = false;
            pictureBoxShowLogs_No.Visible = true;
            ShowLogs = false;
        }

        private void pictureBoxShowLogs_No_Click(object sender, EventArgs e)
        {
            pictureBoxShowLogs_Yes.Visible = true;
            pictureBoxShowLogs_No.Visible = false;
            ShowLogs = true;
        }
    }
}
