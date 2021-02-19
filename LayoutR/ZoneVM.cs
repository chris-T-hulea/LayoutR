using DataModel.Entities;
using DataModel.Geometry;
using Prism.Commands;
using System;
using System.Windows;
using System.Windows.Input;

namespace LayoutR
{
	public class ZoneVM : ViewModel
	{
		private readonly MainWindowViewModel parent;
		private readonly double factor = 0.1;
		private double height;
		private double width;
		private Thickness offset;
		private Screen screen;

		public ZoneVM(MainWindowViewModel parent, double height, double width, double offsetX, double offsetY)
		{
			this.parent = parent;
			// get window controll here
			this.height = height;
			this.width = width;
			this.offset = new Thickness(offsetX, offsetY, 0, 0);
			this.ZoneCommand = new DelegateCommand(OnZoneSelected);
		}

		public ICommand ZoneCommand { get; private set; }

		public double Height { get => height; set => SetProperty(ref this.height, value); }
		public double Width { get => width; set => SetProperty(ref this.width, value); }

		public Thickness Offset { get => this.offset; set => this.SetProperty(ref this.offset, value); }
		public Screen Screen { get => this.screen; set => this.SetProperty(ref this.screen, value); }

		public Rectangle Rectangle 
		{
			get => new Rectangle
			{
				Left = (int)Math.Round(Offset.Left/factor),
				Top = (int)Math.Round(Offset.Top / factor),
				Right = (int)Math.Round(Offset.Left / factor + Width / factor),
				Bottom = (int)Math.Round(Offset.Top / factor + Height / factor),
			};
		}

		private void OnZoneSelected()
		{
			parent.SelectZone(this);
		}
	}
}
