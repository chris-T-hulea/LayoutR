
using System;
using System.Windows;
using Rect = DataModel.Geometry.Rect;
using RectD = DataModel.Geometry.RectD;

namespace LayoutR
{
	/// <summary>
	/// View model class reprezenting an area on the Application and one on the display.
	/// </summary>
	public class RectangleVM : ViewModel
	{
		#region Private Fields

		private Rect actualRectangle;
		private RectD configuredRectangle;
		private double factor;
		private RectD offset;
		private Thickness margins;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of <see cref="RectangleVM"/>
		/// </summary>
		/// <param name="rectangle">The rectangle representings the area of the display.</param>
		/// <param name="factor">the scale factor.</param>
		public RectangleVM(Rect rectangle, double factor = 1)
		{
			this.SetProperties(rectangle, new RectD(), factor);
		}


		/// <summary>
		/// Initializes a new instance of <see cref="RectangleVM"/>
		/// </summary>
		/// <param name="rectangle">The rectangle representings the are of the display.</param>
		/// <param name="offset">The offset on the aplication.</param>
		/// <param name="factor">the scale factor.</param>
		public RectangleVM(Rect rectangle, RectD offset, double factor = 1)
		{
			this.SetProperties(rectangle, offset, factor);
		}

		/// <summary>
		/// Initializes a new instance of <see cref="RectangleVM"/>
		/// </summary>
		/// <param name="rectangle">The rectangle representings the area of the application.</param>
		/// <param name="factor">the scale factor.</param>
		public RectangleVM(RectD rectangle, double factor = 1)
		{
			this.SetProperties(rectangle, new RectD(), factor);
		}

		/// <summary>
		/// Initializes a new instance of <see cref="RectangleVM"/>
		/// </summary>
		/// <param name="rectangle">The rectangle representings the are of the application.</param>
		/// <param name="offset">The offset on the aplication.</param>
		/// <param name="factor">the scale factor.</param>
		public RectangleVM(RectD rectangle, RectD offset, double factor = 1)
		{
			this.SetProperties(rectangle, offset, factor);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the actual area on the display.
		/// </summary>
		public Rect ActualRectangle { get=>this.actualRectangle; set=>SetProperty(ref this.actualRectangle, value); }
		
		/// <summary>
		/// Gets or sets the configured area in the application.
		/// </summary>
		public RectD ConfiguredRectangle
		{
			get => this.configuredRectangle; 
			set
			{
				SetProperty(ref this.configuredRectangle, value);
				this.SetMargins();
			}
		}

		/// <summary>
		/// Gets or sets the offset on the application.
		/// </summary>
		public RectD Offset { get => this.offset; set => SetProperty(ref this.offset, value); }

		/// <summary>
		/// gets or sets the scaling factor.
		/// </summary>
		public double Factor { get => this.factor; set => SetProperty(ref this.factor, value); }

		/// <summary>
		/// Gets or sets the margins in the application.
		/// </summary>
		public Thickness Margins { get => this.margins; set => SetProperty(ref this.margins, value); }

		#endregion

		#region Public Methods

		/// <summary>
		/// Sets the properties for this class with the provided data.
		/// </summary>
		/// <param name="rectangle">The rectangle representings the are of the display.</param>
		/// <param name="offset">The offset on the aplication.</param>
		/// <param name="factor">the scale factor.</param>
		public void SetProperties(Rect rectangle, RectD offset, double factor = 1)
		{
			this.Factor = factor;
			this.ActualRectangle = rectangle;

			this.Offset = offset;
			this.ConfiguredRectangle = new RectD
			{
				Left = this.ActualRectangle.Left * this.factor - offset.Left,
				Top = this.ActualRectangle.Top * this.factor - offset.Top,
				Right = this.ActualRectangle.Right * this.factor - offset.Right,
				Bottom = this.ActualRectangle.Bottom * this.factor - offset.Bottom,
			};
		}

		/// <summary>
		/// Sets the properties for this class with the provided data.
		/// </summary>
		/// <param name="rectangle">The rectangle representings the are of the application.</param>
		/// <param name="offset">The offset on the aplication.</param>
		/// <param name="factor">the scale factor.</param>
		public void SetProperties(RectD rectangle, RectD offset, double factor = 1)
		{
			this.Factor = factor;
			this.ConfiguredRectangle = rectangle;

			this.Offset = offset;
			this.ActualRectangle = new Rect
			{
				Left = (int)Math.Round((this.ConfiguredRectangle.Left - offset.Left) / this.factor),
				Top = (int)Math.Round((this.ConfiguredRectangle.Top - offset.Top) / this.factor),
				Right = (int)Math.Round((this.ConfiguredRectangle.Right - offset.Left) / this.factor),
				Bottom = (int)Math.Round((this.ConfiguredRectangle.Bottom - offset.Top) / this.factor),
			};
		}

		#endregion

		#region Private Methods

		private void SetMargins()
		{
			Margins = new Thickness
			{
				Top = this.configuredRectangle.Top,
				Left = this.configuredRectangle.Left,
				Right = 0,
				Bottom = 0,
			};
		}

		#endregion
	}
}
