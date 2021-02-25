using DataModel.Entities;
using DataModel.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using WindowController.Interfaces;

namespace WindowController
{
	/// <summary>
	/// Thw window controller class.
	/// </summary>
	class WindowController : IWindowConrtoller
	{

		#region Private Fields

		private readonly IProcessService processService;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of class <see cref="WindowController"/>
		/// </summary>
		/// <param name="processService">The process service.</param>
		public WindowController(IProcessService processService)
		{
			this.processService = processService;
		}

		#endregion

		#region Public Methods

		/// <inheritdoc/>
		public IEnumerable<Screen> GetAllWindows()
		{
			var screens = this.processService.GetAllProcesses();
			return screens.Where(s => s.Pointer != IntPtr.Zero);
		}

		/// <inheritdoc/>
		public Rect GetScreenBounds(Screen screen)
		{
			return this.processService.GetScreenBounds(screen);
		}

		/// <inheritdoc/>
		public void SetScreenBounds(Screen screen, Rect lpRect, int offsetX, int offsetY)
		{
			lpRect = new Rect
			{
				Left = lpRect.Left + offsetX,
				Top = lpRect.Top + offsetY,
				Right = lpRect.Right + offsetX,
				Bottom = lpRect.Bottom + offsetY,
			};
			this.processService.SetScreenBounds(screen, lpRect);
		}

		#endregion

	}
}
