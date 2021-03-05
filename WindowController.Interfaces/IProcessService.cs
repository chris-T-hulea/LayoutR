using DataModel.Entities;
using DataModel.Geometry;
using System.Collections.Generic;

namespace WindowController.Interfaces
{
	public interface IProcessService
	{
		/// <summary>
		/// Gets all the Currently running applications.
		/// </summary>
		/// <returns></returns>
		IEnumerable<App> GetAllApplication();

		/// <summary>
		/// Gets the current bounderies of a specified application.
		/// </summary>
		/// <param name="app">The specified application.</param>
		/// <returns>The boundries of the application.</returns>
		Rect GetApplicationBounds(App application);

		/// <summary>
		/// Sets the boundries of a specified application
		/// </summary>
		/// <param name="application">The application to be updated.</param>
		/// <param name="boundries">The boundries to set.</param>
		void SetApplicationBounds(App application, Rect boundries);
	}
}
