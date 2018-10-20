using System;
using NLog;
using Display_Manager;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using MyStuff;

namespace NVidia_Surround_Assistant
{    
    /// <summary>
    /// Surround manager is a C# wrapper for the c++/cli dll that controls the NVidia API
    /// </summary>
    public class SurroundManager
    {
        public bool surroundSetupLoaded = false;
        private  bool initConfig = false;
        Surround_Manager mySurround = new Surround_Manager();

        SurroundConfig defaultConfig;
        SurroundConfig defaultSurroundConfig;

        public SurroundManager()
        {
        }

        ~SurroundManager()
        {
            //free all surround resources
            mySurround.Dispose();
        }

        public bool SM_Initialize()
        {
            bool result = false;
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                result = true;
            }
            catch (DisplayManager_Exception ex)
            {
                MessageBox.Show(ex.Message, "NVAPI Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.logger.Error("DM: {0}", ex.Message);
                result = false;
            }

            return result;
        }

        public bool SM_DoInitialSetup()
        {
            bool skipSurround = false;
            bool skipDefault = false;

            initConfig = true;
            //Save current display setup for re-application later
            SM_SaveCurrentSetup();
            SM_SaveWindowPositions();

            MyMessageBox.Show("The setup will ask to save two surround display configurations.\n" +
                "    \u2022 The default configuration stores the monitor setup that is used when NVidia surround is disabled.\n" +
                "    \u2022 The default surround configuration stores the monitor setup that is used when NVidia surround is\n" +
                "      enabled.\n" +
                "    \u2022 Each application can have it's own custom surround configuration. The default surround configuration\n" +
                "      will automatically be selected when adding a new application to the detection list.\n" +
                "    \u2022 The custom configurations can be added after setup under settings.", "Setup");
            
            //Check if surround setup file already exists
            if (MainForm.sqlInterface.SurroundConfigExists("Default Surround"))
            {
                if (MessageBox.Show("Default Surround Setup found. Overwrite it?", "Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    MainForm.sqlInterface.DeleteSurroundConfig("Default Surround");
                else
                    skipSurround = true;
            }

            //Check if surround setup file already exists
            if (MainForm.sqlInterface.SurroundConfigExists("Default"))
            {
                if (MessageBox.Show("Default Setup found. Overwrite it?", "Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    MainForm.sqlInterface.DeleteSurroundConfig("Default");
                else
                    skipDefault = true;
            }

            if (!skipDefault)
            {
                if (MessageBox.Show("The Default Configuration will be used as your non-surround configuration.\nWould you like to save the current display configuration as your default non-surround configuration?", "Setup", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    if (SM_IsSurroundActive())
                    {
                        if (MessageBox.Show("NVidia Surround mode is currently active. If this is not your intention, then please disable NVidia Surround via NVidia control panel(or keyboard shortcuts) now.\n\nWhen the displays are setup to your liking for the default configuration, press OK", "Default Display Setup", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                            skipDefault = true;
                    }
                    //Save memory to file
                    if(!skipDefault)
                        SM_SaveDefaultSetup();
                }
                else
                {
                    MyMessageBox.Show("Default setup not saved. Certain functionality will not work until application is restarted or setup is run again");
                    MainForm.logger.Info("DM: Default setup not saved. Certain functionality will not work until application is restarted or setup is run again");
                    initConfig = false;
                    return false;
                }
            }

            if (!skipSurround)
            {
                if (MessageBox.Show("The Default Surround Configuration will be used as your surround configuration.\nWould you like to save the current display configuration as your default surround configuration?", "Setup", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    if (!SM_IsSurroundActive())
                    {
                        if (MessageBox.Show("NVidia Surround mode is currently not active. If this is not your intention, then please enable NVidia Surround via NVidia control panel(or keyboard shortcuts) now.\n\nWhen the displays are setup to your liking, press OK", "Default Display Setup", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) == DialogResult.Cancel)
                            skipSurround = true;
                    }
                    //Save memory to file
                    if (!skipSurround)
                        SM_SaveDefaultSurroundSetup();
                }
                else
                {
                    MyMessageBox.Show("Default surround setup not saved. Certain functionality will not work until application is restarted");
                    MainForm.logger.Info("DM: Default surround setup not saved. Certain functionality will not work until application is restarted or setup is run again");
                    initConfig = false;
                    return false;
                }
            }
            SM_ReadDefaultSurroundConfig();
            //Apply saved config. Display manager will not switch if there is no difference to grid setup
            SM_ApplySetupFromMemory(false);
            SM_ApplyWindowPositions();

            MyMessageBox.Show("Surround Setup Complete");

            initConfig = false;

            return true;
        }

        /// <summary>
        /// Initialize NVApi and load the default configs. If there are no configs create them
        /// </summary>
        /// <returns></returns>
        public bool SM_ReadDefaultSurroundConfig()
        {
            try
            {
                defaultConfig = MainForm.sqlInterface.GetSurroundConfig("Default");
                defaultSurroundConfig = MainForm.sqlInterface.GetSurroundConfig("Default Surround");

                if(defaultConfig == null || defaultSurroundConfig == null)
                {
                    if (!SM_DoInitialSetup())
                    {
                        surroundSetupLoaded = false;
                        return surroundSetupLoaded;
                    }
                    defaultConfig = MainForm.sqlInterface.GetSurroundConfig("Default");
                    defaultSurroundConfig = MainForm.sqlInterface.GetSurroundConfig("Default Surround");                    
                }
                mySurround.LoadSetup(false, defaultConfig.Config);
                mySurround.LoadSetup(true, defaultSurroundConfig.Config);
                surroundSetupLoaded = true;
            }
            catch (DisplayManager_Exception ex)
            {
                MainForm.logger.Error("DM: {0}", ex.Message);
            }
            return surroundSetupLoaded;
        }

        public bool SM_ApplySetupFromMemory(bool Surround)
        {
            bool result = false;
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                if (Surround && !surroundSetupLoaded)
                    SM_ReadDefaultSurroundConfig();
                if (Surround && NVidia_Surround_Assistant.Properties.Settings.Default.SaveWindowPositions)
                    SM_SaveWindowPositions();

                mySurround.ApplySetup(Surround);
                if (!Surround && NVidia_Surround_Assistant.Properties.Settings.Default.SaveWindowPositions)
                    SM_ApplyWindowPositions();

                result = true;
            }
            catch (DisplayManager_Exception ex)
            {                
                MainForm.logger.Error("DM: {0}", ex.Message);
            }
            return result;
        }

        public static void DelayAction(int millisecond, Action action)
        {
            var timer = new System.Windows.Forms.Timer();
            timer.Tick += delegate
            {
                action.Invoke();
                timer.Stop();
            };

            timer.Interval = millisecond;
            timer.Start();
        }

        public bool SM_ApplySetup(int setupId)
        {
            bool result = false;                       
            
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                if (NVidia_Surround_Assistant.Properties.Settings.Default.SaveWindowPositions)
                    SM_SaveWindowPositions();
                SurroundConfig config = MainForm.sqlInterface.GetSurroundConfig(setupId);
                if (config != null)
                {
                    mySurround.ApplySetup(config.Config);
                    result = true;
                }
                else
                {
                    MainForm.logger.Info("DM: Application with id {0}, not in list", setupId);                    
                }
            }
            catch (DisplayManager_Exception ex)
            {                
                MainForm.logger.Error("DM: {0}", ex.Message);
            }
            
            return result;
        }

        //Save current setup to memory
        public bool SM_SaveCurrentSetup()
        {
            bool result = false;
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                if (mySurround.IsSurroundActive())
                {
                    mySurround.SaveSetupToMemory(true);
                    MainForm.logger.Info("DM: Saving Surround to memory successful");
                }
                else
                {
                    mySurround.SaveSetupToMemory(false);
                    MainForm.logger.Info("DM: Saving Non-Surround to memory successful");
                }
                result = true;
            }
            catch (DisplayManager_Exception ex)
            {                
                MainForm.logger.Error("DM: Saving to memory Error: {0}", ex.Message);                
            }
            return result;
        }

        //Save current setup to db
        public bool SM_SaveCurrentSetup(string configName)
        {
            bool result = false;
            SurroundConfig surroundConfig = new SurroundConfig();
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                surroundConfig.Name = configName;
                surroundConfig.Config = mySurround.SaveSetup();
                if (surroundConfig.Config != null)
                {
                    MainForm.sqlInterface.SetSurroundConfig(surroundConfig);
                    MainForm.logger.Info("DM: Saving to file successful");
                    result = true;
                }
            }
            catch (DisplayManager_Exception ex)
            {
                MessageBox.Show("Display Manager Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.logger.Error("DM: Saving to file Error: {0}", ex.Message);
            }
            return result;
        }

        public bool SM_SaveCurrentSetup(string configName, int id)
        {
            bool result = false;
            SurroundConfig surroundConfig = new SurroundConfig();
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                surroundConfig.Id = id;
                surroundConfig.Name = configName;
                surroundConfig.Config = mySurround.SaveSetup();
                if (surroundConfig.Config != null)
                {
                    MainForm.sqlInterface.SetSurroundConfig(surroundConfig);
                    MainForm.logger.Info("DM: Saving to file successful");
                    result = true;
                }
            }
            catch (DisplayManager_Exception ex)
            {
                MessageBox.Show("Display Manager Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.logger.Error("DM: Saving to file Error: {0}", ex.Message);
            }
            return result;
        }

        //Save current setup to db
        public bool SM_SaveDefaultSetup()
        {
            bool result = false;
            SurroundConfig surroundConfig = new SurroundConfig();
            try
            {
                if (!initConfig)
                {
                    result = SM_SaveCurrentSetup("Default", defaultConfig.Id);
                    SM_ReadDefaultSurroundConfig();
                }
                else
                    result = SM_SaveCurrentSetup("Default", 0);                
            }
            catch (DisplayManager_Exception ex)
            {
                MessageBox.Show("Display Manager Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.logger.Error("DM: Saving to file Error: {0}", ex.Message);
            }
            return result;
        }

        //Save current setup to db
        public bool SM_SaveDefaultSurroundSetup()
        {
            bool result = false;
            SurroundConfig surroundConfig = new SurroundConfig();
            try
            {
                if (!initConfig)
                {
                    result = SM_SaveCurrentSetup("Default Surround", defaultSurroundConfig.Id);
                    SM_ReadDefaultSurroundConfig();
                }
                else
                    result = SM_SaveCurrentSetup("Default Surround", 1);
            }
            catch (DisplayManager_Exception ex)
            {
                MessageBox.Show("Display Manager Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.logger.Error("DM: Saving to file Error: {0}", ex.Message);
            }
            return result;
        }

        //Save current setup to file
        public bool SM_SaveWindowPositions()
        {
            bool result = false;
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                if (!mySurround.IsSurroundActive())
                {
                    mySurround.SaveWindowPositions();
                    mySurround.MinimizeAllWindows();
                    Thread.Sleep(100);
                    MainForm.logger.Info("DM: Window Positions saved successfully");
                    result = true;
                }
            }
            catch (DisplayManager_Exception ex)
            {
                MainForm.logger.Error("DM: Window Positions saved Error: {0}", ex.Message);
            }
            return result;
        }

        //Save current setup to file
        public bool SM_ApplyWindowPositions()
        {
            bool result = false;
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                mySurround.MinimizeAllWindows();
                mySurround.ApplyWindowPositions();
                MainForm.logger.Info("DM: Window Positions applied successfully");
                result = true;
            }
            catch (DisplayManager_Exception ex)
            {
                MainForm.logger.Error("DM: Window Positions apply Error: {0}", ex.Message);                
            }
            return result;
        }

        public bool SM_IsSurroundActive()
        {
            bool result = false;
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                if (!mySurround.surroundEnabled)
                    result = mySurround.IsSurroundActive();
                else
                    result = mySurround.surroundEnabled;
            }
            catch (DisplayManager_Exception ex)
            {
                MainForm.logger.Error("DM: IsSurroundActive Error: {0}", ex.Message);               
            }
            return result;
        }

        public bool SM_IsSurroundActive(int configID)
        {
            bool result = false;
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                SurroundConfig config = MainForm.sqlInterface.GetSurroundConfig(configID);
                if(config != null)
                    result = mySurround.IsSurroundActive(config.Config);                
            }
            catch (DisplayManager_Exception ex)
            {
                MainForm.logger.Error("DM: IsSurroundActive Error: {0}", ex.Message);                
            }
            return result;
        }

        public void SM_SwitchSurround()
        {
            if (SM_IsSurroundActive())
            {
                SM_ApplySetupFromMemory(false);
            }
            else
            {
                SM_ApplySetupFromMemory(true);
            }
        }
    }
}
