
using System;
using System.Windows;
using Rect = DataModel.Geometry.Rect;
using RectD = DataModel.Geometry.RectD;

namespace LayoutR
{
	public class RectangleVM : ViewModel
	{
		private Rect actualRectangle;
		private RectD configuredRectangle;
		private double factor;
		private RectD offset;
		private Thickness margins;

		public RectangleVM(Rect rectangle, double factor = 1)
		{
			this.SetProperties(rectangle, new RectD(), factor);
		}

		public RectangleVM(Rect rectangle, RectD offset, double factor = 1)
		{
			this.SetProperties(rectangle, offset, factor);
		}
		public RectangleVM(RectD rectangle, double factor = 1)
		{
			this.SetProperties(rectangle, new RectD(), factor);
		}

		public RectangleVM(RectD rectangle, RectD offset, double factor = 1)
		{
			this.SetProperties(rectangle, offset, factor);
		}

		public Rect ActualRectangle { get=>this.actualRectangle; set=>SetProperty(ref this.actualRectangle, value); }
		public RectD ConfiguredRectangle
		{
			get => this.configuredRectangle; 
			set
			{
				SetProperty(ref this.configuredRectangle, value);
				this.SetMargins();
			}
		}
		public RectD Offset { get => this.offset; set => SetProperty(ref this.offset, value); }
		public double Factor { get => this.factor; set => SetProperty(ref this.factor, value); }
		public Thickness Margins { get => this.margins; set => SetProperty(ref this.margins, value); }

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
		public void SetMargins()
		{
			Margins = new Thickness
			{
				Top = this.configuredRectangle.Top,
				Left = this.configuredRectangle.Left,
				Right = 0,
				Bottom = 0,
			};
		}
	}
}
