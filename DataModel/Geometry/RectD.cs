namespace DataModel.Geometry
{
	public struct RectD
	{
		public double Left { get; set; }     
		public double Top { get; set; }      
		public double Right { get; set; }    
		public double Bottom { get; set; }
		public double Height { get => this.Bottom - this.Top; }
		public double Width { get => this.Right - Left; }

		public override string ToString()
		{
			return $"({Left},{Top}),({Height},{Width})";
		}
	}
}
