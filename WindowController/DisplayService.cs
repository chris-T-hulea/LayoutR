using DataModel.Entities;
using DataModel.Geometry;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WindowController.Interfaces;

namespace WindowController
{
	/// <summary>
	/// Service for managing the Displays
	/// </summary>
	class DisplayService : IDisplayService
	{

		#region Private Fields
		
		/// <summary>
		/// Task bar size 
		/// </summary>
		// TODO find it dynamically
		private readonly Rect taskbarDiff = new Rect { Bottom = 40 };

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

		#region External Methods

		/// <summary>
		/// Gets the rectangle representing the frame of a window.
		/// </summary>
		/// <param name="windowHandle">The window handle.</param>
		/// <param name="rectangle">The rectangle.</param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		static extern bool EnumDisplayMonitors(IntPtr hdc, IntPtr lprcClip, EnumMonitorsDelegate lpfnEnum, IntPtr dwData);

		#endregion

		delegate bool EnumMonitorsDelegate(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData);

		#region Public Methods

		/// <inheritdoc/>
		public IEnumerable<Display> GetAllDisplays()
		{
			this.displays.Clear();

			EnumDisplayMonitors((IntPtr)null, (IntPtr)null, AddMonitor, (IntPtr)null);

			return displays;
		}

		#endregion

		#region Private Methods

		private bool AddMonitor(IntPtr hMonitor, IntPtr hdcMonitor, ref Rect lprcMonitor, IntPtr dwData)
		{
			Display display = new Display();

			display.Position = new Rect
			{
				Left = lprcMonitor.Left,
				Top = lprcMonitor.Top,
				Right = lprcMonitor.Right,
				Bottom = lprcMonitor.Bottom - taskbarDiff.Bottom,

			};

			display.IsPrimary = display.Position.Left == 0 && display.Position.Top == 0;

			this.displays.Add(display);

			return true;
		}

		#endregion

	}
}
