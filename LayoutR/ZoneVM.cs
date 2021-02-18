using DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LayoutR
{
	public class ZoneVM : ViewModel
	{
		private double height;
		private double width;
		private Thickness offset;
		private Screen screen;

		public ZoneVM(double height, double width, double offsetX, double offsetY)
		{
			// get window controll here
			this.height = height;
			this.width = width;
			offset = new Thickness(offsetX, offsetY, 0, 0);
		}

		public double Height { get => height; set => SetProperty(ref this.height, value); }
		public double Width { get => width; set => SetProperty(ref this.width, value); }

		public Thickness Offset { get => this.offset; set => this.SetProperty(ref this.offset, value); }
		public Screen Screen { get => this.screen; set => this.SetProperty(ref this.screen, value); }
	}
}
