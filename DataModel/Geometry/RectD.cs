namespace DataModel.Geometry
{
	public struct RectD
	{

		#region Properties

		/// <summary>
		/// Gets or sets the left position.
		/// </summary>
		public double Left { get; set; }

		/// <summary>
		/// Gets or sets the top position.
		/// </summary>   
		public double Top { get; set; }

		/// <summary>
		/// Gets or sets the right position.
		/// </summary>    
		public double Right { get; set; }

		/// <summary>
		/// Gets or sets the bottom position.
		/// </summary>  
		public double Bottom { get; set; }

		/// <summary>
		/// Gets the height of the rectangle.
		/// </summary> 
		public double Height { get => this.Bottom - this.Top; }

		/// <summary>
		/// Gets the width of the rectangle.
		/// </summary>
		public double Width { get => this.Right - Left; }

		#endregion

		#region Public Methods

		/// <inheritdoc/>
		public override string ToString()
		{
			return $"({Left},{Top}),({Width},{Height})";
		}

		#endregion

	}
}
