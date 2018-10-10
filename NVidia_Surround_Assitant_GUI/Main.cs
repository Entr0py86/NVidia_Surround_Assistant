#define CLR

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
using MyStuff;

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
        static public SurroundManager surroundManager = new SurroundManager(); //TOOD NB NB this needs to be come thread safe. Currently not thread safe

        string binDir;

        //List of all registered windows via the hooks
        List<RegisteredWindowInfo> registeredWindows = new List<RegisteredWindowInfo>();
        //Thread safe queue used for new process events (created/destroyed)
        ConcurrentQueue<ProcessEvent> newProcessEventQueue = new ConcurrentQueue<ProcessEvent>();
        //List used to add all stopped processes that are in the applicationDetectList
        ConcurrentQueue<ProcessInfo> StoppedProcessList = new ConcurrentQueue<ProcessInfo>();
        //Hash list use to store names of applications that should trigger surround mode
        HashSet<String> applicationDetectList = new HashSet<string>();
        //Hash list use to store names of applications that should trigger surround mode.
        Semaphore runningApplicationsListSempahore = new Semaphore(initialCount: 1, maximumCount: 1);
        List<ProcessInfo> runningApplicationsList = new List<ProcessInfo>();
        //List off all registered applications
        List<ApplicationInfo> applicationList = new List<ApplicationInfo>();

        //Process creation detectors
        HookManager hookManager;
        //WMI Event watchers
        ManagementEventWatcher processStartEvent;
        ManagementEventWatcher processStopEvent;

        //Timeout form used to allow visual notification as well as a method to cancel the switch.
        ApplicationClosedWaitTimeout processDestroyedTimer = new ApplicationClosedWaitTimeout();
        //Atomic variable used to ensure only one event gets access to the ProcessCreated Function
        bool processDetectedBusy = false;

        //Start timer used as time buffer so that process events get queued but otherwise ignored. Required so that any applications that start and stop to launch diffrent versions of itself from causing a switch back to non-surround. 
        System.Timers.Timer timerStartWait = new System.Timers.Timer();
        //Zombie timer used to periodically check that application in the running list has been missed by the detection methods.
        System.Timers.Timer timerZombieCheck = new System.Timers.Timer();        

        //reset event used to wait for thread to exit before closing app
        private AutoResetEvent _resetEvent = new AutoResetEvent(false);
        private AutoResetEvent _newProcessEvent = new AutoResetEvent(false);

        int border_spacing_Form = 0;
        int y_spacing_logBox = 0;

        int textBoxLogs_PixelShift = 0;
        int mainForm_minSizeWithLogBox = 668;

        //Color for context menu
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

            //Add Version to title
            Text += String.Format("Version {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());

            //Setup zombie check timer
            this.timerZombieCheck.Interval = 15000;
            this.timerZombieCheck.Elapsed += timerZombieCheck_Tick;

            //Setup start wait timer
            this.timerStartWait.Interval = 5000;
            this.timerStartWait.Elapsed += timerStartWait_Tick;

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

            //Init Surround Manager
            if(!surroundManager.SM_Initialize())
            {
                MyMessageBox.Show("The application cannot work without the surround manager. Please fix issue before trying again.", "Critical");
                Close();
                return;
            }
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
                //Stop WMI event manager
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
                if (surroundManager.surroundSetupLoaded)
                {
                    //Switch back to normal mode depending on settings
                    SwitchToNormalMode((Settings_AskSwitch)NVidia_Surround_Assistant.Properties.Settings.Default.SurroundToNormal_OnClose);
                }
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
                processEventWorker.CancelAsync();
                _newProcessEvent.Set();
                //Wait for signal that do work has completed
                _resetEvent.WaitOne();
            }
            //Close db connections
            sqlInterface.SQL_CloseConnection();

            _newProcessEvent.Dispose();
            _resetEvent.Dispose();
        }

        public IntPtr hWnd
        {
            get { return base.Handle; }
        }

        void Initialize()
        {
            //Install the hooks
            hookInstalled = hookManager.InstallHooksAndRegisterWindows();
            //Start message process thread
            if (!processEventWorker.IsBusy)
            {
                processEventWorker.RunWorkerAsync();
            }

            //Load all applications into list
            LoadApplicationList();
            populateContextMenuStripLoadApp();
            populateContextMenuStripLoadSurroundConfig();

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
                    RemoveFromApplicatoionDetectList(AppThumb.ApplicationInfo);
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

        void RemoveFromApplicatoionDetectList(ApplicationInfo App)
        {
            if (App.ProcessName == App.DisplayName)
            {
                applicationDetectList.Remove(App.DisplayName);
            }
            else
            {
                applicationDetectList.Remove(App.DisplayName);
                applicationDetectList.Remove(App.ProcessName);
            }
        }

        bool AddNewApplication(ApplicationInfo newApp)
        {
            bool result = false;
            EditApplicationSettings editWindow = new EditApplicationSettings(newApp, true);
            editWindow.Text = "Add New Application";

            //Check if application already in list
            if (applicationDetectList.Count > 0 && applicationDetectList.Contains(newApp.ProcessName))
            {
                result = EditApplication(GetApplicationFromList(newApp.ProcessName));
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
                        result = true;
                    }
                    else
                    {
                        MessageBox.Show("Error adding application to database", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return result;
        }

        void RegisterApplication(ApplicationInfo App)
        {
            Thumb newThumb = new Thumb(App);
            newThumb.launchApplication += LaunchApplication;
            newThumb.removeApplication += DeleteApplicationFromList;
            newThumb.editApplication += EditApplication;
            newThumb.silentEditApplication += SilentEditApplication;

            //Add data to lists    
            AddToApplicatoionDetectList(App);
            thumbGridView.AddThumb(newThumb);
        }

        void AddToApplicatoionDetectList(ApplicationInfo App)
        {
            if (App.ProcessName == App.DisplayName)
            {
                applicationDetectList.Add(App.ProcessName);
            }
            else
            {
                applicationDetectList.Add(App.DisplayName);
                applicationDetectList.Add(App.ProcessName);
            }
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

        bool EditApplication(ApplicationInfo AppInfo)
        {
            bool result = false;
            EditApplicationSettings editWindow = new EditApplicationSettings(AppInfo, false);

            if (editWindow.ShowDialog() == DialogResult.OK)
            {
                AppInfo = editWindow.AppInfo;
                if (sqlInterface.UpdateApplication(editWindow.AppInfo))
                {
                    result = true;
                    logger.Info("Edit Application: {0} edited", AppInfo.DisplayName);
                }
                else
                {
                    logger.Error("Edit Application: {0} edited", AppInfo.DisplayName);
                }
            }
            return result;
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

            //Only launch if applciation exists
            if (File.Exists(launchApp.FullPath))
            {
                logger.Info("Manual Application Launch: {0}", launchApp.DisplayName);
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

                //Remove from detect list so that ProcessCreatedWindow is not called while app is starting
                RemoveFromApplicatoionDetectList(launchApp);
                //Populate options to start the application
                startProcess.StartInfo.FileName = launchApp.FullPath;
                try
                {                    
                    timerStartWait.Interval = launchApp.StartTimeout * 1000;
                    timerStartWait.Start();
                    logger.Debug("timerStartWait interval, {0}, started", timerStartWait.Interval);
                    //Start process
                    startProcess.Start();
                    //populate required info for list
                    process.processName = Path.GetFileNameWithoutExtension(launchApp.FullPath);
                    process.procID = (IntPtr)startProcess.Id;

                    //Add To running application list
                    runningApplicationsListSempahore.WaitOne();
                    runningApplicationsList.Add(process);
                    runningApplicationsListSempahore.Release();

                    //Add it back for future detections
                    AddToApplicatoionDetectList(launchApp);
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    logger.Error("Error launching application: {0}", ex.Message);
                }
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
            //Empty the queue if there is something in it. If we are switching back then no application in our list are running anymore
            if(!StoppedProcessList.IsEmpty)
            {
                StoppedProcessList = new ConcurrentQueue<ProcessInfo>();
            }
        }

        /// <summary>
        /// Create the WMI event watchers for created/destroyed processes
        /// </summary>
        void CreateProcessWatchers()
        {
            //Create a new WMI event watcher for any started processes
            processStartEvent = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            processStartEvent.EventArrived += new EventArrivedEventHandler(processStartEvent_EventArrived);
            processStartEvent.Start();

            //Create a new WMI event watcher for any destroyed processes
            processStopEvent = new ManagementEventWatcher("SELECT * FROM Win32_ProcessStopTrace");
            processStopEvent.EventArrived += new EventArrivedEventHandler(processStopEvent_EventArrived);
            processStopEvent.Start();
        }

        //WMI Event is fired when a new process is created
        void processStartEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            ProcessEvent newProcessEvent = new ProcessEvent();
            //Get info from the event
            newProcessEvent.procID = new IntPtr(Convert.ToInt64(e.NewEvent.Properties["ProcessID"].Value));
            newProcessEvent.processName = Path.GetFileNameWithoutExtension(e.NewEvent.Properties["ProcessName"].Value as string);
            logger.Trace("WMI Process started: {0} ", newProcessEvent.processName);

            //Post to queue
            newProcessEvent.eventType = HookType.windowCreate;
            newProcessEventQueue.Enqueue(newProcessEvent);
            if (!formClosing)//Only set event if the form is not going to close
                _newProcessEvent.Set();
        }

        //WMI Event is fired when a process is destroyed
        void processStopEvent_EventArrived(object sender, EventArrivedEventArgs e)
        {
            ProcessEvent newProcessEvent = new ProcessEvent();
            //Get info from the event
            newProcessEvent.procID = new IntPtr(Convert.ToInt64(e.NewEvent.Properties["ProcessID"].Value));
            newProcessEvent.processName = Path.GetFileNameWithoutExtension(e.NewEvent.Properties["ProcessName"].Value as string);
            logger.Trace("WMI Process stopped: {0} ", newProcessEvent.processName);

            //Post to queue
            newProcessEvent.eventType = HookType.windowDestroy;
            newProcessEventQueue.Enqueue(newProcessEvent);
            if (!formClosing)//Only set event if the form is not going to close
                _newProcessEvent.Set();
        }        

        //Get the window name from the window handle
        private void GetWindowHandleInfo(IntPtr hWnd, ref ProcessEvent process)
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

        //Handle all WIN32 window messages
        protected override void WndProc(ref Message msg)
        {
            int index = -1;
            Boolean handled = false;
            ProcessEvent newProcessEvent = new ProcessEvent();
            uint msgId = (uint)msg.Msg;

            msg.Result = IntPtr.Zero;

            //if the message id matches a registered window message add that message to the queue
            if ((index = registeredWindows.FindIndex(r => r.windowRegisterID.Equals(msgId))) != -1)
            {
                //Get all available info for the process that fired the event
                GetWindowHandleInfo(msg.WParam, ref newProcessEvent);
                logger.Trace("Hook captured: {0} Hook Type: {1}", newProcessEvent.processName, registeredWindows[index].type.ToString());

                switch (registeredWindows[index].type)
                {
                    case HookType.windowCreate:
                        newProcessEvent.eventType = HookType.windowCreate;
                        newProcessEventQueue.Enqueue(newProcessEvent);
                        break;
                    case HookType.windowDestroy:
                        newProcessEvent.eventType = HookType.windowDestroy;
                        newProcessEventQueue.Enqueue(newProcessEvent);                        
                        break;
                    default:
                        break;
                }                

                //Complete processing of message
                handled = true;
                msg.Result = new IntPtr(1);

                //Signal the worker that new work is available
                if (!formClosing)//Only set event if the form is not going to close
                    _newProcessEvent.Set();
            }
            if (handled)
                DefWndProc(ref msg);
            else
                base.WndProc(ref msg);
        }
                
        //Consume events from queue. Worker used to allow WIN32 queues not to be held up by processing
        private void processEventWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ProcessEvent process;
            BackgroundWorker thisWorker = sender as BackgroundWorker;

            logger.Debug("Message process thread running");
            while (!thisWorker.CancellationPending)
            {
                if (newProcessEventQueue.Count > 1)
                {
                    newProcessEventQueue.TryDequeue(out process);
                    thisWorker.ReportProgress(1, process);
                    if (!formClosing)//Only reset event if the form is not going to close
                        _newProcessEvent.Reset();
                }
                else
                {
                    _newProcessEvent.WaitOne();
                }
            }
            e.Cancel = true;
            //Signal to the destructor that do work has completed
            _resetEvent.Set();
        }        
        
        //Distribute events to correct functions
        private void processEventWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProcessEvent process = (ProcessEvent)e.UserState;
            if (process == null) return;
            
            if (applicationDetectList.Contains(process.processName) && (int)process.procID != 0)
            {
                switch(process.eventType)
                {
                    case HookType.windowCreate:
                        logger.Debug("Window Created: App From list detected: {0} : {1}", process.processName, (int)process.procID);
                        //Disable stopTimer before continuing
                        if (processDestroyedTimer.TimerEnabled)
                        {
                            logger.Debug("processDestroyedTimer stopped: {0} : {1}", process.processName, (int)process.procID);
                            processDestroyedTimer.CancelTimerAndClose();
                        }
                        //Run Nvidia surround check
                        ProcessCreated(process);
                        break;
                    case HookType.windowDestroy:
                        logger.Debug("Window Destroyed: App From list detected: {0} : {1}", process.processName, (int)process.procID);
                        ProcessDestroyed(process);
                        break;
                    default:
                        logger.Warn("Unknown Event: App From list detected: {0} : {1}", process.processName, (int)process.procID);
                        break;
                }                
            }
        }

        private void ProcessCreated(ProcessInfo process)
        {
            int index = 0;
            ApplicationInfo app;
            //Program suspension has multiple methods. I chose the one that seemed to always work and was simple. Read this post https://stackoverflow.com/questions/11010165/how-to-suspend-resume-a-process-in-windows
            try
            {
                runningApplicationsListSempahore.WaitOne();
                //if the message id does not match then process
                if (!processDetectedBusy && !timerStartWait.Enabled)
                {
                    processDetectedBusy = true;    
                    //is message already in running list
                    if ((index = runningApplicationsList.FindIndex(r => r.processName == process.processName)) == -1)
                    {
                        //Get application settings
                        app = GetApplicationFromList(process.processName);

                        if (app != null && app.Enabled)
                        {
                            //Check if surround already active
                            if (!surroundManager.SM_IsSurroundActive(app.SurroundGrid))
                            {
                                logger.Debug("loading surround id {0} for app {1}", app.SurroundGrid, app.ProcessName);
                                //If settings setup for application pause
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
                                //Apply surround settings
                                if (!surroundManager.SM_ApplySetup(app.SurroundGrid))
                                {
                                    if (MessageBox.Show("Surround enable failed!\nWould you like to kill the application?", "Continue", MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000) == DialogResult.Yes)
                                    {
                                        if (app.PauseOnDetect)
                                        {
                                            if (DebugActiveProcessStop(process.procID) == 0)
                                            {
                                                logger.Debug("App Un-Paused Error: {0}", GetLastError().ToString());
                                            }
                                        }
                                        //Kill the application
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
                                    else
                                    {
                                        if (app.PauseOnDetect)
                                        {
                                            if (DebugActiveProcessStop(process.procID) == 0)
                                            {
                                                logger.Debug("App Un-Paused Error: {0}", GetLastError().ToString());
                                            }
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
                                            logger.Debug("App Un-Paused Error: {0}", GetLastError().ToString());
                                        }
                                    }
                                }

                            }
                            logger.Debug("ProcessCreatedWindow: processDetectedBusy = false", process.processName);
                            timerStartWait.Interval = app.StartTimeout * 1000;
                            timerStartWait.Start();
                        }
                        else
                        {
                            if (app == null)
                                logger.Trace("App not in detection list: {0}", process.processName);
                        }
                    }
                    else
                    {
                        //Process name already in list. Check if proc Id is the same
                        ProcessAlreadyInList(process);
                    }
                    processDetectedBusy = false;
                }
                else
                {
                    //Call to process create already busy. add to list if proc Id is not in list
                    ProcessAlreadyInList(process);
                }
            }
            catch (Exception ex)
            {
                logger.Error("ProcessCreatedWindow: {0}", ex.Message);
                processDetectedBusy = false;
            }
            finally
            {
                runningApplicationsListSempahore.Release();
            }
        }

        private void ProcessDestroyed(ProcessInfo processInfo)
        {
            ApplicationInfo applicationInfo;

            //Is the window in the registered list
            if (applicationDetectList.Contains(processInfo.processName) && IsProcessDead(processInfo))
            {
                //Add process to stop queue
                applicationInfo = GetApplicationFromList(processInfo.processName);
                StoppedProcessList.Enqueue(processInfo);
                //Has the app gone past the set start wait time
                if (!timerStartWait.Enabled)
                {
                    if (!processDetectedBusy && !processDestroyedTimer.TimerEnabled)
                    {
                        logger.Debug("WMI Process stopped: {0}; Switch in {1} sec", processInfo.processName, applicationInfo.SwitchbackTimeout);
                        logger.Debug("processStopEvent_EventArrived: Stop Timer Started");
                        timerZombieCheck.Stop();
                        processDestroyedTimer.StartTimer(applicationInfo.SwitchbackTimeout * 1000);//Make seconds into ms and start timer
                        processDestroyedTimer.ShowDialog();
                        if(!processDestroyedTimer.TimerCanceled)
                        {
                            FinalizeProcessDestroyed();
                        }
                    }
                }
            }
        }

        private void ProcessAlreadyInList(ProcessInfo process)
        {
            int index = 0;
            try
            {
                logger.Info("Application Detected but already in list: {0}, {1}", process.processName, process.procID);
                //Add To running application list only if different process id
                if ((index = runningApplicationsList.FindIndex(r => r.procID == process.procID)) == -1)
                {
                    logger.Debug("Adding Detected application to list: {0}, {1}, Count {2}", process.processName, process.procID, runningApplicationsList.Capacity);
                    runningApplicationsList.Add(process);
                }
                else
                {
                    if (processDestroyedTimer.TimerEnabled)
                    {
                        logger.Debug("Stopping processDestroyedTimer: {0}, {1}", process.processName, process.procID);
                        processDestroyedTimer.CancelTimerAndClose();
                    }
                }
                if (!timerZombieCheck.Enabled)
                {
                    timerZombieCheck.Start();
                }
            }
            catch (Exception ex)
            {
                logger.Error("ProcessCreatedWindow: {0}", ex.Message);
            }
        }
        
        private bool IsProcessDead(ProcessInfo proc)
        {
            bool isDead = true;
            Process[] procList;

            try
            {
                //Check process id is still runnning
                if ((int)proc.procID != 0)
                {
                    Process.GetProcessById((int)proc.procID);
                    isDead = false;
                }
                else
                    isDead = true;
            }
            //If the process id is no longer running ArgumentException will be thrown thus we can remove it from list
            catch (ArgumentException)
            { }            
            
            //Check that the proccess name is not still running
            procList = Process.GetProcessesByName(proc.processName);
            if (procList.Length > 0)
            {
                isDead = false;
            }
            else
            {
                isDead = true;
            }            

            return isDead;
        }

        private void FinalizeProcessDestroyed()
        {
            ProcessInfo proc = new ProcessInfo();
            int index = 0;
            try
            {                
                while (!StoppedProcessList.IsEmpty)
                {
                    if (StoppedProcessList.TryDequeue(out proc))
                    {
                        logger.Info("Application exiting: {0}, {1}", proc.processName, proc.procID);
                        //if in running list remove it
                        runningApplicationsListSempahore.WaitOne();
                        if ((index = runningApplicationsList.FindIndex(r => r.procID == proc.procID)) != -1)
                        {
                            logger.Debug("Removing from running list: {0} , {1}, Count {2}", proc.processName, proc.procID, runningApplicationsList.Capacity);
                            runningApplicationsList.RemoveAt(index);
                        }
                        runningApplicationsListSempahore.Release();
                    }
                }
                CheckRunningListForZombies();
                //If no other app is in list switch back to normal mode
                if (runningApplicationsList.Count == 0 && !timerStartWait.Enabled)
                {
                    if (timerZombieCheck.Enabled)
                    {
                        timerZombieCheck.Stop();
                    }
                    logger.Info("No more running applications, switching called");
                    SwitchToNormalMode((Settings_AskSwitch)NVidia_Surround_Assistant.Properties.Settings.Default.SurroundToNormal_OnExit);
                }
                else if (!timerStartWait.Enabled && !timerZombieCheck.Enabled)
                {
                    logger.Debug("timerZombieCheck start, interval {0}", timerZombieCheck.Interval.ToString());
                    timerZombieCheck.Start();
                }
            }
            catch (InvalidOperationException)
            {
                runningApplicationsListSempahore.Release();
            }
        }
        
        //Process the closed/destryed apllications
        private void processStopTimer_Tick(Object source, System.Timers.ElapsedEventArgs e)
        {
            FinalizeProcessDestroyed();
        }

        private void timerStartWait_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            logger.Debug("timerStartWait_Tick");
            timerStartWait.Stop();
            timerZombieCheck.Start();
        }

        private void timerZombieCheck_Tick(object sender, System.Timers.ElapsedEventArgs e)
        {
            logger.Trace("Running Zombie process check tick");
            //Only run zombie check if all the timers are not active
            if (!processDestroyedTimer.TimerEnabled && !timerStartWait.Enabled && CheckRunningListForZombies())
            {
                logger.Debug("Zombie check complete");
                logger.Info("No more running applications, switching called");
                SwitchToNormalMode((Settings_AskSwitch)NVidia_Surround_Assistant.Properties.Settings.Default.SurroundToNormal_OnExit);
                timerZombieCheck.Stop();
            }
        }

        //Ahh brains ;D Check that none of the applications listed in running have not been missed somehow
        private bool CheckRunningListForZombies()
        {
            ProcessInfo proc = new ProcessInfo();
            int count = runningApplicationsList.Count;

            logger.Debug("Zombie Check: Running Application list count {0}", count);
            if (count > 0)
            {
                runningApplicationsListSempahore.WaitOne();                
                for(int index = 0; index < count; index++)
                {
                    proc = runningApplicationsList[index];
                    //Check if proc is dead if so remove from list
                    if(IsProcessDead(proc))
                    {                        
                        runningApplicationsList.RemoveAt(index);                        
                        index--;
                        logger.Debug("Removing from running list: {0} , {1} : Running list count {2}", proc.processName, proc.procID, runningApplicationsList.Count);
                    }
                    
                    if(index < 0)
                    {
                        index = 0;
                    }
                    if(count != runningApplicationsList.Count && runningApplicationsList.Count > 0)
                    {
                        count = runningApplicationsList.Count;                        
                    }
                    
                }
                runningApplicationsListSempahore.Release();
            }
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
                    if ((Height + textBoxLogs_PixelShift) < mainForm_minSizeWithLogBox)
                        this.Size = new Size(this.Width, mainForm_minSizeWithLogBox);
                    else
                        this.Size = new Size(this.Width, this.Height + textBoxLogs_PixelShift);
                }
                else
                {
                    if ((Height - textBoxLogs_PixelShift) > mainForm_minSizeWithLogBox)
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

                if (AddNewApplication(newApp))
                {
                    if (MessageBox.Show(String.Format("Would you like to launch {0}?", newApp.ProcessName), "Launch Application", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                        LaunchApplication(newApp);
                }
            }
        }

        private void pictureBoxSettings_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();

            settings.ShowDialog();
            textBoxLogs.Visible = NVidia_Surround_Assistant.Properties.Settings.Default.ShowLogs;

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

        private void populateContextMenuStripLoadSurroundConfig()
        {
            ToolStripMenuItem toolStripButton;

            contextMenuStripLoadSurroundConfig.AutoSize = false;
            contextMenuStripLoadSurroundConfig.SuspendLayout();
            if (contextMenuStripLoadSurroundConfig.Items.Count > 0)
                contextMenuStripLoadSurroundConfig.Items.Clear();

            foreach (SurroundConfig surroundConfig in sqlInterface.GetSurroundConfigList())
            {
                toolStripButton = new ToolStripMenuItem(surroundConfig.Name, null, contextMenuStripLoadSurroundConfig_Click);
                toolStripButton.ForeColor = System.Drawing.SystemColors.GradientInactiveCaption;
                toolStripButton.BackColor = normalControlColor;
                toolStripButton.Tag = surroundConfig;
                contextMenuStripLoadSurroundConfig.Items.Add(toolStripButton);
            }
            contextMenuStripLoadSurroundConfig.ResumeLayout();
            contextMenuStripLoadSurroundConfig.AutoSize = true;
        }

        private void contextMenuStripLoadSurroundConfig_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem)
            {
                ToolStripMenuItem toolStripMenuItem = sender as ToolStripMenuItem;
                if (toolStripMenuItem.Tag != null)
                    surroundManager.SM_ApplySetup((toolStripMenuItem.Tag as SurroundConfig).Id);
            }
        }

        private void saveAsDefaultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (surroundManager.SM_SaveDefaultSetup())
            {
                MyMessageBox.Show("Default setup saved succesfully.");
            }
        }

        private void saveAsDefaultSurroundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (surroundManager.SM_SaveDefaultSurroundSetup())
            {
                MyMessageBox.Show("Default setup saved succesfully.");
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SurroundConfigSaveAsPopup popup = new SurroundConfigSaveAsPopup();

            if (popup.ShowDialog() == DialogResult.OK)
            {
                if (surroundManager.SM_SaveCurrentSetup(popup.surroundConfigName))
                {
                    MyMessageBox.Show("Default setup saved succesfully.");
                }
            }
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
