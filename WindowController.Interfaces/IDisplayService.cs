using DataModel.Entities;
using System.Collections.Generic;

namespace WindowController.Interfaces
{
	/// <summary>
	/// Interface for servie managing displays.
	/// </summary>
	public interface IDisplayService
	{
		/// <summary>
		/// Gets all the currently visible displays from Windows.
		/// </summary>
		/// <returns>All the visible displays.</returns>
		IEnumerable<Display> GetAllDisplays();
	}
}
