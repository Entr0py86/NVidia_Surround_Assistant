using System;
using NLog;
using Display_Manager;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace NVidia_Surround_Assistant
{    
    public class SurroundManager
    {
        public bool surroundSetupLoaded = false;
        Surround_Manager mySurround = new Surround_Manager();
        //Logger
        Logger logger = LogManager.GetLogger("nvsaLogger");

        // saveSurroundFileDialog         
        SaveFileDialog saveSurroundFileDialog = new SaveFileDialog();        
        
        // openFileDialog
        OpenFileDialog openFileDialog = new OpenFileDialog();

        public SurroundManager()
        {
            saveSurroundFileDialog.Filter = "Surround Setup files (*.nvsa)|*.nvsa|All files (*.*)|*.*";
            openFileDialog.Filter = "Surround Setup files (*.nvsa)|*.nvsa|Executable (*.exe)|*.exe|All files (*.*)|*.*";

            surroundSetupLoaded = SM_ReadDefaultSurroundConfig();
        }

        ~SurroundManager()
        {
            //free all surround resources
            mySurround.Dispose();
        }

        public bool SM_ReadDefaultSurroundConfig()
        {
            bool result = false;
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                mySurround.LoadSetup(NVidia_Surround_Assistant.Properties.Settings.Default.SurroundSetupFileName, true);
                mySurround.LoadSetup(NVidia_Surround_Assistant.Properties.Settings.Default.DefaultSetupFileName, false);
                result = true;
            }
            catch (DisplayManager_Exception ex)
            {
                logger.Debug("DM: {0}", ex.Message);
            }
            return result;
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
                    DelayAction(500, new Action(() => { SM_ApplyWindowPositions(); }));

                result = true;
            }
            catch (DisplayManager_Exception ex)
            {
                MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                logger.Debug("DM: {0}", ex.Message);
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

        public bool SM_ApplySetupFromFile()
        {
            bool result = false;
            string setupFile;
            openFileDialog.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath) + "\\cfg";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                openFileDialog.FilterIndex = 1;
                setupFile = openFileDialog.FileName;
                try
                {
                    if (!mySurround.apiLoaded)
                        mySurround.Initialize();
                    if (NVidia_Surround_Assistant.Properties.Settings.Default.SaveWindowPositions)
                        SM_SaveWindowPositions();
                    mySurround.ApplySetup(setupFile);
                    result = true;
                }
                catch (DisplayManager_Exception ex)
                {
                    MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                    logger.Debug("DM: {0}", ex.Message);                    
                }
            }
            return result;
        }

        public bool SM_ApplySetupFromFile(string setupFile)
        {
            bool result = false;                       
            
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                if (NVidia_Surround_Assistant.Properties.Settings.Default.SaveWindowPositions)
                    SM_SaveWindowPositions();
                mySurround.ApplySetup(setupFile);
                result = true;
            }
            catch (DisplayManager_Exception ex)
            {
                MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                logger.Debug("DM: {0}", ex.Message);
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
                    logger.Info("DM: Saving Surround to memory successful");
                }
                else
                {
                    mySurround.SaveSetupToMemory(false);
                    logger.Info("DM: Saving Non-Surround to memory successful");
                }
                result = true;
            }
            catch (DisplayManager_Exception ex)
            {
                MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                logger.Debug("DM: Saving to memory Error: {0}", ex.Message);                
            }
            return result;
        }

        //Save current setup to File
        public bool SM_SaveCurrentSetup(string filePath)
        {
            bool result = false;
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                mySurround.SaveSetupToFile(filePath);
                logger.Info("DM: Saving to file successful");
                result = true;
            }
            catch (DisplayManager_Exception ex)
            {
                MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Debug("DM: Saving to file Error: {0}", ex.Message);
            }
            return result;
        }

        //Save current setup to file
        public bool SM_SaveCurrentSetupToFile()
        {
            bool result = false;
            saveSurroundFileDialog.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath) + "\\cfg";
            if (saveSurroundFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (!mySurround.apiLoaded)
                        mySurround.Initialize();
                    mySurround.SaveSetupToFile(saveSurroundFileDialog.FileName);
                    logger.Info("DM: Saving to file successful");
                    result = true;
                }
                catch (DisplayManager_Exception ex)
                {
                    MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    logger.Debug("DM: Saving to file Error: {0}", ex.Message);
                }
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
                logger.Info("DM: Window Positions saved successfully");
                result = true;
            }
            catch (DisplayManager_Exception ex)
            {
                logger.Debug("DM: Window Positions saved Error: {0}", ex.Message);
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
                mySurround.MinimizeAllWindows();//TODO This must be moved to before witching back to nomral mode
                mySurround.ApplyWindowPositions();
                logger.Info("DM: Window Positions applied successfully");
                result = true;
            }
            catch (DisplayManager_Exception ex)
            {
                logger.Debug("DM: Window Positions apply Error: {0}", ex.Message);                
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
                logger.Debug("DM: IsSurroundActive Error: {0}", ex.Message);               
            }
            return result;
        }

        public bool SM_IsSurroundActive(String filePath)
        {
            bool result = false;
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                result = mySurround.IsSurroundActive(filePath);
            }
            catch (DisplayManager_Exception ex)
            {
                MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Debug("DM: IsSurroundActive Error: {0}", ex.Message);                
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
