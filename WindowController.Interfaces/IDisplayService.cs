using DataModel.Entities;
using System.Collections.Generic;

namespace WindowController.Interfaces
{
	public interface IDisplayService
	{
		IEnumerable<Display> GetAllDisplays();
	}
}
