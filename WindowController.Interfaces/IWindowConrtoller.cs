using DataModel.Entities;
using System.Collections.Generic;

namespace WindowController.Interfaces
{
	public interface IWindowConrtoller
	{
		IEnumerable<Screen> GetAllWindows();
		(int left, int top, int right, int bot) GetScreenBounds(Screen screen);
	}
}
