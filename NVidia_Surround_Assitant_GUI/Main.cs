#define CLR

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using DllWrapper;
using System.Runtime.ConstrainedExecution;
using System.Security;
using System.Security.Principal;
using System.IO;
using System.Net;

using System.Data.SQLite;
using NLog;
using NLog.Windows.Forms;
using Display_Manager;

/*  TODO List:
 * Re-Install hooks after sleep, first make sure that it is the problem
 * investigate why the hook stops responding after a time. Assuming its the above problem.
 * Add surround profiles that can be applied on application basis
 
 */

namespace NVidia_Surround_Assistant
{
    public partial class MainForm : Form
    {
        #region DLL_Imports
        #region advapi32_dll
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern bool CreateProcessAsUser(
        IntPtr hToken,
        string lpApplicationName,
        string lpCommandLine,
        ref SECURITY_ATTRIBUTES lpProcessAttributes,
        ref SECURITY_ATTRIBUTES lpThreadAttributes,
        bool bInheritHandles,
        uint dwCreationFlags,
        IntPtr lpEnvironment,
        string lpCurrentDirectory,
        ref STARTUPINFO lpStartupInfo,
        out PROCESS_INFORMATION lpProcessInformation);

        #endregion
        #region User32_dll
        //User32.dll  PInvoke Calls
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint RegisterWindowMessage(string lpString);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, IntPtr ProcessId);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out IntPtr lpdwProcessId);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd, uint msg, ChangeWindowMessageFilterExAction action, ref CHANGEFILTERSTRUCT changeInfo);

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
        #region HookDLL_dll
        [DllImport("HookDLL.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        static extern int InstallHook(IntPtr hWnd, ref HookId id);

        [DllImport("HookDLL.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        static extern int UnInstallHook(IntPtr hWnd, ref HookId id);

        [DllImport("HookDLL.dll", SetLastError = true, CharSet = CharSet.Unicode, CallingConvention = CallingConvention.Cdecl)]
        static extern int UnInstallAllHooks();
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

        //Sql
        SQLiteConnection m_dbConnection = null;
        SQLiteCommand command;

        //Logger
        Logger logger = LogManager.GetLogger("nvsaLogger");
        
        //reset event used to wait for thread to exit before closing app
        private AutoResetEvent _resetEvent = new AutoResetEvent(false);
        private AutoResetEvent _newMsgEvent = new AutoResetEvent(false);

        //Form that receives all messages and adds them to the queue
        MessageInfo msgTemp = new MessageInfo();
        int y_spacing;

        public MainForm()
        {
            logger.Debug("Application Started {0}", Application.ExecutablePath);
            InitializeComponent();

            binDir = Path.GetDirectoryName(Application.ExecutablePath);            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //Check the operating system and the application being run
            if(Environment.Is64BitProcess && !Environment.Is64BitOperatingSystem)
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

            if (true)//todo make setting for logs view
            {
                pictureBoxLogs_Click(null, null);
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
            if (!File.Exists(NVidia_Surround_Assistant.Properties.Settings.Default.SurroundSetupFileName) || !File.Exists(NVidia_Surround_Assistant.Properties.Settings.Default.DefaultSetupFileName))
                SaveSetupFiles();
            
            //Initialize application
            Initialize();
            //Get Y Spacing for resizes
            y_spacing = Height - textBoxLogs.Bottom;

            //Re-init richtext for NLog
            RichTextBoxTarget.ReInitializeAllTextboxes(this);

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
                int exitValue = UnInstallAllHooks();
                logger.Debug("Hook uninstall value: {0}", exitValue);
                //Cancel background worker
                hookMessageCatcher.CancelAsync();
                _newMsgEvent.Set();
                //Wait for signal that dowork has compeleted
                _resetEvent.WaitOne();
            }
            //Close db connections
            SQL_CloseConnection();

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
            hookInstalled = InstallHooksAndRegisterWindows();
            //Open SQL conection to db
            SQL_OpenConnection(binDir + "\\cfg\\nvsa_db.sqlite");
            //Init and load the surround config
            
            //Load all applications into list
            LoadApplicationList();           
        }

        public bool SaveSetupFiles()
        {
            string SurroundSetupFileName = binDir + "\\cfg\\Default Surround Setup.nvsa";
            string SetupFileName = binDir + "\\cfg\\Default Setup.nvsa";
            bool skipSurround = false;
            bool skipDefault = false;
            bool nonSurroundSaved = false;
                
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
                if (surroundManager.SM_IsSurroundActive())
                {                                        
                    MessageBox.Show("Please disable NVidia Surround via NVidia control panel now.\n\nWhen surround is deactivated and setup to your liking, press OK", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (!surroundManager.SM_IsSurroundActive())
                {
                    if (MessageBox.Show("Surround deactivated.\n\nSave current default setup?", "Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        //Save memory to file
                        surroundManager.SM_SaveCurrentSetup(SetupFileName);                        
                    }
                    else
                    {
                        MessageBox.Show("Default setup not saved.\nPlease re-run setup for proper operation of application.");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Surround not deactivated, setup not saved.\nPlease re-run setup.");
                    return false;
                }
            }

            if (!skipSurround)
            {
                if (!surroundManager.SM_IsSurroundActive())
                {
                    //Save current dispaly setup for re-apllication later
                    surroundManager.SM_SaveCurrentSetup();
                    surroundManager.SM_SaveWindowPositions();
                    nonSurroundSaved = true;
                    MessageBox.Show("Please setup and enable NVidia Surround via NVidia control panel now.\n\nWhen surround is active and setup to your liking, press OK", "Setup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                if (surroundManager.SM_IsSurroundActive())
                {
                    if (MessageBox.Show("Surround active.\n\nSave current surround setup?", "Setup", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        //Save memory to file
                        surroundManager.SM_SaveCurrentSetup(SurroundSetupFileName);
                        if (nonSurroundSaved)
                        {
                            surroundManager.SM_ApplySetupFromMemory(false);
                            surroundManager.SM_ApplyWindowPositions();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Default surround setup not saved.\nPlease re-run setup for proper operation of application.");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Surround not active, setup not saved.\nPlease re-run setup.");
                    return false;
                }
            }

            NVidia_Surround_Assistant.Properties.Settings.Default.SurroundSetupFileName = SurroundSetupFileName;
            NVidia_Surround_Assistant.Properties.Settings.Default.DefaultSetupFileName = SetupFileName;

            return true;
        }       

        void LoadApplicationList()
        {
            SQLiteDataReader reader = SQL_ExecuteQuery("SELECT * FROM ApplicationList");
            if (reader != null)
            {
                if (reader.VisibleFieldCount > 0)
                {
                    while (reader.Read())
                    {
                        try
                        {
                            RegisterApplication(new ApplicationInfo
                            {
                                Id = (int)reader.GetInt32(reader.GetOrdinal("id")),
                                Enabled = (bool)reader["enabled"],
                                DisplayName = (string)reader["DisplayName"],
                                FullPath = (string)reader["fullPath"],
                                Image = new Bitmap(ByteToImage((byte[])reader["image"]))
                            });
                        }
                        catch (System.InvalidCastException ex)
                        {
                            logger.Debug("Invalid Cast: {0}", ex.Message);
                        }
                    }
                }
            }
        }

        void DeleteApplicationFromList(Thumb AppThumb)
        {
            if (MessageBox.Show("Delete from database?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                SQLiteParameter[] parameters = { new SQLiteParameter("@id", AppThumb.Id) };
                if (SQL_ExecuteNonQuery("DELETE FROM ApplicationList WHERE id = @id", parameters) > 0)
                {
                    //Remove data from lists    
                    logger.Info("Delete Application: {0} deleted from library", AppThumb.DisplayName);
                    applicationDetectList.Remove(AppThumb.FullPath);
                    thumbGridView.RemoveThumb(AppThumb);                    
                }
            }
            else
            {
                SQLiteParameter[] parameters = { new SQLiteParameter("@Enabled", false), new SQLiteParameter("@id", AppThumb.Id) };
                int rowsAffected = SQL_ExecuteNonQuery("UPDATE ApplicationList SET enabled = @Enabled WHERE id = @id", parameters);
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

                SQLiteParameter[] parameters = { new SQLiteParameter("@enabled", newApp.Enabled), new SQLiteParameter("@DisplayName", newApp.DisplayName), new SQLiteParameter("@fullPath", newApp.FullPath), new SQLiteParameter("@image", ImageToByte(newApp.Image)) };
                if (SQL_ExecuteNonQuery("INSERT INTO ApplicationList (enabled,  DisplayName, fullPath, image) values (@enabled, @DisplayName, @fullPath, @image)", parameters) > 0)
                {
                    SQLiteDataReader reader = SQL_ExecuteQuery("SELECT * FROM ApplicationList WHERE DisplayName = @DisplayName", parameters);
                    if (reader != null)
                    {
                        if (reader.VisibleFieldCount > 0)
                        {
                            while (reader.Read())
                            {
                                newApp.Id = (int)reader.GetInt32(reader.GetOrdinal("id"));
                                RegisterApplication(newApp);
                            }
                        }
                    }
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

            //Add data to lists    
            applicationDetectList.Add(App.FullPath);
            thumbGridView.AddThumb(newThumb);
        }

        void EditApplication(Thumb AppThumb)
        {
            EditApplicationSettings editWindow = new EditApplicationSettings(AppThumb.applicationInfo, false);
            
            if(editWindow.ShowDialog() == DialogResult.OK)
            {
                AppThumb.applicationInfo = editWindow.AppInfo;

                SQLiteParameter[] parameters = { new SQLiteParameter("@id", AppThumb.Id), new SQLiteParameter("@enabled", AppThumb.Enabled),  new SQLiteParameter("@DisplayName", AppThumb.DisplayName), new SQLiteParameter("@fullPath", AppThumb.FullPath), new SQLiteParameter("@image", ImageToByte(AppThumb.Image)) };
                int rowsAffected = SQL_ExecuteNonQuery("UPDATE ApplicationList SET enabled = @Enabled, DisplayName = @DisplayName, fullPath = @fullPath, image = @image WHERE id = @id", parameters);
                logger.Info("Edit Application: {0} edited", AppThumb.DisplayName);
            }            
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
        
        public static byte[] ImageToByte(Image img)
        {
            if (img != null)
            {
                using (var stream = new MemoryStream())
                {
                    img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                    return stream.ToArray();
                }
            }
            return null;
        }

        public static Image ByteToImage(byte[] byteImg)
        {
            if (byteImg != null)
            {
                using (var stream = new MemoryStream(byteImg))
                {
                    return Image.FromStream(stream);
                }
            }
            return null;
        }

        #region HookFunctions
        

        private bool InstallHooksAndRegisterWindows()
        {
            bool cbtHookInstalled = false;
            bool shellHookInstalled = false;
            bool result = false;
            HookId installHook;
            //install the CBT hook
            installHook = HookId.wh_cbt;
            try
            {
                cbtHookInstalled = Convert.ToBoolean(InstallHook(hWnd, ref installHook));

                if (cbtHookInstalled)
                {
                    //This is the CBT create window register from the HookDLL 
                    registeredWindows.Add(CreateWindowRegister(HookType.windowCreate, SharedDefines.UWM_HCBT_CREATEWND));
                    logger.Info("CBT Hook installed succesfully");
                }
                else
                {
                    MessageBox.Show("Could not load HookDLL.dll. Application can not function without it.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    logger.Fatal("Could not load HookDLL.dll.");
                }

                //install the Shell hook
                installHook = HookId.wh_shell;
                shellHookInstalled = Convert.ToBoolean(InstallHook(hWnd, ref installHook));
                if (shellHookInstalled)
                {
                    //This is the SHELL create window register from the HookDLL 
                    registeredWindows.Add(CreateWindowRegister(HookType.windowCreate, SharedDefines.UWM_HSHELL_WINDOWCREATED));
                    logger.Info("Shell Hook installed succesfully");
                }
            }
            catch (DllNotFoundException ex)
            {
                MessageBox.Show("Could not load HookDLL.dll. Application can not function without it.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Fatal("Hook DLL not found. {0}", ex.Message);
            }
            finally
            {
                if ((shellHookInstalled || cbtHookInstalled) && !hookMessageCatcher.IsBusy)
                {
                    hookMessageCatcher.RunWorkerAsync();
                    result =  true;
                }
            }

            return result;
        }

        private bool InstallHooks()
        {
            bool cbtHookInstalled = false;
            bool shellHookInstalled = false;
            bool result = false;
            HookId installHook;
            //install the CBT hook
            installHook = HookId.wh_cbt;
            try
            {
                cbtHookInstalled = Convert.ToBoolean(InstallHook(hWnd, ref installHook));
                if (!cbtHookInstalled)
                {
                    MessageBox.Show("Could not load HookDLL.dll. Application can not function without it.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    logger.Fatal("Could not load HookDLL.dll.");
                }

                //install the Shell hook
                installHook = HookId.wh_shell;
                shellHookInstalled = Convert.ToBoolean(InstallHook(hWnd, ref installHook));                

            }
            catch (DllNotFoundException ex)
            {
                MessageBox.Show("Could not load HookDLL.dll. Application can not function without it.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Fatal("Hook DLL not found. {0}", ex.Message);
            }
            finally
            {
                if ((shellHookInstalled || cbtHookInstalled) && !hookMessageCatcher.IsBusy)
                {
                    try
                    {
                        hookMessageCatcher.RunWorkerAsync();
                    }
                    catch (InvalidOperationException ex)
                    {
                        logger.Debug("Background Worker: {0}", ex.Message);
                    }
                    result = true;

                }
            }

            return result;
        }

        RegisteredWindowInfo CreateWindowRegister(HookType type, String messageName)
        {
            RegisteredWindowInfo winRegTemp = new RegisteredWindowInfo();
            CHANGEFILTERSTRUCT filterStatus = new CHANGEFILTERSTRUCT();
            filterStatus.size = (uint)Marshal.SizeOf(filterStatus);
            filterStatus.info = 0;

            //Register the window wiht api call
            winRegTemp.windowRegisterID = RegisterWindowMessage(messageName);
            //Allow messages to be received form lower privileged processes
            ChangeWindowMessageFilterEx(hWnd, winRegTemp.windowRegisterID, ChangeWindowMessageFilterExAction.Allow, ref filterStatus);
            winRegTemp.type = type;

            return winRegTemp;
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

        private void HookMessageCatcher_DoWork(object sender, DoWorkEventArgs e)
        {
            MessageInfo message = new MessageInfo();
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
        
        private void HookMessageCatcher_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            MessageInfo msg = (MessageInfo)e.UserState;
            if (msg == null) return;

            GetWindowHandleInfo(msg.WParam, ref msg);
            logger.Debug("Hook captured: {0}", msg.procInfo.processName);
            if (applicationDetectList.Contains(msg.procInfo.processName))
            {
                ProcessCreatedWindow(msg);
            }
        }

        void ProcessCreatedWindow(MessageInfo msg)
        {
            int index = 0;
            //Program suspension has mutliple methods. I chose the one that seemd to always worked and was simple. Read this post https://stackoverflow.com/questions/11010165/how-to-suspend-resume-a-process-in-windows

            //if the msg id does not match then process
            if ((index = runningApplicationsList.FindIndex(r => r.procID == msg.procInfo.procID)) == -1)
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
                            if (MessageBox.Show("Would you like to continue starting your application?", "Continue", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1,(MessageBoxOptions)0x40000) == DialogResult.No)
                            {                                
                                if (DebugActiveProcessStop(msg.procInfo.procID) == 0)
                                {
                                    logger.Debug("App Un-Paused: {0}", GetLastError().ToString());
                                }

                                try
                                {
                                    msg.procInfo.process = Process.GetProcessById((int)msg.procInfo.procID);
                                    //TODO this needs to be fixed not sure why detected applications id is returning this application process
                                    //msg.procInfo.process.Kill();
                                }
                                catch(Win32Exception ex)
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

        #endregion

        #region SQLite
        bool SQL_OpenConnection(string SQLiteDbName)
        {
            try
            {
                if (m_dbConnection != null && m_dbConnection.State != ConnectionState.Closed)
                    m_dbConnection.Close();

                if (!File.Exists(SQLiteDbName))
                {
                    NVidia_Surround_Assistant.Properties.Settings.Default.SQLiteDbName = SQLiteDbName;
                    //Create db and all relevant tables
                    SQLiteConnection.CreateFile(SQLiteDbName);
                    m_dbConnection = new SQLiteConnection($"Data Source={SQLiteDbName};Version=3;");
                    m_dbConnection.Open();
                    SQL_ExecuteNonQuery("CREATE TABLE ApplicationList (id INTEGER PRIMARY KEY AUTOINCREMENT UNIQUE, enabled BOOLEAN, DisplayName STRING (256), fullPath STRING (260) UNIQUE, image BLOB (20971520))");//20mb file
                }
                else
                {
                    m_dbConnection = new SQLiteConnection($"Data Source=\"{SQLiteDbName}\";Version=3;");
                    m_dbConnection.Open();
                }
            }
            catch (SQLiteException ex)
            {
                logger.Debug("SQLite: SQL Open: {0}", ex.Message);
            }
            catch (System.DllNotFoundException ex)
            {
                MessageBox.Show("Could not open SQL database.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logger.Fatal("Dll not Found: SQL Open: {0}", ex.Message);
            }
            catch(BadImageFormatException ex)
            {
                logger.Fatal("Wrong Dll: SQL Open: {0}", ex.Message);//TODO expand on the rror message
            }

            if (m_dbConnection != null && m_dbConnection.State == ConnectionState.Open)
                return true;
            else
                return false;
        }

        bool SQL_CloseConnection()
        {
            try
            {
                if (m_dbConnection != null && m_dbConnection.State == ConnectionState.Open)
                {
                    m_dbConnection.Close();
                }
            }
            catch (SQLiteException ex)
            {
                logger.Debug("SQL: Close: {0}", ex.Message);
            }

            if (m_dbConnection != null && m_dbConnection.State == ConnectionState.Closed)
                return true;
            else
                return false;
        }

        int SQL_ExecuteNonQuery(string sqlCommand)// need to make sqlliteparmaters list to pass into here. creat an overloaded function
        {
            int result = 0;
            try
            {
                if (m_dbConnection != null && m_dbConnection.State == ConnectionState.Open)
                {
                    command = new SQLiteCommand(sqlCommand, m_dbConnection);
                    result = command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                logger.Debug("SQL: Execute No Query: {0}", ex.Message);
                result = -1;
            }
            return result;
        }

        int SQL_ExecuteNonQuery(string sqlCommand, SQLiteParameter[] parameters)// need to make sqlliteparmaters list to pass into here. creat an overloaded function
        {
            int result = 0;
            try
            {
                if (m_dbConnection != null && m_dbConnection.State == ConnectionState.Open)
                {
                    command = new SQLiteCommand(sqlCommand, m_dbConnection);
                    command.Parameters.AddRange(parameters);
                    result = command.ExecuteNonQuery();
                }
            }
            catch (SQLiteException ex)
            {
                logger.Debug("SQL: Execute No Query: {0}", ex.Message);
                result = -1;
            }
            return result;
        }

        SQLiteDataReader SQL_ExecuteQuery(string sqlCommand)
        {
            SQLiteDataReader reader = null;
            try
            {
                if (m_dbConnection != null && m_dbConnection.State == ConnectionState.Open)
                {
                    command = new SQLiteCommand(sqlCommand, m_dbConnection);
                    reader = command.ExecuteReader();
                }
            }
            catch (SQLiteException ex)
            {
                logger.Debug("SQL: Execute: {0}", ex.Message);
            }
            return reader;
        }

        SQLiteDataReader SQL_ExecuteQuery(string sqlCommand, SQLiteParameter[] parameters)
        {
            SQLiteDataReader reader = null;
            try
            {
                if (m_dbConnection != null && m_dbConnection.State == ConnectionState.Open)
                {
                    command = new SQLiteCommand(sqlCommand, m_dbConnection);
                    command.Parameters.AddRange(parameters);
                    reader = command.ExecuteReader();
                }
            }
            catch (SQLiteException ex)
            {
                logger.Debug("SQL: Execute: {0}", ex.Message);
            }
            return reader;
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
                //TODO minimize scrren and save window positions here as well
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
            surroundManager.SM_ApplySetupFromFile(openFileDialog.FileName);            
        }

        private void toolStripMenuItem_SaveSurroundFile_Click(object sender, EventArgs e)
        {            
            surroundManager.SM_SaveCurrentSetup(saveSurroundFileDialog.FileName);            
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
        #endregion
    }
}
