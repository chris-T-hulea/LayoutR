using DataModel.Entities;
using System.Collections.Generic;

namespace WindowController.Interfaces
{
	public interface IProcessService
	{
		IEnumerable<Screen> GetAllProcesses();
		(int left, int top, int right, int bot) GetScreenBounds(Screen screen);
	}
}
