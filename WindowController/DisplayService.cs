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
		/// <summary>
		/// Gets the rectangle representing the frame of a window.
		/// </summary>
		/// <param name="windowHandle">The window handle.</param>
		/// <param name="rectangle">The rectangle.</param>
		/// <returns></returns>
		[DllImport("user32.dll")]
		public static extern bool GetWindowRect(IntPtr windowHandle, ref Rectangle rectangle);

		public IEnumerable<Display> GetAllDisplays()
		{
			return new List<Display>()
			{
				new Display
				{
					Id = 0,
					Width = 2560,
					Height = 1440,
					OffsetY = 0,
					OffsetX = 0,
					IsPrimary = true,
				},
				new Display
				{
					Id = 0,
					Width = 1440,
					Height = 2560,
					OffsetX = -1440,
					OffsetY = -827,
					IsPrimary = true,
				}
			};
		}
	}
}
