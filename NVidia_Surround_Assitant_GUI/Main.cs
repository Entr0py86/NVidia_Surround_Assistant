#define CLR
//#define DEBUG_NO_SURROUND_SWITCH

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Text;
using System.Management;
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
using System.Reflection;

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
        bool formShown = false;
        bool hookInstalled = false;

        bool quitApplication = false;

        //SQL Instance
        static public SQL sqlInterface = new SQL();
        //Logger
        static public Logger logger = LogManager.GetLogger("nvsaLogger");
        //Surround Manager Instance
        static public SurroundManager surroundManager = new SurroundManager();

        string binDir;

        List<RegisteredWindowInfo> registeredWindows = new List<RegisteredWindowInfo>();
        //Thread safe queue used for new messages from WndProc. Thread(background worker) used here to alleviate any performance hit that could arise from not exiting WndProc quickly enough.
        ConcurrentQueue<HookMessageInfo> userMessages = new ConcurrentQueue<HookMessageInfo>();
        //Hash list use to store names of applications that should trigger surround mode
        HashSet<String> applicationDetectList = new HashSet<string>();
        //Hash list use to store names of applications that should trigger surround mode. Hash list used for speed of search
        List<ProcessInfo> runningApplicationsList = new List<ProcessInfo>();
        //List off all applications
        List<ApplicationInfo> applicationList = new List<ApplicationInfo>();
        //Mutex to lock ProcessCreatedWindow while busy
        Mutex newProcessMutex = new Mutex();

        //Process creation detectors
        HookManager hookManager;
        ManagementEventWatcher processStartEvent;
        ManagementEventWatcher processStopEvent;

        //List used to add all stopped processes that are in the applicationDetectList
        ConcurrentQueue<ProcessInfo> StoppedProcessList = new ConcurrentQueue<ProcessInfo>();
        int processStoppedSyncPoint = 0;
        //Timer used to stagger switching as some applications start multiple processes of the same name but exit before the application actually runs
        System.Timers.Timer processStopTimer = new System.Timers.Timer();

        //reset event used to wait for thread to exit before closing app
        private AutoResetEvent _resetEvent = new AutoResetEvent(false);
        private AutoResetEvent _newMsgEvent = new AutoResetEvent(false);

        int border_spacing_Form = 0;
        int y_spacing_logBox = 0;

        int textBoxLogs_PixelShift = 0;
        int mainForm_minSizeWithLogBox = 726;

        static public Color hoverButtonColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
        static public Color normalControlColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));


        //Form that receives all messages and adds them to the queue
        public MainForm()
        {
            InitializeComponent();

            foreach (Control control in this.Controls)
            {
                control.BackColor = normalControlColor;
                control.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
            }

            Text += String.Format("Version {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            //Setup process stopped timer. 
            processStopTimer.Interval = 5000;//5seconds
            processStopTimer.AutoReset = false;
            processStopTimer.Elapsed += processStopTimer_OnTimedEvent;

            binDir = Path.GetDirectoryName(Application.ExecutablePath);
            //Set the renderer to the one created.
            ToolStripManager.Renderer = new NvsaRenderer();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            LoggerSetup();
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

            //Check that app is started with Administrator privileges
            WindowsPrincipal myPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            if (myPrincipal.IsInRole(WindowsBuiltInRole.Administrator) == false)
            {
                //show message box - displaying a message to the user that rights are missing
                MessageBox.Show(text: "The application requires Administrator rights.", caption: "Administrator rights required", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Exclamation);
                quitApplication = true;
                Close();
                return;
            }

            logger.Debug("Application is run as Administrator");
            //Create cfg dir
            if (!Directory.Exists(binDir + "\\cfg"))
                Directory.CreateDirectory(binDir + "\\cfg");
            //Create log dir
            if (!Directory.Exists(binDir + "\\logs"))
                Directory.CreateDirectory(binDir + "\\logs");
            logger.Debug("Directories created/checked");


            //Open SQL connection to db
            sqlInterface.SQL_OpenConnection(binDir + "\\db\\nvsa_db.sqlite");
            //Load defaults config's
            surroundManager.SM_ReadDefaultSurroundConfig();

            //Initialize application            
            hookManager = new HookManager(hWnd, ref registeredWindows);
            Initialize();

            //Re-init richtext for NLog
            try
            {
                RichTextBoxTarget.ReInitializeAllTextboxes(this);
            }
            catch (NullReferenceException)
            {

            }
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
                //Stop event manager
                if (processStartEvent != null)
                {
                    try
                    {
                        processStartEvent.Stop();
                    }
                    catch (System.Runtime.InteropServices.COMException ex)
                    {
                        logger.Debug("processStartEvent stop failed: {0}", ex.Message);
                    }
                }

                if (processStopEvent != null)
                {
                    try
                    {
                        processStopEvent.Stop();
                    }
                    catch (System.Runtime.InteropServices.COMException ex)
                    {
                        logger.Debug("processStopEvent stop failed: {0}", ex.Message);
                    }
                }
                //Switch back to normal mode depending on settings
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
                //Un-install hooks
                int exitValue = hookManager.UninstallHooks();
                logger.Debug("Hook un-install value: {0}", exitValue);
                //Cancel background worker
                processCreatedCatcher.CancelAsync();
                _newMsgEvent.Set();
                //Wait for signal that do work has completed
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
            HookMessageInfo msgTemp = new HookMessageInfo();
            m.Result = IntPtr.Zero;

            uint msg_id = (uint)m.Msg;
            int index = -1;
            //if the message id matches a registered window message add that message to the queue
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

            //Load all applications into list
            LoadApplicationList();            
            populateContextMenuStripLoadApp();

            CreateProcessWatchers();
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
                    if (AppThumb.ProcessName == AppThumb.DisplayName)
                    {
                        applicationDetectList.Remove(AppThumb.DisplayName);
                    }
                    else
                    {
                        applicationDetectList.Remove(AppThumb.DisplayName);
                        applicationDetectList.Remove(AppThumb.ProcessName);
                    }
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
            if (applicationDetectList.Count > 0 && applicationDetectList.Contains(newApp.ProcessName))
            {
                EditApplication(GetApplicationFromList(newApp.ProcessName));
            }
            else
            {
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
        }

        void RegisterApplication(ApplicationInfo App)
        {
            Thumb newThumb = new Thumb(App);
            newThumb.launchApplication += LaunchApplication;
            newThumb.removeApplication += DeleteApplicationFromList;
            newThumb.editApplication += EditApplication;
            newThumb.silentEditApplication += SilentEditApplication;

            //Add data to lists    
            if (App.ProcessName == App.DisplayName)
            {
                applicationDetectList.Add(App.ProcessName);
            }
            else
            {
                applicationDetectList.Add(App.DisplayName);
                applicationDetectList.Add(App.ProcessName);
            }
            thumbGridView.AddThumb(newThumb);
        }

        ApplicationInfo GetApplicationFromList(String processName)
        {
            foreach (ApplicationInfo app in applicationList)
            {
                if (app.ProcessName.Equals(processName))
                    return app;
            }
            return null;
        }

        void EditApplication(Thumb AppThumb)
        {
            EditApplicationSettings editWindow = new EditApplicationSettings(AppThumb.ApplicationInfo, false);

            if (editWindow.ShowDialog() == DialogResult.OK)
            {
                AppThumb.ApplicationInfo = editWindow.AppInfo;
                if (sqlInterface.UpdateApplication(editWindow.AppInfo))
                    logger.Info("Edit Application: {0} edited", AppThumb.DisplayName);
                else
                    logger.Error("Edit Application: {0} edited", AppThumb.DisplayName);
            }
        }

        void EditApplication(ApplicationInfo AppInfo)
        {
            EditApplicationSettings editWindow = new EditApplicationSettings(AppInfo, false);

            if (editWindow.ShowDialog() == DialogResult.OK)
            {
                AppInfo = editWindow.AppInfo;
                if (sqlInterface.UpdateApplication(editWindow.AppInfo))
                    logger.Info("Edit Application: {0} edited", AppInfo.DisplayName);
                else
                    logger.Error("Edit Application: {0} edited", AppInfo.DisplayName);
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

        void LaunchApplication(ApplicationInfo launchApp)
        {
            ProcessInfo process = new ProcessInfo();
            Process startProcess = new Process();

            logger.Info("Manual Application Launch: {0}", launchApp.DisplayName);
            if (launchApp.Enabled)
            {
                if (!surroundManager.SM_IsSurroundActive(launchApp.SurroundGrid))
                {
                    //Save current display setup for re-application later
                    surroundManager.SM_SaveCurrentSetup();
                    if (!surroundManager.SM_ApplySetup(launchApp.SurroundGrid))
                    {
                        if (MessageBox.Show("Surround enable failed!\nWould you like to continue starting your application?", "Continue", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000) == DialogResult.No)
                        {
                            return;
                        }
                    }
                }
            }
            //Populate options to start the application
            startProcess.StartInfo.FileName = launchApp.FullPath;
            //Start process
            startProcess.Start();
            //populate required info for list
            process.processName = Path.GetFileNameWithoutExtension(launchApp.FullPath);
            process.procID = (IntPtr)startProcess.Id;
            //Don't add to list if app is not enabled
            if (launchApp.Enabled)
            {
                //Add To running application list
                runningApplicationsList.Add(process);
            }
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

        void CreateProcessWatchers()
        {
            processStartEvent = new ManagementEventWatcher(
              new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            processStartEvent.EventArrived += new EventArrivedEventHandler(processStartEvent_EventArrived);
            processStartEvent.Start();

            processStopEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStopTrace");
            processStopEvent.EventArrived += new EventArrivedEventHandler(processStopEvent_EventArrived);
            processStopEvent.Start();
        }

        //Event is fired when a new process is created
        void processStartEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            ProcessInfo process = new ProcessInfo();            

            process.procID = new IntPtr(Convert.ToInt64(e.NewEvent.Properties["ProcessID"].Value));
            process.processName = Path.GetFileNameWithoutExtension(e.NewEvent.Properties["ProcessName"].Value as string);
            logger.Trace("WMI Process started: {0} ", process.processName);

            if (applicationDetectList.Contains(process.processName))
            {                    
              ProcessCreatedWindow(process);             
            }
        }

        void processStopEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            ProcessInfo process = new ProcessInfo();
            ApplicationInfo applicationInfo;

            string processName = e.NewEvent.Properties["ProcessName"].Value as string;
            process.procID = new IntPtr(Convert.ToInt64(e.NewEvent.Properties["ProcessID"].Value));
            process.processName = Path.GetFileNameWithoutExtension(processName);
            logger.Trace("WMI Process stopped: {0} ", process.processName);

            if (applicationDetectList.Contains(process.processName))
            {
                applicationInfo = GetApplicationFromList(process.processName);
                
                StoppedProcessList.Enqueue(process);
                logger.Debug("WMI Process stopped: {0}; Switch in {1} sec", process.processName, applicationInfo.SwitchbackTimeout);
                if (!processStopTimer.Enabled)
                {
                    processStopTimer.Interval = applicationInfo.SwitchbackTimeout * 1000;//Make seconds into ms
                    timerZombieCheck.Interval = applicationInfo.SwitchbackTimeout * 1000 * 2;//Make seconds into ms
                    timerZombieCheck.Start();
                    processStopTimer.Start();
                }
            }
        }

        //Get the window name from the window handle
        private void GetWindowHandleInfo(IntPtr hWnd, ref ProcessInfo process)
        {
            StringBuilder strBuilder = new StringBuilder(1024);
            ProcessAccessFlags procFlags = ProcessAccessFlags.All;
            try
            {
                //Allocate window handle
                process.hWnd = hWnd;
                //Get process id of window handle
                GetWindowThreadProcessId(hWnd, out process.procID);

                //Get process name
                process.handleID = OpenProcess(procFlags, false, process.procID);
                GetModuleFileNameEx(process.handleID, IntPtr.Zero, strBuilder, 1024);
                CloseHandle(process.handleID);
                process.processName = Path.GetFileNameWithoutExtension(strBuilder.ToString());

            }
            catch (ArgumentException)
            {
                logger.Error("GetWindowHandleInfo: Invalid Argument {0}", process);
            }
        }

        private void processCreatedCatcher_DoWork(object sender, DoWorkEventArgs e)
        {
            HookMessageInfo message;
            logger.Debug("Hook message process thread running");
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
            HookMessageInfo msg = (HookMessageInfo)e.UserState;
            ProcessInfo process = new ProcessInfo();
            if (msg == null) return;

            if (msg.regWndInfo.type != HookType.sysCommand)
            {
                GetWindowHandleInfo(msg.WParam, ref process);
                logger.Trace("Hook captured: {0} Hook Type: {1}", process.processName, msg.regWndInfo.type.ToString());
                if (applicationDetectList.Contains(process.processName) && (int)process.procID != 0)
                {
                    logger.Debug("App From list detected by Hook: {0}", process.processName);
                    ProcessCreatedWindow(process);
                }
            }
            else
            {
                logger.Debug("Hook Type: {0}", msg.regWndInfo.type.ToString());
            }
        }

        void ProcessCreatedWindow(ProcessInfo process)
        {
            int index = 0;
            ApplicationInfo app;
            //Program suspension has multiple methods. I chose the one that seemed to always worked and was simple. Read this post https://stackoverflow.com/questions/11010165/how-to-suspend-resume-a-process-in-windows
            try
            {
                newProcessMutex.WaitOne();
                //if the message id does not match then process
                if ((index = runningApplicationsList.FindIndex(r => r.processName == process.processName)) == -1)
                {
                    app = GetApplicationFromList(process.processName);
#if !DEBUG_NO_SURROUND_SWITCH
                    if (app != null && app.Enabled)
                    {
                        logger.Debug("loading surround id {0} for app {1}", app.SurroundGrid, app.ProcessName);
                        if (!surroundManager.SM_IsSurroundActive(app.SurroundGrid))
                        {
                            if (app.PauseOnDetect)
                            {
                                //Pause detected application until surround has been activated. If no error then switch will be done without pause. Might cause issues to driver
                                if (DebugActiveProcess(process.procID) != 0)
                                {
                                    logger.Info("App Pause Error: {0}", GetLastError().ToString());
                                }
                            }

                            //Save current display setup for re-application later
                            surroundManager.SM_SaveCurrentSetup();
                            if (!surroundManager.SM_ApplySetup(app.SurroundGrid))
                            {
                                if (MessageBox.Show("Surround enable failed!\nWould you like to kill the application?", "Continue", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000) == DialogResult.Yes)
                                {
                                    if (app.PauseOnDetect)
                                    {
                                        if (DebugActiveProcessStop(process.procID) == 0)
                                        {
                                            logger.Debug("App Un-Paused: {0}", GetLastError().ToString());
                                            Thread.Sleep(100);
                                        }
                                    }

                                    try
                                    {
                                        process.process = Process.GetProcessById((int)process.procID);
                                        process.process.Kill();
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
                                logger.Info("Application Detected and Surround Applied: {0}", process.processName);
                                //Add To running application list
                                logger.Debug("Adding Detected application to list: {0}, {1}", process.processName, process.procID);
                                runningApplicationsList.Add(process);

                                if (app.PauseOnDetect)
                                {
                                    if (DebugActiveProcessStop(process.procID) == 0)
                                    {
                                        logger.Debug("App Un-Paused: {0}", GetLastError().ToString());
                                    }
                                }
                            }

                        }

                    }
                    else
                    {                
                        if(app == null)
                            logger.Trace("App not in list: {0}", process.processName);
                    }
#else
                    if (app != null && app.Enabled)
                    {
                        logger.Info("Application Detected and Surround Applied: {0}, {1}", process.processName, process.procID);
                        runningApplicationsList.Add(process);

                        if (DebugActiveProcessStop(process.procID) == 0)
                        {
                            logger.Debug("App Un-Paused: {0}", GetLastError().ToString());
                        }
                    }
#endif
                }
                else
                {
                    logger.Info("Application Detected but already in list: {0}, {1}", process.processName, process.procID);
                    //Add To running application list only if different process id
                    if ((index = runningApplicationsList.FindIndex(r => r.procID == process.procID)) == -1)
                    {
                        logger.Debug("Adding Detected application to list: {0}, {1}", process.processName, process.procID);
                        runningApplicationsList.Add(process);
                    }
                    else
                    {
                        if (processStopTimer.Enabled)
                        {
                            logger.Debug("Stopping processStopTimer: {0}, {1}", process.processName, process.procID);
                            processStopTimer.Stop();
                        }
                    }
                }
            }
            finally
            {
                newProcessMutex.ReleaseMutex();
            }
        }

        private void processStopTimer_OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            ProcessInfo proc = new ProcessInfo();
            int index = 0;
            try
            {
                processStopTimer.Stop();
                if (Interlocked.CompareExchange(ref processStoppedSyncPoint, 1, 0) == 0)
                {
                    while (!StoppedProcessList.IsEmpty)
                    {
                        if (StoppedProcessList.TryDequeue(out proc))
                        {
                            logger.Info("Application exiting: {0}, {1}", proc.processName, proc.procID);
                            //if in running list remove it
                            if ((index = runningApplicationsList.FindIndex(r => r.procID == proc.procID)) != -1)
                            {
                                logger.Debug("Removing from running list: {0} , {1}", proc.processName, proc.procID);
                                runningApplicationsList.RemoveAt(index);
                            }
                        }
                    }
                    //If no other app is in list switch back to normal mode
                    if (runningApplicationsList.Count == 0)
                    {
                        logger.Info("No more running applications, switching called");
                        SwitchToNormalMode((Settings_AskSwitch)NVidia_Surround_Assistant.Properties.Settings.Default.SurroundToNormal_OnExit);
                    }
                    else if (CheckRunningListForZombies())
                    {
                        logger.Debug("Zombie check");
                        logger.Info("No more running applications, switching called");
                        SwitchToNormalMode((Settings_AskSwitch)NVidia_Surround_Assistant.Properties.Settings.Default.SurroundToNormal_OnExit);
                    }
                    else
                    {
                        //Do nothing
                    }
                    // Release control of syncPoint.
                    processStoppedSyncPoint = 0;
                }
            }
            catch (InvalidOperationException)
            {
                // Release control of syncPoint.
                processStoppedSyncPoint = 0;
            }
        }

        //Ahh brains ;D Check that none of the applications listed in running have not been missed somehow
        private bool CheckRunningListForZombies()
        {
            ProcessInfo proc = new ProcessInfo();
            int count = runningApplicationsList.Count;

            newProcessMutex.WaitOne();
            for (int index = 0; index < count; index++)
            {
                proc = runningApplicationsList[index];
                try
                {
                    //Process id should never be 0
                    if ((int)proc.procID != 0)
                        Process.GetProcessById((int)proc.procID);
                    else
                    {
                        runningApplicationsList.RemoveAt(index);
                        count--;
                        index--;
                    }
                }
                //If the process id is no longer running ArgumentException will be thrown thus we can remove it from list
                catch (ArgumentException)
                {
                    logger.Debug("Removing from running list: {0} , {1}", proc.processName, proc.procID);
                    runningApplicationsList.RemoveAt(index);
                    count--;
                    index--;
                }
            }
            newProcessMutex.ReleaseMutex();
            return (runningApplicationsList.Count == 0);
        }

        private void LoggerSetup()
        {
            // Create configuration object 
            LoggingConfiguration config = new LoggingConfiguration();
            NLog.Layouts.Layout layout = @"[${level:uppercase=true}] ${longdate} : ${message}";

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
                    logLevel = LogLevel.Trace;
                    break;
                case 2:
                    logLevel = LogLevel.Debug;
                    break;
                case 3:
                    logLevel = LogLevel.Info;
                    break;
                case 4:
                    logLevel = LogLevel.Error;
                    break;
                case 5:
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

        private void UpdateForm()
        {
            this.SuspendLayout();
            pictureBoxClose.Location = new Point(this.ClientSize.Width - (pictureBoxClose.Width + border_spacing_Form), border_spacing_Form);
            this.ResumeLayout();
        }

        private void UpdateTextBoxLogAndThumbGrid()
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                thumbGridView.SetAutoScroll(false);
                if (textBoxLogs.Visible == true)
                {
                    if((Height + textBoxLogs_PixelShift) < mainForm_minSizeWithLogBox)
                        this.Size = new Size(this.Width, mainForm_minSizeWithLogBox);
                    else
                        this.Size = new Size(this.Width, this.Height + textBoxLogs_PixelShift);
                }
                else
                {
                    if((Height - textBoxLogs_PixelShift) > mainForm_minSizeWithLogBox)
                        this.Size = new Size(this.Width, this.Height - textBoxLogs_PixelShift);
                    else
                        this.Size = new Size(this.Width, mainForm_minSizeWithLogBox - textBoxLogs_PixelShift);
                }
                thumbGridView.SetAutoScroll(true);
                thumbGridView.ResetScrollBar(0);
            }
        }        

        private void populateContextMenuStripLoadApp()
        {
            ToolStripMenuItem toolStripButton;

            contextMenuStripLoadApp.AutoSize = false;
            contextMenuStripLoadApp.SuspendLayout();
            if (contextMenuStripLoadApp.Items.Count > 0)
                contextMenuStripLoadApp.Items.Clear();

            foreach (ApplicationInfo app in sqlInterface.GetApplicationList())
            {
                toolStripButton = new ToolStripMenuItem(app.DisplayName, null, contextMenuStripLoadApp_Click);
                toolStripButton.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
                toolStripButton.BackColor = normalControlColor;
                toolStripButton.Tag = app;
                contextMenuStripLoadApp.Items.Add(toolStripButton);
            }
            contextMenuStripLoadApp.ResumeLayout();
            contextMenuStripLoadApp.AutoSize = true;
        }

        #region GUI Tweaks
        private class NvsaRenderer : ToolStripProfessionalRenderer
        {
            protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs myMenu)
            {
                if (!myMenu.Item.Selected)
                    base.OnRenderMenuItemBackground(myMenu);
                else
                {
                    Rectangle menuRectangle = new Rectangle(Point.Empty, myMenu.Item.Size);
                    //Fill Color
                    myMenu.Graphics.FillRectangle(Brushes.DarkGreen, menuRectangle);
                    // Border Color
                    myMenu.Graphics.DrawRectangle(Pens.Lime, 1, 0, menuRectangle.Width - 2, menuRectangle.Height - 1);
                }
            }
        }
        #endregion

        #region controls
        private void PictureBoxAddGame_Click(object sender, EventArgs e)
        {
            ApplicationInfo newApp = new ApplicationInfo();

            openFileDialog.InitialDirectory = Path.GetPathRoot(Application.ExecutablePath);
            openFileDialog.FilterIndex = 2;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                newApp.DisplayName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);                
                newApp.FullPath = Path.GetFullPath(openFileDialog.FileName);
                newApp.ProcessName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);                

                AddNewApplication(newApp);
                if (MessageBox.Show(String.Format("Would you like to launch {0}?", newApp.ProcessName), "Launch Application", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    LaunchApplication(newApp);
            }
        }

        private void pictureBoxSettings_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();

            settings.ShowDialog();
        }

        private void toolStripMenuItem_ToggelSurround_Click(object sender, EventArgs e)
        {
            surroundManager.SM_SwitchSurround();
        }

        private void toolStripMenuItem_AddApp_Click(object sender, EventArgs e)
        {
            PictureBoxAddGame_Click(null, null);
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
            thumbGridView.SetAutoScroll(true);
        }

        private void SystemTrayIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            this.WindowState = FormWindowState.Normal;
            thumbGridView.SetAutoScroll(true);
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                thumbGridView.SetAutoScroll(false);
                Hide();
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
            }
            else
            {
                textBoxLogs.Visible = true;
            }
        }        

        private void saveAsDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            surroundManager.SM_SaveDefaultSetup();
        }

        private void saveAsDefaultSurroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            surroundManager.SM_SaveDefaultSurroundSetup();
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SurroundConfigSaveAsPopup popup = new SurroundConfigSaveAsPopup();

            if (popup.ShowDialog() == DialogResult.OK)
            {
                surroundManager.SM_SaveCurrentSetup(popup.surroundConfigName);
            }
        }        

        private void contextMenuStripLoadApp_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
                if (toolStripMenuItem.Tag != null)
                    LaunchApplication(toolStripMenuItem.Tag as ApplicationInfo);
            }
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            quitApplication = true;
            Close();
        }

        private void pictureBoxSwitchSurround_Click(object sender, EventArgs e)
        {
            surroundManager.SM_SwitchSurround();
        }

        private void MainForm_Layout(object sender, LayoutEventArgs e)
        {
            if (formShown)
                UpdateForm();
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if (y_spacing_logBox == 0 || border_spacing_Form == 0)
            {
                //Get Spacing and sizes for resizes                
                y_spacing_logBox = ClientSize.Height - textBoxLogs.Bottom;
                border_spacing_Form = pictureBoxClose.Top;
                textBoxLogs_PixelShift = y_spacing_logBox + textBoxLogs.Height;

                textBoxLogs.Visible = NVidia_Surround_Assistant.Properties.Settings.Default.ShowLogs;
                thumbGridView.ResetScrollBar(0);
                //Start minimized 
                if (NVidia_Surround_Assistant.Properties.Settings.Default.StartMinimized)
                    WindowState = FormWindowState.Minimized;
                formShown = true;
            }
        }

        private void textBoxLogs_VisibleChanged(object sender, EventArgs e)
        {
            UpdateTextBoxLogAndThumbGrid();
        }

        private void pictureBoxAbout_Click(object sender, EventArgs e)
        {
            AboutBoxNVSA about = new AboutBoxNVSA();
            about.Show();
        }

        private void timerZombieCheck_Tick(object sender, EventArgs e)
        {
            logger.Debug("Running Zombie process check");
            CheckRunningListForZombies();
            timerZombieCheck.Stop();
        }        

        private void pictureBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            pictureBox.BackColor = hoverButtonColor;
        }

        private void pictureBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            pictureBox.BackColor = normalControlColor;
        }
        #endregion
    }
}
