using DataModel.Entities;
using DataModel.Geometry;
using System.Collections.Generic;

namespace WindowController.Interfaces
{
	/// <summary>
	/// Interface for window controller.
	/// </summary>
	public interface IWindowConrtoller
	{
		/// <summary>
		/// Gets all the currently available windows.
		/// </summary>
		/// <returns>List of screens.</returns>
		IEnumerable<Screen> GetAllWindows();


		/// <summary>
		/// Gets the current bounderies of a specified screen.
		/// </summary>
		/// <param name="screen">The specified screen.</param>
		/// <returns>The boundries of the screen.</returns>
		Rect GetScreenBounds(Screen screen);


		/// <summary>
		/// Sets the boundries of a specified screen
		/// </summary>
		/// <param name="screen">The screen to be updated.</param>
		/// <param name="boundries">The boundries to set.</param>
		void SetScreenBounds(Screen screen, Rect lpRect, int offsetX, int offsetY);
	}
}
