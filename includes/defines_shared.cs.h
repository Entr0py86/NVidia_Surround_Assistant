#pragma once

///To use in C# change Build Action of file to Compile
#if WIN32

using namespace std;
// C++ code--removes public keyword for C++
#define public


#else

// C# code
using System;
namespace DllWrapper
{
	public class SharedDefines
	{
		//... C# Specific class code

#endif

		//These items are members of the shared C# class 
		//and global in C++	

		/*CBT Hook Message Window Registers*/
		public const string UWM_HCBT_ACTIVATE = "UWM_HCBT_ACTIVATE_MSG-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HCBT_CLICKSKIPPED = "UWM_HCBT_CLICKSKIPPED_MSG-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HCBT_CREATEWND = "UWM_HCBT_CREATEWND_MSG-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HCBT_DESTROYWND = "UWM_HCBT_DESTROYWND_MSG-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HCBT_KEYSKIPPED = "UWM_HCBT_KEYSKIPPED_MSG-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HCBT_MINMAX = "UWM_HCBT_MINMAX_MSG-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HCBT_MOVESIZE = "UWM_HCBT_MOVESIZE_MSG-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HCBT_QS = "UWM_HCBT_QS_MSG-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HCBT_SETFOCUS = "UWM_HCBT_SETFOCUS_MSG-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HCBT_SYSCOMMAND = "UWM_HCBT_SYSCOMMAND_MSG-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";

		/*Shell Hook Message Window Registers*/
		public const string UWM_HSHELL_WINDOWCREATED = "UWM_HSHELL_WINDOWCREATED-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HSHELL_WINDOWDESTROYED = "UWM_HSHELL_WINDOWDESTROYED-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HSHELL_ACTIVATESHELLWINDOW = "UWM_HSHELL_ACTIVATESHELLWINDOW-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";

															   
	//#if(WINVER == 0x0400)								   
		public const string UWM_HSHELL_WINDOWACTIVATED = "UWM_HSHELL_WINDOWACTIVATED-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HSHELL_GETMINRECT = "UWM_HSHELL_GETMINRECT-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HSHELL_REDRAW = "UWM_HSHELL_REDRAW-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HSHELL_TASKMAN = "UWM_HSHELL_TASKMAN-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HSHELL_LANGUAGE = "UWM_HSHELL_LANGUAGE-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HSHELL_SYSMENU = "UWM_HSHELL_SYSMENU-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HSHELL_ENDTASK = "UWM_HSHELL_ENDTASK-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
	//#endif /* WINVER >= 0x0400 */
	//#if(_WIN32_WINNT >= 0x0500)
		public const string UWM_HSHELL_ACCESSIBILITYSTATE = "UWM_HSHELL_ACCESSIBILITYSTATE-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HSHELL_APPCOMMAND = "UWM_HSHELL_APPCOMMAND-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
	//#endif /* _WIN32_WINNT >= 0x0500 */

	//#if(_WIN32_WINNT >= 0x0501)
		public const string UWM_HSHELL_WINDOWREPLACED = "UWM_HSHELL_WINDOWREPLACED-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
		public const string UWM_HSHELL_WINDOWREPLACING = "UWM_HSHELL_WINDOWREPLACING-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
	//#endif /* _WIN32_WINNT >= 0x0501 */

	//#if(_WIN32_WINNT >= 0x0602)
		public const string UWM_HSHELL_MONITORCHANGED = "UWM_HSHELL_WINDOWREPLACING-{0485D79B-7931-4F2D-B1B3-41BB19C0D883}";
	//#if (NTDDI_VERSION >= NTDDI_WIN10_RS3)
	//#endif // NTDDI_VERSION >= NTDDI_WIN10_RS3

	//#endif /* _WIN32_WINNT >= 0x0602 */
		
		public const int UWM_WH_MINHOOK = 0;
		public const int UWM_WH_MAXHOOK = 16;

#if !WIN32
		// C# code
	};   //end of class SharedEnums

#endif

	public enum HookId : int
	{
		wh_none				= -1,
		wh_msgfilter		= 0,
		wh_journalrecord	= 1,
		wh_journalplayback  = 2,
		wh_keyboard         = 3,
		wh_getmessage       = 4,
		wh_callwndproc		= 5,
		wh_cbt				= 6,
		wh_sysmsgfilter		= 7,
		wh_hardware			= 8,
		wh_mouse			= 9,
		wh_debug			= 10,
		wh_shell			= 11,
		wh_foregroundidle	= 12,
		wh_callwndprocret	= 13,
		wh_keyboard_ll		= 14,
		wh_mouse_ll			= 15,
	};

	//public struct 

#if WIN32	
	//C++ code

#undef public

#else

	//C# code
}    // end DllWrapper

#endif