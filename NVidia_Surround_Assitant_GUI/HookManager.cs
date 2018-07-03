#define CLR

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using DllWrapper;
using NLog;

namespace NVidia_Surround_Assistant
{
    public class HookManager
    {

        #region DLL_Imports        
        #region User32_dll
        //User32.dll  PInvoke Calls
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint RegisterWindowMessage(string lpString);        

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool ChangeWindowMessageFilterEx(IntPtr hWnd, uint msg, ChangeWindowMessageFilterExAction action, ref CHANGEFILTERSTRUCT changeInfo);

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

        IntPtr hWnd;

        private AutoResetEvent _newMsgEvent = new AutoResetEvent(false);
        
        List<RegisteredWindowInfo> registeredWindows;
        

        public HookManager(IntPtr hWnd, ref List<RegisteredWindowInfo> registeredWindowsList)
        {
            registeredWindows = registeredWindowsList;            

            this.hWnd = hWnd;
        }        

        public bool InstallHooksAndRegisterWindows()
        {
            bool hookInstalled = false;
            bool result = false;
            HookId installHook;
            //install the CBT hook
            installHook = HookId.wh_cbt;
            try
            {
                hookInstalled = Convert.ToBoolean(InstallHook(hWnd, ref installHook));

                if (hookInstalled)
                {
                    //This is the CBT create window register from the HookDLL 
                    registeredWindows.Add(CreateWindowRegister(HookType.windowCreate, SharedDefines.UWM_HCBT_CREATEWND));
                    MainForm.logger.Info("CBT Hook installed successfully");
                }
                else
                {
                    MessageBox.Show("Could not load HookDLL.dll. Application can not function without it.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    MainForm.logger.Fatal("Could not load HookDLL.dll.");
                }

                //install the Shell hook
                installHook = HookId.wh_shell;
                hookInstalled &= Convert.ToBoolean(InstallHook(hWnd, ref installHook));
                if (hookInstalled)
                {
                    //This is the SHELL create window register from the HookDLL 
                    registeredWindows.Add(CreateWindowRegister(HookType.windowCreate, SharedDefines.UWM_HSHELL_WINDOWCREATED));                    

                    MainForm.logger.Info("Shell Hook installed successfully");
                }
            }
            catch (DllNotFoundException ex)
            {
                MessageBox.Show("Could not load HookDLL.dll. Application can not function without it.", "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                MainForm.logger.Fatal("Hook DLL not found. {0}", ex.Message);
            }
            finally
            {
                if (hookInstalled)
                { 
                    result = true;
                }
            }

            return result;
        }

        public int UninstallHooks()
        {
            return UnInstallAllHooks();
        }

        RegisteredWindowInfo CreateWindowRegister(HookType type, String messageName)
        {
            RegisteredWindowInfo winRegTemp = new RegisteredWindowInfo();
            CHANGEFILTERSTRUCT filterStatus = new CHANGEFILTERSTRUCT();
            filterStatus.size = (uint)Marshal.SizeOf(filterStatus);
            filterStatus.info = 0;

            //Register the window with api call
            winRegTemp.windowRegisterID = RegisterWindowMessage(messageName);
            //Allow messages to be received form lower privileged processes
            ChangeWindowMessageFilterEx(hWnd, winRegTemp.windowRegisterID, ChangeWindowMessageFilterExAction.Allow, ref filterStatus);
            winRegTemp.type = type;

            return winRegTemp;
        }        
    }
}
