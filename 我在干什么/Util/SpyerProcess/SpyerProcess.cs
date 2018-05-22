using System;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Time时间记录器.Util;
using DotNet4.Utilities.UtilReg;

namespace Time时间记录器
{
	public static class SpyerProcess
	{
		[DllImport("User32.dll")]
		private static extern IntPtr GetForegroundWindow();     //获取活动窗口句柄  
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		private static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);   //获取线程ID  
		public static Process GetCurrentProcessFocus()
		{
			IntPtr hWnd = GetForegroundWindow();    //获取活动窗口句柄   
			int CalcThreadID = 0;    //线程ID  
			CalcThreadID = GetWindowThreadProcessId(hWnd, out int CalcID);
			return Process.GetProcessById(CalcID);
		}
		public static string GetMillToString(long time)
		{
			time *= 10000;
			return new TimeSpan(time).ToString("d'天'hh'小时'mm'分钟'ss'秒'");
		}
	}
	
}
