namespace DataModel.Entities
{
	public class Display : Enitity
	{
		public bool IsPrimary { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public int OffsetX { get; set; }
		public int OffsetY { get; set; }
	}
}
