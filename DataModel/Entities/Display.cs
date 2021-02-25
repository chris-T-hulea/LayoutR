using DataModel.Geometry;

namespace DataModel.Entities
{
	public class Display : Enitity
	{
		public bool IsPrimary { get; set; }
		public Rect Position { get; set; }
	}
}
