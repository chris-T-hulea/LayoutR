using DataModel.Entities;
using DataModel.Geometry;
using System.Collections.Generic;

namespace WindowController.Interfaces
{
	public interface IWindowConrtoller
	{
		IEnumerable<Screen> GetAllWindows();
		(int left, int top, int right, int bot) GetScreenBounds(Screen screen);
		void SetScreenBounds(Screen screen, Rectangle lpRect, int offsetX, int offsetY);
	}
}
