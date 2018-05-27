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
        bool surroundSetupLoaded = false;
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
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                mySurround.LoadSetup(NVidia_Surround_Assistant.Properties.Settings.Default.SurroundSetupFileName, true);
                mySurround.LoadSetup(NVidia_Surround_Assistant.Properties.Settings.Default.DefaultSetupFileName, false);
            }
            catch (DisplayManager_Exception ex)
            {
                logger.Debug("DM: {0}", ex.Message);
                return false;
            }
            return true;
        }

        public bool SM_ReadSurroundConfig(string fileName)
        {
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                mySurround.LoadSetup(NVidia_Surround_Assistant.Properties.Settings.Default.SurroundSetupFileName, true);
            }
            catch (DisplayManager_Exception ex)
            {
                logger.Debug("DM: {0}", ex.Message);
                return false;
            }
            return true;
        }

        public bool SM_ApplySetupFromMemory(bool Surround)
        {
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
            }
            catch (DisplayManager_Exception ex)
            {
                MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                logger.Debug("DM: {0}", ex.Message);
                return false;
            }
            return true;
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
                    mySurround.ApplySetup(setupFile);
                }
                catch (DisplayManager_Exception ex)
                {
                    MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                    logger.Debug("DM: {0}", ex.Message);
                    return false;
                }
                return true;
            }
            return false;
        }

        //Save current setup to memory
        public bool SM_SaveCurrentSetup()
        {
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                if (mySurround.IsSurroundActive())
                {
                    mySurround.SaveSetupToMemory(true);
                    logger.Info("DM: Saving Surround to memory succesfull");
                }
                else
                {
                    mySurround.SaveSetupToMemory(false);
                    logger.Info("DM: Saving Non-Surround to memory succesfull");
                }
            }
            catch (DisplayManager_Exception ex)
            {
                MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                logger.Debug("DM: Saving to memory Error: {0}", ex.Message);
                return false;
            }
            return true;
        }

        //Save current setup to File
        public bool SM_SaveCurrentSetup(string filePath)
        {
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                mySurround.SaveSetupToFile(filePath);
                logger.Info("DM: Saving to file succesfull");
            }
            catch (DisplayManager_Exception ex)
            {
                MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Debug("DM: Saving to file Error: {0}", ex.Message);
                return false;
            }
            return true;
        }

        //Save current setup to file
        public bool SM_SaveCurrentSetupToFile()
        {
            saveSurroundFileDialog.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath) + "\\cfg";
            if (saveSurroundFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if (!mySurround.apiLoaded)
                        mySurround.Initialize();
                    mySurround.SaveSetupToFile(saveSurroundFileDialog.FileName);
                    logger.Info("DM: Saving to file succesfull");
                }
                catch (DisplayManager_Exception ex)
                {
                    MessageBox.Show("Display Manger Error: " + ex.Message + ".", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    logger.Debug("DM: Saving to file Error: {0}", ex.Message);
                    return false;
                }
                return true;
            }
            return false;
        }

        //Save current setup to file
        public bool SM_SaveWindowPositions()
        {
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                mySurround.SaveWindowPositions();
                mySurround.MinimizeAllWindows();
                Thread.Sleep(100);
                logger.Info("DM: Window Positions saved succesfully");
            }
            catch (DisplayManager_Exception ex)
            {
                logger.Debug("DM: Window Positions saved Error: {0}", ex.Message);
                return false;
            }
            return true;
        }

        //Save current setup to file
        public bool SM_ApplyWindowPositions()
        {
            try
            {
                if (!mySurround.apiLoaded)
                    mySurround.Initialize();
                mySurround.ApplyWindowPositions();
                logger.Info("DM: Window Positions applied succesfully");
            }
            catch (DisplayManager_Exception ex)
            {
                logger.Debug("DM: Window Positions apply Error: {0}", ex.Message);
                return false;
            }
            return true;
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
                return false;
            }
            return result;
        }
    }
}
