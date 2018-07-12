using System;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;
using NLog;
using NLog.Config;

namespace NVidia_Surround_Assistant
{
    public partial class Settings : Form
    {
        //Load all settings to local class
        LoggingConfiguration loggingConfiguration = new LoggingConfiguration();
        
        bool StartOnStartup = NVidia_Surround_Assistant.Properties.Settings.Default.StartOnStartup;
        bool StartMinimized = NVidia_Surround_Assistant.Properties.Settings.Default.StartMinimized;
        bool CloseToTray = NVidia_Surround_Assistant.Properties.Settings.Default.CloseToTray;
        bool SaveWindowPositions = NVidia_Surround_Assistant.Properties.Settings.Default.SaveWindowPositions;
        bool ShowLogs = NVidia_Surround_Assistant.Properties.Settings.Default.ShowLogs;
        int SurroundToNormal_OnClose = NVidia_Surround_Assistant.Properties.Settings.Default.SurroundToNormal_OnClose;
        int SurroundToNormal_OnExit = NVidia_Surround_Assistant.Properties.Settings.Default.SurroundToNormal_OnExit;
        int configLogLevel = NVidia_Surround_Assistant.Properties.Settings.Default.LogLevel;

        bool settingsNotSaved = false;

        public Settings()
        {
            InitializeComponent();            

            if (StartMinimized)
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
            comboBoxLogLevel.SelectedIndex = configLogLevel;
        }       
        
        void CreateTask()
        {
            string fileName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            // Get the service on the local machine
            using (TaskService ts = new TaskService())
            {
                // Create a new task definition and assign properties
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "Start NVidia Surround Assistant at Login with admin privileges";
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

        private void UpdateLogRules(LogLevel logLevel)
        {
            for (int i = 0; i < LogManager.Configuration.LoggingRules.Count; i++)
            {
                if (LogManager.Configuration.LoggingRules[i].IsLoggingEnabledForLevel(logLevel))
                    LogManager.Configuration.LoggingRules[i].DisableLoggingForLevel(logLevel);
                else
                    LogManager.Configuration.LoggingRules[i].EnableLoggingForLevel(logLevel);
            }            
        }

        private void SaveSettings()
        {
            NVidia_Surround_Assistant.Properties.Settings.Default.StartOnStartup = StartOnStartup;
            NVidia_Surround_Assistant.Properties.Settings.Default.StartMinimized = StartMinimized;
            NVidia_Surround_Assistant.Properties.Settings.Default.CloseToTray = CloseToTray;
            NVidia_Surround_Assistant.Properties.Settings.Default.SaveWindowPositions = SaveWindowPositions;
            NVidia_Surround_Assistant.Properties.Settings.Default.ShowLogs = ShowLogs;
            NVidia_Surround_Assistant.Properties.Settings.Default.SurroundToNormal_OnClose = SurroundToNormal_OnClose;
            NVidia_Surround_Assistant.Properties.Settings.Default.SurroundToNormal_OnExit = SurroundToNormal_OnExit;
            NVidia_Surround_Assistant.Properties.Settings.Default.LogLevel = configLogLevel;

            //Save settings
            NVidia_Surround_Assistant.Properties.Settings.Default.Save();
        }

        private void pictureBoxApply_Click(object sender, EventArgs e)
        {
            SaveSettings();
            MainForm.logger.Debug("Settings: Saved");

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

            MainForm.logger.Info("Settings: Minimize on start-up.");
        }

        private void pictureBoxStartMax_Click(object sender, EventArgs e)
        {
            pictureBoxStartMin.Visible = true;
            pictureBoxStartMax.Visible = false;
            StartMinimized = true;
            settingsNotSaved = true;

            MainForm.logger.Info("Settings: Maximize on start-up.");
        }

        private void pictureBoxCloseToTray_Click(object sender, EventArgs e)
        {
            pictureBoxCloseToTray.Visible = false;
            pictureBoxCloseOnClose.Visible = true;
            CloseToTray = false;
            settingsNotSaved = true;

            MainForm.logger.Info("Settings: Close to tray.");
        }

        private void pictureBoxCloseOnClose_Click(object sender, EventArgs e)
        {
            pictureBoxCloseToTray.Visible = true;
            pictureBoxCloseOnClose.Visible = false;
            CloseToTray = true;
            settingsNotSaved = true;

            MainForm.logger.Info("Settings: Close normally.");
        }

        private void pictureBoxSaveWindowPositions_Yes_Click(object sender, EventArgs e)
        {
            pictureBoxSaveWindowPositions_Yes.Visible = false;
            pictureBoxSaveWindowPositions_No.Visible = true;
            SaveWindowPositions = false;
            settingsNotSaved = true;

            MainForm.logger.Info("Settings: Save Window positions on surround switch.");
        }

        private void pictureBoxSaveWindowPositions_No_Click(object sender, EventArgs e)
        {
            pictureBoxSaveWindowPositions_Yes.Visible = true;
            pictureBoxSaveWindowPositions_No.Visible = false;
            SaveWindowPositions = true;
            settingsNotSaved = true;

            MainForm.logger.Info("Settings: Discard window positions.");
        }

        private void pictureBoxStartOnStartup_Yes_Click(object sender, EventArgs e)
        {
            pictureBoxStartOnStartup_Yes.Visible = false;
            pictureBoxStartOnStartup_No.Visible = true;
            StartOnStartup = false;
            DeleteTask();

            MainForm.logger.Info("Settings: Start with windows.");
        }

        private void pictureBoxStartOnStartup_No_Click(object sender, EventArgs e)
        {
            pictureBoxStartOnStartup_Yes.Visible = true;
            pictureBoxStartOnStartup_No.Visible = false;
            StartOnStartup = true;
            CreateTask();

            MainForm.logger.Info("Settings: Start manually.");
        }        

        private void comboBoxSurroundToNormal_OnClose_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SurroundToNormal_OnClose != comboBoxSurroundToNormal_OnClose.SelectedIndex)
            {
                SurroundToNormal_OnClose = comboBoxSurroundToNormal_OnClose.SelectedIndex;
                settingsNotSaved = true;
            }

            switch(SurroundToNormal_OnClose)
            {
            case 0:
                MainForm.logger.Info("Settings: Always switch to normal on NVSA exit.");
                break;
            case 1:
                MainForm.logger.Info("Settings: Ask switch to normal on NVSA exit.");
                break;
            case 2:
                MainForm.logger.Info("Settings: Never switch to normal on NVSA exit.");
                break;
            default:
                MainForm.logger.Info("Settings: Unknown selection for switch to normal on NVSA exit.");
                break;
            }            
        }

