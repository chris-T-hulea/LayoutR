using DataModel.Geometry;

namespace DataModel.Entities
{
	/// <summary>
	/// Entity clas representing a monitor display.
	/// </summary>
	public class Display : Enitity
	{
		/// <summary>
		/// Gets or set a value indicating whether or not the scren is the primary one.
		/// </summary>
		public bool IsPrimary { get; set; }

		/// <summary>
		/// Gets or sets the bounds of the display.
		/// </summary>
		public Rect Position { get; set; }
	}
}
