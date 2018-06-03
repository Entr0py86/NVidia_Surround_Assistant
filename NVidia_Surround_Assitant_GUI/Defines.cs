using System;
using System.Diagnostics;
using DllWrapper;
using System.Runtime.InteropServices;
using System.Drawing;

[Flags]
public enum ProcessAccessFlags : uint
{
    All = 0x001F0FFF,
    Terminate = 0x00000001,
    CreateThread = 0x00000002,
    VirtualMemoryOperation = 0x00000008,
    VirtualMemoryRead = 0x00000010,
    VirtualMemoryWrite = 0x00000020,
    DuplicateHandle = 0x00000040,
    CreateProcess = 0x000000080,
    SetQuota = 0x00000100,
    SetInformation = 0x00000200,
    QueryInformation = 0x00000400,
    QueryLimitedInformation = 0x00001000,
    Synchronize = 0x00100000
}

[StructLayout(LayoutKind.Sequential)]
public struct STARTUPINFO
{
    public Int32 cb;
    public string lpReserved;
    public string lpDesktop;
    public string lpTitle;
    public Int32 dwX;
    public Int32 dwY;
    public Int32 dwXSize;
    public Int32 dwXCountChars;
    public Int32 dwYCountChars;
    public Int32 dwFillAttribute;
    public Int32 dwFlags;
    public Int16 wShowWindow;
    public Int16 cbReserved2;
    public IntPtr lpReserved2;
    public IntPtr hStdInput;
    public IntPtr hStdOutput;
    public IntPtr hStdError;
}

[StructLayout(LayoutKind.Sequential)]
public struct PROCESS_INFORMATION
{
    public IntPtr hProcess;
    public IntPtr hThread;
    public Int32 dwProcessID;
    public Int32 dwThreadID;
}

[StructLayout(LayoutKind.Sequential)]
public struct SECURITY_ATTRIBUTES
{
    public Int32 Length;
    public IntPtr lpSecurityDescriptor;
    public bool bInheritHandle;
}

public enum SECURITY_IMPERSONATION_LEVEL
{
    SecurityAnonymous,
    SecurityIdentification,
    SecurityImpersonation,
    SecurityDelegation
}

public enum TOKEN_TYPE
{
    TokenPrimary = 1,
    TokenImpersonation
}

public enum MessageFilterInfo : uint
{
    None = 0, AlreadyAllowed = 1, AlreadyDisAllowed = 2, AllowedHigher = 3
};

public enum ChangeWindowMessageFilterExAction : uint
{
    Reset = 0, Allow = 1, DisAllow = 2
};

[StructLayout(LayoutKind.Sequential)]
public struct CHANGEFILTERSTRUCT
{
    public uint size;
    public MessageFilterInfo info;
}

namespace NVidia_Surround_Assistant
{
    public enum HookType
    {
        windowCreate,
        windowDestroy,
        windowReplaced
    };

    public enum Settings_AskSwitch
    {
        Always,
        Ask,
        Never
    };

    public struct RegisteredWindowInfo
    {
        public HookId id;
        public uint windowRegisterID;
        public HookType type;
    };

    public struct ProcessInfo
    {
        public Process      process;
        public IntPtr       hWnd;        
        public IntPtr       procID;
        public IntPtr       handleID;
        public String       processName;
    };

    public class MessageInfo
    {
        public RegisteredWindowInfo regWndInfo;
        public ProcessInfo          procInfo;
        public IntPtr               WParam;
        public IntPtr               LParam;        
    };

    public class ApplicationInfo
    {
        public int Id { get; set; }
        public bool Enabled { get; set; }
        public string DisplayName { get; set; }
        public string FullPath { get; set; }
        public Bitmap Image { get; set; }
        public string NormalGrid { get; set; }
        public string SurroundGrid { get; set; }

        public ApplicationInfo()
        {
            Image = null;
        }
    };
};