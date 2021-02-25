using System.Runtime.InteropServices;

namespace DataModel.Geometry
{
	/// <summary>
	/// Rectangle class used to describe an area on the display.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	public struct Rect
	{

		#region Properties

		/// <summary>
		/// Gets or sets the left position.
		/// </summary>
		public int Left { get; set; }

		/// <summary>
		/// Gets or sets the top position.
		/// </summary>   
		public int Top { get; set; }

		/// <summary>
		/// Gets or sets the right position.
		/// </summary>    
		public int Right { get; set; }

		/// <summary>
		/// Gets or sets the bottom position.
		/// </summary>  
		public int Bottom { get; set; }

		/// <summary>
		/// Gets the height of the rectangle.
		/// </summary> 
		public int Height	 { get => this.Bottom - this.Top; }

		/// <summary>
		/// Gets the width of the rectangle.
		/// </summary>
		public int Width { get => this.Right - Left; }

		#endregion
		
		#region Public Methods

		/// <summary>
		/// Adds the values of 2 rectangles.
		/// </summary>
		/// <param name="a">First rectangle.</param>
		/// <param name="b">Second rectangle.</param>
		/// <returns>The resulted rectangle.</returns>
		public static Rect operator +(Rect a, Rect b) => new Rect
		{
			Left = a.Left + b.Left,
			Top = a.Top + b.Top,
			Right = a.Right + b.Right,
			Bottom = a.Bottom + b.Bottom,
		};

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"({Left},{Top}),({Width},{Height})";
		}

		#endregion

	}
}