        private void comboBoxSurroundToNormal_OnExit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SurroundToNormal_OnExit != comboBoxSurroundToNormal_OnExit.SelectedIndex)
            {
                SurroundToNormal_OnExit = comboBoxSurroundToNormal_OnExit.SelectedIndex;
                settingsNotSaved = true;
            }

            switch (SurroundToNormal_OnClose)
            {
                case 0:
                    MainForm.logger.Info("Settings: Always switch to normal on application exit.");
                    break;
                case 1:
                    MainForm.logger.Info("Settings: Ask switch to normal on application exit.");
                    break;
                case 2:
                    MainForm.logger.Info("Settings: Never switch to normal on application exit.");
                    break;
                default:
                    MainForm.logger.Info("Settings: Unknown selection for switch to normal on application exit.");
                    break;
            }
        }

        private void Settings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(DialogResult == DialogResult.Cancel && settingsNotSaved)
            {
                if(MessageBox.Show("Would you like to save your changed settings?", "Unsaved Settings", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    SaveSettings();
                }
            }
        }

        private void pictureBoxShowLogs_Yes_Click(object sender, EventArgs e)
        {
            pictureBoxShowLogs_Yes.Visible = false;
            pictureBoxShowLogs_No.Visible = true;
            ShowLogs = false;

            MainForm.logger.Info("Settings: Show logs window.");
        }

        private void pictureBoxShowLogs_No_Click(object sender, EventArgs e)
        {
            pictureBoxShowLogs_Yes.Visible = true;
            pictureBoxShowLogs_No.Visible = false;
            ShowLogs = true;

            MainForm.logger.Info("Settings: Hide logs window.");
        }

        /// <summary>
        /// Reconfigures the NLog logging level.
        /// </summary>
        /// <param name="level">The <see cref="LogLevel" /> to be set.</param>
        private static void SetNlogLogLevel(LogLevel level)
        {
            if (level == LogLevel.Off)
            {
                LogManager.DisableLogging();
            }
            else
            {
                if (!LogManager.IsLoggingEnabled())
                {
                    LogManager.EnableLogging();
                }

                foreach (var rule in LogManager.Configuration.LoggingRules)
                {
                    // Iterate over all levels up to and including the target, (re)enabling them.
                    for (int i = level.Ordinal; i <= 5; i++)
                    {
                        rule.EnableLoggingForLevel(LogLevel.FromOrdinal(i));
                    }
                }
            }

            LogManager.ReconfigExistingLoggers();
        }        

        private void comboBoxLogLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            LogLevel logLevel;
            switch (comboBoxLogLevel.SelectedIndex)
            {
                case 0:
                    logLevel = LogLevel.Off;
                    SetNlogLogLevel(LogLevel.Off);
                    break;
                case 1:
                    logLevel = LogLevel.Trace;
                    SetNlogLogLevel(LogLevel.Trace);
                    break;
                case 2:
                    logLevel = LogLevel.Debug;
                    SetNlogLogLevel(LogLevel.Debug);
                    break;
                case 3:
                    logLevel = LogLevel.Info;
                    SetNlogLogLevel(LogLevel.Info);
                    break;
                case 4:
                    logLevel = LogLevel.Error;
                    SetNlogLogLevel(LogLevel.Error);
                    break;
                case 5:
                    logLevel = LogLevel.Fatal;
                    SetNlogLogLevel(LogLevel.Fatal);
                    break;
                default:
                    logLevel = LogLevel.Info;
                    break;
            }
            configLogLevel = comboBoxLogLevel.SelectedIndex;
            UpdateLogRules(logLevel);
            LogManager.Configuration.Reload();

        }

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            pictureBox.BackColor = MainForm.hoverButtonColor;
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            pictureBox.BackColor = MainForm.normalControlColor;
        }
    }
}
