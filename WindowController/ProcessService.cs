using DataModel.Entities;
using DataModel.Geometry;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace WindowController
{
	class ProcessService : Interfaces.IProcessService
	{

		public ProcessService()
		{
		}

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetWindowRect(HandleRef hWnd, out Rectangle lpRect);

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
			GetWindowRect(new HandleRef(screen, screen.Pointer), out Rectangle lpRect);

			return (lpRect.Left, lpRect.Top, lpRect.Right, lpRect.Bottom);
		}
	}
}
