using DataModel.Entities;
using DataModel.Geometry;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WindowController.Interfaces;

namespace WindowController
{
	class DisplayService : IDisplayService
	{
		private List<Display> displays;

		private readonly Rect taskbarDiff = new Rect { Bottom = 40 };

		public DisplayService()
		{
			this.displays = new List<Display>();
		}

		/// <summary>
		/// Gets the rectangle representing the frame of a window.
		/// </summary>
		/// <param name="windowHandle">The window handle.</param>
		/// <param name="rectangle">The rectangle.</param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

		delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);


		private bool func(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData)
		{
			Display display;
			Display di = new Display();

			di.Position = new Rect
			{
				Left = lprcMonitor.Left,
				Top = lprcMonitor.Top,
				Right = lprcMonitor.Right,
				Bottom = lprcMonitor.Bottom - taskbarDiff.Bottom,

			};

			di.IsPrimary = di.Position.Left == 0 && di.Position.Top == 0;
			this.displays.Add(di);
			return true;
		}

		public IEnumerable<Display> GetAllDisplays()
		{
			Rect al = new Rect();

			EnumDisplayMonitors((IntPtr)null, (IntPtr)null, func, (IntPtr)null);

			return displays;
		}
	}
}
