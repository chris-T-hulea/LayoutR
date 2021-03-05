using DataModel.Entities;
using DataModel.Geometry;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using Vanara.PInvoke;
using static Vanara.PInvoke.User32;

namespace WindowController
{
	/// <summary>
	/// Service class for managing processes
	/// </summary>
	public class ProcessService : Interfaces.IProcessService
	{
		#region Private Fields

		private List<App> appCache;

		private Timer timer;

		private object cacheLock;

		#endregion

		#region Constructors

		public ProcessService()
		{
			this.appCache = new List<App>();
			this.cacheLock = new object();
			this.timer = new Timer();
			this.timer.Interval = 500;
			this.timer.Elapsed += this.OntimerElapsed;
			this.timer.AutoReset = true;
			this.timer.Start();
			this.OntimerElapsed(null, null);
		}

		private void OntimerElapsed(object sender, ElapsedEventArgs e)
		{
			IEnumerable<Process> processes = Process.GetProcesses();
			var apps = processes.Select(process => new App
			{
				Id = process.Id,
				Name = process.ProcessName,
				Title = process.MainWindowTitle,
				Pointer = process.MainWindowHandle,
			});

			lock (this.cacheLock)
			{
				this.appCache = apps.ToList();
			}
		}

		#endregion

		#region Public Methods


		/// <inheritdoc/>
		public IEnumerable<App> GetAllApplication()
		{
			lock (this.cacheLock)
			{
				return this.appCache;
			}
		}

		/// <inheritdoc/>
		public Rect GetApplicationBounds(App application)
		{
			GetWindowRect(application.Pointer, out RECT resultedRect);

			return new Rect
			{
				Bottom = resultedRect.bottom,
				Left = resultedRect.left,
				Right = resultedRect.right,
				Top = resultedRect.top,
			};
		}

		/// <inheritdoc/>
		public void SetApplicationBounds(App application, Rect boundries)
		{
			WINDOWINFO info = new WINDOWINFO();

			GetWindowInfo(application.Pointer, ref info);
			Rect adjustments = new Rect
			{
				Top = 0,
				Left = -(int)info.cxWindowBorders,
				Right = (int)info.cxWindowBorders,
				Bottom = (int)info.cyWindowBorders,
			};

			boundries += adjustments;

			//SetForegroundWindow(application.Pointer);

			ShowWindow(application.Pointer, ShowWindowCommand.SW_RESTORE);
			MoveWindow(application.Pointer, boundries.Left, boundries.Top, boundries.Width, boundries.Height, true);
		}

		#endregion

		#region Private Methods

		#endregion

	}
}
