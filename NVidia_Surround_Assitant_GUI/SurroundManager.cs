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
    public class SurroundManager
    {
        public bool surroundSetupLoaded = false;
        Surround_Manager mySurround = new Surround_Manager();        
        
        public SurroundManager()
        {        
        }

        ~SurroundManager()
        {
            //free all surround resources
            mySurround.Dispose();
        }

        public bool SM_DoInitialSetup()
        {
            bool skipSurround = false;
            bool skipDefault = false;

            //Save current display setup for re-application later
            SM_SaveCurrentSetup();
            SM_SaveWindowPositions();

            MyMessageBox.Show("The setup will ask to save two surround display configurations.\n" +
                "    \u2022 The default configuration stores the monitor setup that is used when NVidia surround is disabled.\n" +
                "    \u2022 The default surround configuration stores the monitor setup that is used when NVidia surround is enabled.\n" +
                "    \u2022 Each application can have it's own custom surround configuration. The default surround configuration will automatically be selected when adding a new application to the detect list.\n" +
                "    \u2022 The custom configurations can be added after setup under settings.", "Setup");
            
            //Check if surround setup file already exists
            if (MainForm.sqlInterface.SurroundConfigExists("Default Surround"))
            {
                if (MessageBox.Show("Default Surround Setup detected. Overwrite it?", "Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    MainForm.sqlInterface.DeleteSurroundConfig("Default Surround");
                else
                    skipSurround = true;
            }

            //Check if surround setup file already exists
            if (MainForm.sqlInterface.SurroundConfigExists("Default"))
            {
                if (MessageBox.Show("Default Setup detected. Overwrite it?", "Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    MainForm.sqlInterface.DeleteSurroundConfig("Default");
                else
                    skipDefault = true;
            }

            if (!skipDefault)
            {
                if (MessageBox.Show("The Default Configuration will be used as your non-surround configuration.\nWould you like to save the current display configuration as your default non-surround configuration?", "Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if (SM_IsSurroundActive())
                    {
                        MessageBox.Show("NVidia Surround mode currently active. If this is not your intention, then please disable NVidia Surround via NVidia control panel(or keyboard shortcuts) now.\n\nWhen the displays are setup to your liking, press OK", "Default Display Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    //Save memory to file
                    SM_SaveDefaultSetup();
                }
                else
                {
                    MessageBox.Show("Default setup not saved. Certain functionality will not work until application is restarted");
                    MainForm.logger.Debug("DM: Default setup not saved. Certain functionality will not work until application is restarted");
                    return false;
                }
            }

            if (!skipSurround)
            {//TODO
                if (MessageBox.Show("Save current display setup as surround setup?", "Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    //Save memory to file
                    SM_SaveDefaultSurroundSetup();                    
                }
                else
                {
                    MessageBox.Show("Default surround setup not saved.\nPlease re-run setup for proper operation of application.");
                    return false;
                }
            }
            //Apply saved config. Display manager will not switch if there is no difference to grid setup
            SM_ApplySetupFromMemory(false);
            SM_ApplyWindowPositions();           
           
            return true;
        }

        public bool SM_ReadDefaultSurroundConfig()
        {
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();

                SurroundConfig defaultConfig = MainForm.sqlInterface.GetSurroundConfig("Default");
                SurroundConfig defaultSurroundConfig = MainForm.sqlInterface.GetSurroundConfig("Default Surround");

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
                MainForm.logger.Debug("DM: {0}", ex.Message);
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
                MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                MainForm.logger.Debug("DM: {0}", ex.Message);
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
                MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                MainForm.logger.Debug("DM: {0}", ex.Message);
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
                MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                MainForm.logger.Debug("DM: Saving to memory Error: {0}", ex.Message);                
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
                MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.logger.Debug("DM: Saving to file Error: {0}", ex.Message);
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
                result = SM_SaveCurrentSetup("Default");
            }
            catch (DisplayManager_Exception ex)
            {
                MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.logger.Debug("DM: Saving to file Error: {0}", ex.Message);
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
                result = SM_SaveCurrentSetup("Default Surround");
            }
            catch (DisplayManager_Exception ex)
            {
                MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.logger.Debug("DM: Saving to file Error: {0}", ex.Message);
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
                mySurround.SaveWindowPositions();
                mySurround.MinimizeAllWindows();
                Thread.Sleep(100);
                MainForm.logger.Info("DM: Window Positions saved successfully");
                result = true;
            }
            catch (DisplayManager_Exception ex)
            {
                MainForm.logger.Debug("DM: Window Positions saved Error: {0}", ex.Message);
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
                MainForm.logger.Debug("DM: Window Positions apply Error: {0}", ex.Message);                
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
                result = mySurround.IsSurroundActive();
            }
            catch (DisplayManager_Exception ex)
            {
                MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.logger.Debug("DM: IsSurroundActive Error: {0}", ex.Message);               
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
                MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.logger.Debug("DM: IsSurroundActive Error: {0}", ex.Message);                
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
