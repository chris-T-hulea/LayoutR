using DataModel.Entities;
using DataModel.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace WindowController
{
	public class ProcessService : Interfaces.IProcessService
	{
		private readonly IDictionary<string, Rect> knownAdjustments = new Dictionary<string, Rect>
		{
			{ "chrome", new Rect{Top = 0, Bottom = 7, Left = -7, Right = 7 } },
			{ "Rambox", new Rect{Top = 0, Bottom = 7, Left = -7, Right = 7 } }
		};

		public ProcessService()
		{
		}

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetWindowRect(HandleRef hWnd, out Rect lpRect);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

		[DllImport("user32.dll")]
		public static extern IntPtr FindWindow(string className, string windowTitle);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);

		[DllImport("user32.dll")]
		private static extern int SetForegroundWindow(IntPtr hwnd);



		public IEnumerable<Screen> GetAllProcesses()
		{
			IEnumerable<Process> processes = Process.GetProcesses();
			return processes.Select(process => new Screen
			{
				Id = process.Id,
				Name = process.ProcessName,
				Title = process.MainWindowTitle,
				Pointer = process.MainWindowHandle,
			});
		}

		public (int left, int top, int right, int bot) GetScreenBounds(Screen screen)
		{
			GetWindowRect(new HandleRef(screen, screen.Pointer), out Rect lpRect);

			return (lpRect.Left, lpRect.Top, lpRect.Right, lpRect.Bottom);
		}

		public void SetScreenBounds(Screen screen, Rect lpRect)
		{
			var adjustment = new Rect();
			if (knownAdjustments.ContainsKey(screen.Name))
			{
				adjustment = knownAdjustments[screen.Name];
			}
			lpRect += adjustment;

			ShowWindow(screen.Pointer, ShowWindowEnum.ShowNormal);

			//SetForegroundWindow(screen.Pointer);

			MoveWindow(screen.Pointer, lpRect.Left, lpRect.Top, lpRect.Width, lpRect.Height, true);
		}

		private enum ShowWindowEnum
		{
			Hide = 0,
			ShowNormal = 1, ShowMinimized = 2, ShowMaximized = 3,
			Maximize = 3, ShowNormalNoActivate = 4, Show = 5,
			Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8,
			Restore = 9, ShowDefault = 10, ForceMinimized = 11
		};
	}
}
