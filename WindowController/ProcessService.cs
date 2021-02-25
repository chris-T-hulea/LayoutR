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

		#region Private Fields

		private readonly IDictionary<string, Rect> knownAdjustments = new Dictionary<string, Rect>
		{
			{ "chrome", new Rect{Top = 0, Bottom = 7, Left = -7, Right = 7 } },
			{ "Rambox", new Rect{Top = 0, Bottom = 7, Left = -7, Right = 7 } }
		};

		#endregion

		#region Public Methods


		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public Rect GetScreenBounds(Screen screen)
		{
			GetWindowRect(new HandleRef(screen, screen.Pointer), out Rect resultedRect);

			return resultedRect;
		}

		/// <inheritdoc/>
		public void SetScreenBounds(Screen screen, Rect boundries)
		{
			var adjustment = new Rect();
			if (knownAdjustments.ContainsKey(screen.Name))
			{
				adjustment = knownAdjustments[screen.Name];
			}
			boundries += adjustment;

			ShowWindow(screen.Pointer, ShowWindowEnum.ShowNormal);

			//SetForegroundWindow(screen.Pointer);

			MoveWindow(screen.Pointer, boundries.Left, boundries.Top, boundries.Width, boundries.Height, true);
		}

		#endregion

		#region External Methods

		/// <summary>
		/// Gets the window frame position.
		/// </summary>
		/// <param name="hWnd">The window pointer.</param>
		/// <param name="lpRect">The window position.</param>
		/// <returns><see cref="true"/> id sceeded</returns>
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool GetWindowRect(HandleRef hWnd, out Rect lpRect);

		/// <summary>
		/// Moves the window to a specified locaion.
		/// </summary>
		/// <param name="hWnd">The pointer tot the window.</param>
		/// <param name="X">The position of the left of the frame.</param>
		/// <param name="Y">The position of the top of the frame.</param>
		/// <param name="nWidth">The width of the frame.</param>
		/// <param name="nHeight">The heinght of the frame.</param>
		/// <param name="bRepaint">Value indicatinf if window should be repainted.</param>
		/// <returns><see cref="true"/> id sceeded</returns>

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

		/// <summary>
		/// Finds a specified Window
		/// </summary>
		/// <param name="className">The name cless.</param>
		/// <param name="windowTitle">Th title of the window.</param>
		/// <returns>Pointer to the window.</returns>
		[DllImport("user32.dll")]
		private static extern IntPtr FindWindow(string className, string windowTitle);

		/// <summary>
		/// Updates window state (Maximized/Minimized/Normal)
		/// </summary>
		/// <param name="hWnd">The pointer to the window.</param>
		/// <param name="flags">The flag indicating the desired state.</param>
		/// <returns><see cref="true"/> id sceeded</returns>
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool ShowWindow(IntPtr hWnd, ShowWindowEnum flags);

		/// <summary>
		/// Bring the specified window to foreground.
		/// </summary>
		/// <param name="hwnd">The pointer to the window</param>
		/// <returns><see cref="true"/> id sceeded</returns>
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool SetForegroundWindow(IntPtr hwnd);

		#endregion

		#region Private Methods

		#endregion

		#region Helpers

		private enum ShowWindowEnum
		{
			Hide = 0,
			ShowNormal = 1, ShowMinimized = 2, ShowMaximized = 3,
			Maximize = 3, ShowNormalNoActivate = 4, Show = 5,
			Minimize = 6, ShowMinNoActivate = 7, ShowNoActivate = 8,
			Restore = 9, ShowDefault = 10, ForceMinimized = 11
		};

		#endregion

	}
}
