using System.Runtime.InteropServices;

namespace DataModel.Geometry
{
	[StructLayout(LayoutKind.Sequential)]
	public struct Rect
	{
		public int Left { get; set; }    
		public int Top { get; set; }     
		public int Right { get; set; }   
		public int Bottom { get; set; }  
		public int Height	 { get => this.Bottom - this.Top; }  
		public int Width { get => this.Right - Left; }

		public static Rect operator +(Rect a, Rect b) => new Rect
		{
			Left = a.Left + b.Left,
			Top = a.Top + b.Top,
			Right = a.Right + b.Right,
			Bottom = a.Bottom + b.Bottom,
		};

		public override string ToString()
		{
			return $"({Left},{Top}),({Height},{Width})";
		}
	}
}
