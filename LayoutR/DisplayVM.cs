using DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UIUtils;

namespace LayoutR
{
	public class DisplayVM : ViewModel
	{
		private readonly Display display;
		private readonly double factor = 0.1;
		private double height;
		private double width;
		private Thickness offset;
		private int hDivisions = 2;
		private int vDivisions = 1;

		public DisplayVM(Display display)
		{
			this.display = display;

			this.height = display.Height;
			this.width = display.Width;
			offset = new Thickness(display.OffsetX * factor, display.OffsetY * factor, 0, 0);

			this.Zones = new BulkObservableCollection<ZoneVM>();
			UpdateZones();
		}

		public double Height { get => (int)Math.Round(height * factor); set => SetProperty(ref this.height, (int)Math.Round(value / factor)); }
		public double Width { get => (int)Math.Round(width * factor); set => SetProperty(ref this.width, (int)Math.Round(value / factor)); }


		public int HDivisions
		{
			get => hDivisions;
			set
			{
				SetProperty(ref this.hDivisions, value);
				this.UpdateZones();
			}
		}

		public int VDivisions
		{
			get => vDivisions;
			set
			{
				SetProperty(ref this.vDivisions, value);
				this.UpdateZones();
			}
		}

		private void UpdateZones()
		{
			double zoneHeight = this.Height / this.VDivisions;
			double zoneWidth = this.Width / this.HDivisions;
			double zoneOffsetX;
			double zoneOffsetY;
			using (this.Zones.SuppressiNotifications())
			{
				this.Zones.Clear();
				for (int i = 0; i < vDivisions; i++)
				{
					for (int j = 0; j < hDivisions; j++)
					{
						zoneOffsetX = (j * zoneWidth);
						zoneOffsetY = (i * zoneHeight);
						this.Zones.Add(new ZoneVM(zoneHeight, zoneWidth, zoneOffsetX, zoneOffsetY));
					}
				}

			}

		}

		public Thickness Offset { get => this.offset; set => this.SetProperty(ref this.offset, value); }
		public BulkObservableCollection<ZoneVM> Zones { get; private set; }
	}
}
