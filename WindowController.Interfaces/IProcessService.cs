using DataModel.Entities;
using DataModel.Geometry;
using System.Collections.Generic;

namespace WindowController.Interfaces
{
	public interface IProcessService
	{
		/// <summary>
		/// Gets all the Currently running processes
		/// </summary>
		/// <returns></returns>
		IEnumerable<Screen> GetAllProcesses();

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
		void SetScreenBounds(Screen screen, Rect boundries);
	}
}
