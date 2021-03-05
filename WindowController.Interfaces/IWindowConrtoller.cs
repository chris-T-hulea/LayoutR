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
		/// Gets all the currently available application.
		/// </summary>
		/// <returns>List of application.</returns>
		IEnumerable<App> GetAllApplication();


		/// <summary>
		/// Gets the current bounderies of a specified application.
		/// </summary>
		/// <param name="application">The specified application.</param>
		/// <returns>The boundries of the application.</returns>
		Rect GetApplicationBounds(App application);


		/// <summary>
		/// Sets the boundries of a specified application
		/// </summary>
		/// <param name="application">The application to be updated.</param>
		/// <param name="boundries">The boundries to set.</param>
		void SetApplicationBounds(App application, Rect lpRect, int offsetX, int offsetY);
	}
}
