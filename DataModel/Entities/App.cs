using System;

namespace DataModel.Entities
{
	/// <summary>
	/// The entity class for representing a manageable window.
	/// </summary>
	public class App : Enitity
	{
		/// <summary>
		/// Gets or sets the process name of the app.
		/// </summary>
		public string Name { get; set; }
		
		/// <summary>
		/// Gets or sets the title of the window.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// The pointer to the process.
		/// </summary>
		public IntPtr Pointer { get; set; }
	}
}
