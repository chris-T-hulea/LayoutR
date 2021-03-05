using DataModel.Entities;
using DataModel.Geometry;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Vanara.PInvoke;
using WindowController.Interfaces;
using static Vanara.PInvoke.User32;

namespace WindowController
{
	/// <summary>
	/// Service for managing the Displays
	/// </summary>
	class DisplayService : IDisplayService
	{

		#region Private Fields
		
		private List<Display> displays;

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of <see cref="DisplayService"/>.
		/// </summary>
		public DisplayService()
		{
			this.displays = new List<Display>();

			
		}

		#endregion

		#region Public Methods

		/// <inheritdoc/>
		public IEnumerable<Display> GetAllDisplays()
		{
			this.displays.Clear();

			EnumDisplayMonitors(HDC.NULL, null, this.AddMonitor, IntPtr.Zero) ;

			return displays;
		}

		#endregion

		#region Private Methods

		private bool AddMonitor(IntPtr Arg1, IntPtr Arg2, PRECT Arg3, IntPtr Arg4)
		{
			Display display = new Display();
			MONITORINFOEX monitor = new MONITORINFOEX();

			monitor.cbSize = (uint)Marshal.SizeOf(typeof(MONITORINFOEX));
			GetMonitorInfo(Arg1, ref monitor);

			display.Position = new Rect
			{
				Left = monitor.rcWork.left,
				Top = monitor.rcWork.top,
				Right = monitor.rcWork.right,
				Bottom = monitor.rcWork.bottom,
			};


			display.IsPrimary = display.Position.Left == 0 && display.Position.Top == 0;

			this.displays.Add(display);

			return true;
		}

		#endregion

	}
}
