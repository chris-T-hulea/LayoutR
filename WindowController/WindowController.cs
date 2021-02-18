using DataModel.Entities;
using System.Collections.Generic;
using System.Linq;
using WindowController.Interfaces;

namespace WindowController
{
	class WindowController : IWindowConrtoller
	{
		private readonly IProcessService processService;

		public WindowController(IProcessService processService)
		{
			this.processService = processService;
		}

		public IEnumerable<Screen> GetAllWindows()
		{
			var screens = this.processService.GetAllProcesses();
			return screens.Where(screen => screen.Title?.Length > 0);
		}

		public (int left, int top, int right, int bot) GetScreenBounds(Screen screen)
		{
			return this.processService.GetScreenBounds(screen);
		}
	}
}
