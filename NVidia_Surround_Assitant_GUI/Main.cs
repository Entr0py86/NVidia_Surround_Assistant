#define CLR

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Principal;
using System.IO;
using NLog;
using NLog.Windows.Forms;
using System.Diagnostics;
using NLog.Config;
using NLog.Targets;

/*  TODO List:
 * Add surround profiles that can be applied on application basis
 */

namespace NVidia_Surround_Assistant
{
    public partial class MainForm : Form
    {
        #region DLL_Imports        
        #region User32_dll
        //User32.dll  PInvoke Calls
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out IntPtr lpdwProcessId);
        #endregion
        #region kernel32_dll
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr OpenProcess(
                ProcessAccessFlags processAccess,
                bool bInheritHandle,
                IntPtr processId
        );

        [DllImport("kernel32.dll", SetLastError = true)]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool CloseHandle(IntPtr hObject);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int DebugActiveProcess(IntPtr processID);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int DebugActiveProcessStop(IntPtr processID);

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern int GetLastError();

        #endregion
        #region psapi_dll
        [DllImport("psapi.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode)]
        static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, uint nSize);

        #endregion        
        #endregion

        bool formClosing = false;
        bool hookInstalled = false;        

        bool quitApplication = false;

        static public SurroundManager surroundManager = new SurroundManager();

        string binDir;
                
        List<RegisteredWindowInfo> registeredWindows = new List<RegisteredWindowInfo>();
        //Thread safe queue used for new messages from WndProc. Thread(background worker) used here to eleviate any performance hit that could arise from not exiting WndProc quickly enough.
        ConcurrentQueue<MessageInfo> userMessages = new ConcurrentQueue<MessageInfo>();
        //Hash list use to store names of applications that should trigger surround mode
        HashSet<String> applicationDetectList = new HashSet<string>();
        //Hash list use to store names of applications that should trigger surround mode. Hash list used for speed of search
        List<ProcessInfo> runningApplicationsList = new List<ProcessInfo>();
        //List off all applications
        List<ApplicationInfo> applicationList = new List<ApplicationInfo>();

        //Logger
        Logger logger;        

        //SQL
        SQL sqlInterface = new SQL();

        HookManager hookManager;

        //reset event used to wait for thread to exit before closing app
        private AutoResetEvent _resetEvent = new AutoResetEvent(false);
        private AutoResetEvent _newMsgEvent = new AutoResetEvent(false);

        //Form that receives all messages and adds them to the queue
        
        int y_spacing;

        public MainForm()
        {            
            InitializeComponent();
            
            binDir = Path.GetDirectoryName(Application.ExecutablePath);            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoggerSetup();
            logger = LogManager.GetLogger("nvsaLogger");
            logger.Debug("Application Started {0}", Application.ExecutablePath);

            //Check the operating system and the application being run
            if (Environment.Is64BitProcess && !Environment.Is64BitOperatingSystem)
            {
                MessageBox.Show("Trying to run 64bit version of application on 32bit Operating system.\n\nPlease use the 32bit binaries.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                quitApplication = true;
                Close();
                return;
            }
            if (!Environment.Is64BitProcess && Environment.Is64BitOperatingSystem)
            {
                MessageBox.Show("Trying to run 32bit version of application on 64bit Operating system.\n\nPlease use the 64bit binaries.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                quitApplication = true;
                Close();
                return;
            }

            //Check that app is started with admin priv
            WindowsPrincipal myPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            if (myPrincipal.IsInRole(WindowsBuiltInRole.Administrator) == false)
            {
                //show messagebox - displaying a messange to the user that rights are missing
                MessageBox.Show(text: "The application requires Admin rights.", caption: "Administrator rights required", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Exclamation);
                quitApplication = true;
                Close();
                return;
            }            

            logger.Debug("Application is admin");
            //Create cfg dir
            if (!Directory.Exists(binDir + "\\cfg"))
                Directory.CreateDirectory(binDir + "\\cfg");
            //Create log dir
            if (!Directory.Exists(binDir + "\\logs"))
                Directory.CreateDirectory(binDir + "\\logs");
            logger.Debug("Directories created/checked");

            //run setup if file does not exist
            if (!File.Exists(NVidia_Surround_Assistant.Properties.Settings.Default.SurroundSetupFileName) || !File.Exists(NVidia_Surround_Assistant.Properties.Settings.Default.DefaultSetupFileName) || !surroundManager.surroundSetupLoaded)
                SaveSetupFiles();

            //Initialize application
            hookManager = new HookManager(hWnd, ref registeredWindows);
            Initialize();
            //Get Y Spacing for resizes
            y_spacing = Height - textBoxLogs.Bottom;

            if (NVidia_Surround_Assistant.Properties.Settings.Default.ShowLogs)
            {
                pictureBoxLogs_Click(null, null);
            }

            //Re-init richtext for NLog
            try
            {
                RichTextBoxTarget.ReInitializeAllTextboxes(this);
            }
            catch(NullReferenceException)
            {

            }

            //Start minimized 
            if (NVidia_Surround_Assistant.Properties.Settings.Default.StartMinimized)
                WindowState = FormWindowState.Minimized;           
        }
        
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (NVidia_Surround_Assistant.Properties.Settings.Default.CloseToTray && !quitApplication)
            {                
                Hide();
                e.Cancel = true;
            }
            else
            {
                systemTrayIcon.Visible = false;
                //Swithc back to normal mode depending on settings
                SwitchToNormalMode((Settings_AskSwitch)NVidia_Surround_Assistant.Properties.Settings.Default.SurroundToNormal_OnClose);
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {           
            //Remove icon from system tray            
            systemTrayIcon.Dispose();
                        
            //Ask for threads to join
            formClosing = true;
            if (hookInstalled)
            {
                //Uninstall hooks
                int exitValue = hookManager.UninstallHooks();
                logger.Debug("Hook uninstall value: {0}", exitValue);
                //Cancel background worker
                processCreatedCatcher.CancelAsync();
                _newMsgEvent.Set();
                //Wait for signal that dowork has compeleted
                _resetEvent.WaitOne();
            }
            //Close db connections
            sqlInterface.SQL_CloseConnection();

            _newMsgEvent.Dispose();
            _resetEvent.Dispose();
        }

        public IntPtr hWnd
        {
            get { return base.Handle; }
        }

        protected override void WndProc(ref Message m)
        {
            Boolean handled = false;
            MessageInfo msgTemp = new MessageInfo();
            m.Result = IntPtr.Zero;

            uint msg_id = (uint)m.Msg;
            int index = -1;
            //if the msg id mathces a registered window message add that message to the queue
            if ((index = registeredWindows.FindIndex(r => r.windowRegisterID.Equals(msg_id))) != -1)
            {
                msgTemp.regWndInfo = registeredWindows[index];
                msgTemp.LParam = m.LParam;
                msgTemp.WParam = m.WParam;
                userMessages.Enqueue(msgTemp);
                if (!formClosing)//Only set event if the form is not going to close
                    _newMsgEvent.Set();
                handled = true;
                m.Result = new IntPtr(1);
            }
            if (handled)
                DefWndProc(ref m);
            else
                base.WndProc(ref m);
        }

        void Initialize()
        {            
            //Install the hooks
            hookInstalled = hookManager.InstallHooksAndRegisterWindows();
            if (!processCreatedCatcher.IsBusy)
            {
                processCreatedCatcher.RunWorkerAsync();
            }
            //Open SQL conection to db
            sqlInterface.SQL_OpenConnection(binDir + "\\cfg\\nvsa_db.sqlite");            
            
            //Load all applications into list
            LoadApplicationList();           
        }

        public bool SaveSetupFiles()
        {
            string SurroundSetupFileName = binDir + "\\cfg\\Default Surround Grid.nvsa";
            string SetupFileName = binDir + "\\cfg\\Default Grid.nvsa";            
            bool skipSurround = false;
            bool skipDefault = false;

            //Save current dispaly setup for re-apllication later
            surroundManager.SM_SaveCurrentSetup();
            surroundManager.SM_SaveWindowPositions();

            //Check if surround setup file already exists
            if (File.Exists(SurroundSetupFileName))
            {
                if (MessageBox.Show("Default Surround Setup file detected. Delete it?", "Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    File.Delete(SurroundSetupFileName);
                else
                    skipSurround = true;
            }

            //Check if surround setup file already exists
            if (File.Exists(SetupFileName))
            {
                if (MessageBox.Show("Default Setup file detected. Delete it?", "Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    File.Delete(SetupFileName);
                else
                    skipDefault = true;
            }

            if (!skipDefault)
            {
                if (MessageBox.Show("Save current display setup as default?", "Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    if (surroundManager.SM_IsSurroundActive())
                    {
                        MessageBox.Show("NVidia Surround mode currently active. If this is not your intention, then please disable NVidia Surround via NVidia control panel(keyboard shortcuts) now.\n\nWhen display is setup to your liking, press OK", "Default Display Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);                        
                    }
                    //Save memory to file
                    surroundManager.SM_SaveCurrentSetup(SetupFileName);
                }
                else
                {
                    MessageBox.Show("Default setup not saved. Certain functionality will not work until application is restarted");
                    return false;
                }
            }

            if (!skipSurround)
            {
                if (MessageBox.Show("Save current display setup as surround setup?", "Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    //Save memory to file
                    surroundManager.SM_SaveCurrentSetup(SurroundSetupFileName);
                }
                else
                {
                    MessageBox.Show("Default surround setup not saved.\nPlease re-run setup for proper operation of application.");
                    return false;
                }
            }            
            //Apply saved config. Display manager will not switch if there is no difference to grid setup
            surroundManager.SM_ApplySetupFromMemory(false);
            surroundManager.SM_ApplyWindowPositions();

            NVidia_Surround_Assistant.Properties.Settings.Default.SurroundSetupFileName = SurroundSetupFileName;
            NVidia_Surround_Assistant.Properties.Settings.Default.DefaultSetupFileName = SetupFileName;

            surroundManager.SM_ReadDefaultSurroundConfig();
            return true;
        }       

        void LoadApplicationList()
        {
            applicationList = sqlInterface.GetApplicationList();

            foreach (ApplicationInfo app in applicationList)
            {
                RegisterApplication(app);
            }           
        }

        void DeleteApplicationFromList(Thumb AppThumb)
        {
            if (MessageBox.Show("Delete from database?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                
                if (sqlInterface.DeleteApplication(AppThumb.Id))
                {
                    //Remove data from lists    
                    logger.Info("Delete Application: {0} deleted from library", AppThumb.DisplayName);
                    applicationDetectList.Remove(AppThumb.FullPath);
                    thumbGridView.RemoveThumb(AppThumb);
                    applicationList.Remove(AppThumb.ApplicationInfo);
                }
            }
            else
            {
                sqlInterface.DisableApplication(AppThumb.Id);
                AppThumb.AppEnabled = false;
                logger.Info("Disable Application: {0} disabled", AppThumb.DisplayName);
            }
        }        

        void AddNewApplication(ApplicationInfo newApp)
        {
            EditApplicationSettings editWindow = new EditApplicationSettings(newApp, true);
            editWindow.Text = "Add New Application";            
            
            //Check if application already in list
            if (applicationDetectList.Count > 0 && applicationDetectList.Contains(newApp.FullPath))
            {
                MessageBox.Show("Application already in list and will not be added.", "Add Application Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
           
            if (editWindow.ShowDialog() == DialogResult.OK)
            {
                newApp = editWindow.AppInfo;
                newApp.Id = sqlInterface.AddApplication(newApp);
                if (newApp.Id >= 0)
                {
                    RegisterApplication(newApp);                
                }
                else
                {
                    MessageBox.Show("Error adding application to database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        void RegisterApplication(ApplicationInfo App)
        {
            Thumb newThumb = new Thumb(App, logger);
            newThumb.removeApplication += DeleteApplicationFromList;
            newThumb.editApplication += EditApplication;
            newThumb.silentEditApplication += SilentEditApplication;

            //Add data to lists    
            applicationDetectList.Add(App.FullPath);
            thumbGridView.AddThumb(newThumb);
        }

        ApplicationInfo GetApplicationFromList(String processName)
        {
            foreach(ApplicationInfo app in applicationList)
            {
                if (app.FullPath.Equals(processName))
                    return app;
            }
            return null;
        }

        void EditApplication(Thumb AppThumb)
        {
            EditApplicationSettings editWindow = new EditApplicationSettings(AppThumb.ApplicationInfo, false);
            
            if(editWindow.ShowDialog() == DialogResult.OK)
            {
                AppThumb.ApplicationInfo = editWindow.AppInfo;
                if(sqlInterface.UpdateApplication(editWindow.AppInfo))
                    logger.Info("Edit Application: {0} edited", AppThumb.DisplayName);
                else
                    logger.Error("Edit Application: {0} edited", AppThumb.DisplayName);
            }       
        }

        void SilentEditApplication(Thumb AppThumb)
        {
            if (AppThumb.AppEnabled)
                AppThumb.AppEnabled = false;
            else
                AppThumb.AppEnabled = true;
            if (sqlInterface.UpdateApplication(AppThumb.ApplicationInfo))
                logger.Info("Edit Application: {0} edited", AppThumb.DisplayName);
            else
                logger.Error("Edit Application: {0} edited", AppThumb.DisplayName);
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

        void SwitchToNormalMode(Settings_AskSwitch ask)
        {
            if (surroundManager != null)
            {
                if (surroundManager.SM_IsSurroundActive())
                {
                    switch (ask)
                    {
                        case Settings_AskSwitch.Always:
                            surroundManager.SM_ApplySetupFromMemory(false);
                            break;
                        case Settings_AskSwitch.Ask:
                            if (MessageBox.Show("Disable NVidia surround?", "Disable Surround", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000) == DialogResult.Yes)
                                surroundManager.SM_ApplySetupFromMemory(false);
                            break;
                        case Settings_AskSwitch.Never:
                            break;
                        default:
                            break;

                    }
                }
            }
        }

        //Get the window name from the window handle
        private void GetWindowHandleInfo(IntPtr hWnd, ref MessageInfo msg)
        {
            StringBuilder strBuilder = new StringBuilder(1024);
            ProcessAccessFlags procFlags = ProcessAccessFlags.All;

            //Allocate window handle
            msg.procInfo.hWnd = hWnd;
            //Get process id of window handle
            GetWindowThreadProcessId(hWnd, out msg.procInfo.procID);

            //Get process name
            msg.procInfo.handleID = OpenProcess(procFlags, false, msg.procInfo.procID);
            GetModuleFileNameEx(msg.procInfo.handleID, IntPtr.Zero, strBuilder, 1024);
            CloseHandle(msg.procInfo.handleID);
            msg.procInfo.processName = strBuilder.ToString();
        }

        private void processCreatedCatcher_DoWork(object sender, DoWorkEventArgs e)
        {
            MessageInfo message;
            logger.Debug("App Paused: {0}", GetLastError().ToString());
            BackgroundWorker thisWorker = sender as BackgroundWorker;
            while (!thisWorker.CancellationPending)
            {
                if (userMessages.Count > 1)
                {
                    userMessages.TryDequeue(out message);
                    thisWorker.ReportProgress(1, message);
                    if (!formClosing)//Only reset event if the form is not going to close
                        _newMsgEvent.Reset();
                }
                else
                {
                    _newMsgEvent.WaitOne();
                }
            }
            e.Cancel = true;
            //Signal to the destructor that do work has completed
            _resetEvent.Set();
        }

        private void processCreatedCatcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            MessageInfo msg = (MessageInfo)e.UserState;
            if (msg == null) return;

            GetWindowHandleInfo(msg.WParam, ref msg);
            logger.Debug("Hook captured: {0} Hook Type: {1}", msg.procInfo.processName, msg.regWndInfo.type.ToString());
            if (applicationDetectList.Contains(msg.procInfo.processName))
            {
                ProcessCreatedWindow(msg);
            }
        }

        void ProcessCreatedWindow(MessageInfo msg)
        {
            int index = 0;
            ApplicationInfo app;
            //Program suspension has mutliple methods. I chose the one that seemd to always worked and was simple. Read this post https://stackoverflow.com/questions/11010165/how-to-suspend-resume-a-process-in-windows

            //if the msg id does not match then process
            if ((index = runningApplicationsList.FindIndex(r => r.procID == msg.procInfo.procID)) == -1)
            {
                app = GetApplicationFromList(msg.procInfo.processName);
                if (app == null)
                {
                    if (!surroundManager.SM_IsSurroundActive())
                    {
                        //Pause detected application until surround has been activated
                        if (DebugActiveProcess(msg.procInfo.procID) != 0)
                        {
                            logger.Debug("App Paused: {0}", GetLastError().ToString());
                            //Save current dispaly setup for re-apllication later
                            surroundManager.SM_SaveCurrentSetup();
                            if (!surroundManager.SM_ApplySetupFromMemory(true))
                            {
                                if (MessageBox.Show("Surround enable failed!\nWould you like to continue starting your application?", "Continue", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000) == DialogResult.No)
                                {
                                    if (DebugActiveProcessStop(msg.procInfo.procID) == 0)
                                    {
                                        logger.Debug("App Un-Paused: {0}", GetLastError().ToString());
                                        Thread.Sleep(100);
                                    }

                                    try
                                    {
                                        msg.procInfo.process = Process.GetProcessById((int)msg.procInfo.procID);
                                        msg.procInfo.process.Kill();
                                    }
                                    catch (ArgumentException ex)
                                    {
                                        logger.Error("Process kill error: {0}", ex.Message);
                                    }
                                    catch (Win32Exception ex)
                                    {
                                        logger.Error("Process kill error: {0}", ex.Message);
                                    }
                                    catch (NotSupportedException ex)
                                    {
                                        logger.Error("Process kill error: {0}", ex.Message);
                                    }
                                    catch (InvalidOperationException ex)
                                    {
                                        logger.Error("Process kill error: {0}", ex.Message);
                                    }
                                }
                            }
                            else
                            {
                                logger.Info("Application Detected: {0}", msg.procInfo.processName);
                                runningApplicationsList.Add(msg.procInfo);

                                //Attach to OnExited event of Window
                                msg.procInfo.process = Process.GetProcessById((int)msg.procInfo.procID);
                                msg.procInfo.process.EnableRaisingEvents = true;
                                msg.procInfo.process.Exited += (sender, e) => ListedProcess_Exited(sender, e);

                                if (DebugActiveProcessStop(msg.procInfo.procID) == 0)
                                {
                                    logger.Debug("App Un-Paused: {0}", GetLastError().ToString());
                                }
                            }
                        }
                        else
                        {
                            logger.Debug("App Pause: {0}", GetLastError().ToString());
                        }
                    }
                }
                else
                {
                    if (!surroundManager.SM_IsSurroundActive(binDir + "\\cfg\\" + app.SurroundGrid))
                    {
                        //Pause detected application until surround has been activated
                        if (DebugActiveProcess(msg.procInfo.procID) != 0)
                        {
                            logger.Debug("App Paused: {0}", GetLastError().ToString());
                            //Save current dispaly setup for re-apllication later
                            surroundManager.SM_SaveCurrentSetup();
                            if (!surroundManager.SM_ApplySetupFromFile(binDir + "\\cfg\\" + app.SurroundGrid))
                            {
                                if (MessageBox.Show("Surround enable failed!\nWould you like to continue starting your application?", "Continue", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000) == DialogResult.No)
                                {
                                    if (DebugActiveProcessStop(msg.procInfo.procID) == 0)
                                    {
                                        logger.Debug("App Un-Paused: {0}", GetLastError().ToString());
                                        Thread.Sleep(100);
                                    }

                                    try
                                    {
                                        msg.procInfo.process = Process.GetProcessById((int)msg.procInfo.procID);
                                        msg.procInfo.process.Kill();
                                    }
                                    catch (ArgumentException ex)
                                    {
                                        logger.Error("Process kill error: {0}", ex.Message);
                                    }
                                    catch (Win32Exception ex)
                                    {
                                        logger.Error("Process kill error: {0}", ex.Message);
                                    }
                                    catch (NotSupportedException ex)
                                    {
                                        logger.Error("Process kill error: {0}", ex.Message);
                                    }
                                    catch (InvalidOperationException ex)
                                    {
                                        logger.Error("Process kill error: {0}", ex.Message);
                                    }
                                }
                            }
                            else
                            {
                                logger.Info("Application Detected: {0}", msg.procInfo.processName);
                                runningApplicationsList.Add(msg.procInfo);

                                //Attach to OnExited event of Window
                                msg.procInfo.process = Process.GetProcessById((int)msg.procInfo.procID);
                                msg.procInfo.process.EnableRaisingEvents = true;
                                msg.procInfo.process.Exited += (sender, e) => ListedProcess_Exited(sender, e);

                                if (DebugActiveProcessStop(msg.procInfo.procID) == 0)
                                {
                                    logger.Debug("App Un-Paused: {0}", GetLastError().ToString());
                                }
                            }
                        }
                        else
                        {
                            logger.Debug("App Pause: {0}", GetLastError().ToString());
                        }
                    }
                }
            }
        }

        // Handle Exited event: check running list if none switch back to normal display setup
        private void ListedProcess_Exited(object sender, System.EventArgs e)
        {
            //If the event is fired from another thread, then we want to invoke the method again in the applications main context
            if (InvokeRequired)
            {
                Invoke(new Action<object, EventArgs>(ListedProcess_Exited), sender, e);
                return;
            }

            Process proc = sender as Process;
            int index = 0;
            if ((index = runningApplicationsList.FindIndex(r => (int)r.procID == proc.Id)) != -1)
            {
                runningApplicationsList.RemoveAt(index);
                //If no other app is in list switch back to normal mode
                if (runningApplicationsList.Count == 0)
                {
                    SwitchToNormalMode((Settings_AskSwitch)NVidia_Surround_Assistant.Properties.Settings.Default.SurroundToNormal_OnExit);
                }
            }
        }

        private void LoggerSetup()
        {
            // Create configuration object 
            LoggingConfiguration config = new LoggingConfiguration();            
            NLog.Layouts.Layout layout = @"${level:uppercase=true} ${longdate} ${message}";

            // Create targets and add them to the configuration 
            ColoredConsoleTarget consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);

            FileTarget fileTarget = new FileTarget();
            config.AddTarget("logFile", fileTarget);

            RichTextBoxTarget textBoxTarget = new RichTextBoxTarget();
            config.AddTarget("textBox", textBoxTarget);

            //Set target properties
            consoleTarget.Layout = layout;            
            fileTarget.Layout = layout;
            textBoxTarget.Layout = layout;
            
            fileTarget.FileName = "logs/nvsa_log.txt";
            textBoxTarget.AutoScroll = true;
            textBoxTarget.ControlName = "textBoxLogs";
            textBoxTarget.FormName = "MainForm";
            textBoxTarget.AllowAccessoryFormCreation = true;
            textBoxTarget.MaxLines = 500;

            // Define rules
            LogLevel logLevel;

            switch (NVidia_Surround_Assistant.Properties.Settings.Default.LogLevel)
            {
                case 0:
                    logLevel = LogLevel.Off;
                    break;
                case 1:
                    logLevel = LogLevel.Debug;
                    break;
                case 2:
                    logLevel = LogLevel.Info;
                    break;
                case 3:
                    logLevel = LogLevel.Error;
                    break;
                case 4:
                    logLevel = LogLevel.Fatal;
                    break;
                default:
                    logLevel = LogLevel.Info;
                    break;
            }

            

            LoggingRule rule1 = new LoggingRule("*", logLevel, consoleTarget);
            LoggingRule rule2 = new LoggingRule("*", logLevel, fileTarget);
            LoggingRule rule3 = new LoggingRule("*", logLevel, textBoxTarget);
            config.LoggingRules.Add(rule1);
            config.LoggingRules.Add(rule2);
            config.LoggingRules.Add(rule3);

            // Activate the configuration
            LogManager.Configuration = config;
        }

        #region controls
        private void PictureBoxAddGame_Click(object sender, EventArgs e)
        {
            ApplicationInfo newApp = new ApplicationInfo();

            openFileDialog.InitialDirectory = Path.GetPathRoot(Application.ExecutablePath);
            openFileDialog.FilterIndex = 2;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                newApp.DisplayName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                newApp.Enabled = true;
                newApp.FullPath = Path.GetFullPath(openFileDialog.FileName);
                newApp.Id = 0;

                AddNewApplication(newApp);
            }
        }

        private void pictureBoxSettings_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();

            settings.ShowDialog();
        }

        private void toolStripMenuItem_ToggelSurround_Click(object sender, EventArgs e)
        {
            if (surroundManager.SM_IsSurroundActive())
            {
                surroundManager.SM_ApplySetupFromMemory(false);
            }
            else
            {
                surroundManager.SM_ApplySetupFromMemory(true);
            }
        }

        private void toolStripMenuItem_AddApp_Click(object sender, EventArgs e)
        {
            PictureBoxAddGame_Click(null, null);
        }

        private void toolStripMenuItem_LoadSurroundFile_Click(object sender, EventArgs e)
        {            
            surroundManager.SM_ApplySetupFromFile();            
        }

        private void toolStripMenuItem_SaveSurroundFile_Click(object sender, EventArgs e)
        {            
            surroundManager.SM_SaveCurrentSetupToFile();            
        }

        private void toolStripMenuItem_Quit_Click(object sender, EventArgs e)
        {
            quitApplication = true;
            Close();
        }

        private void SystemTrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void SystemTrayIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Hide();
                systemTrayIcon.Visible = true;
            }
        }        

        private void contextMenuStrip_SystemTray_Opening(object sender, CancelEventArgs e)
        {
            if (surroundManager.SM_IsSurroundActive())
            {
                toolStripMenuItem_ToggelSurround.Text = "Switch To Normal Mode";
            }
            else
            {
                toolStripMenuItem_ToggelSurround.Text = "Switch To Surround Mode";
            }
        }       

        private void pictureBoxLogs_Click(object sender, EventArgs e)
        {            
            if (textBoxLogs.Visible == true)
            {                
                textBoxLogs.Visible = false;
                thumbGridView.Size = new Size(thumbGridView.Width, textBoxLogs.Top + y_spacing);
                this.Size = new Size(this.Width, textBoxLogs.Top + y_spacing);
                
            }
            else
            {
                textBoxLogs.Visible = true;
                thumbGridView.Size = new Size(thumbGridView.Width, textBoxLogs.Top + y_spacing);
                this.Size = new Size(this.Width, textBoxLogs.Bottom + y_spacing);                
            }
        }

        private void pictureBoxSaveConfig_Click(object sender, EventArgs e)
        {
            surroundManager.SM_SaveCurrentSetupToFile();
        }

        private void pictureBoxLoadConfig_Click(object sender, EventArgs e)
        {
            surroundManager.SM_ApplySetupFromFile();
        }
        #endregion


    }
}
