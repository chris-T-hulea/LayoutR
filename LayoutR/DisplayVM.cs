using DataModel.Entities;
using DataModel.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

using Thickness = System.Windows.Thickness;

namespace LayoutR
{
	public class DisplayVM : ViewModel
	{
		private readonly MainWindowViewModel mainWindow;
		private readonly Display display;
		private readonly double factor = 0.1;
		private readonly int index;
		private const int headerHeight = 10;
		private double height;
		private double width;
		private Thickness offset;


		private int hDivisions = 1;
		private int vDivisions = 3;
		private ZoneVM[,] zones;
		private int[] rows;
		private int[] columns;
		private double[] rowSizes;
		private double[] columnSizes;
		private ZoneVM selectedZone;
		private RectangleVM rectangleVm;
		private double defaultHeight;

		public DisplayVM(MainWindowViewModel mainWindow, Display display, int index)
		{
			this.mainWindow = mainWindow;
			this.display = display;
			this.index = index;

			this.RectangleVm = new RectangleVM(
				display.Position,
				new RectD
				{
					Top = headerHeight + 4,
					Left = -index * headerHeight,       // assuming horizontal alignament
					Right = -(index + 1) * headerHeight

				},
				this.factor);

			this.height = RectangleVm.ConfiguredRectangle.Height;
			this.width = RectangleVm.ConfiguredRectangle.Width;

			this.PopulateZones();

			this.UpdateZones();
		}

		public double DefaultHeight { get => this.defaultHeight; set => SetProperty(ref this.defaultHeight, value); }
		public RectangleVM RectangleVm { get => this.rectangleVm; set => SetProperty(ref this.rectangleVm, value); }
		public ZoneVM[,] Zones { get => this.zones; set => SetProperty(ref this.zones, value); }
		public ZoneVM SelectedZone { get => this.selectedZone; set => SetProperty(ref this.selectedZone, value); }
		public int[] Rows { get => this.rows; set => SetProperty(ref this.rows, value); }
		public int[] Columns { get => this.columns; set => SetProperty(ref this.columns, value); }
		public double[] RowSizes { get => this.rowSizes; set => SetProperty(ref this.rowSizes, value); }
		public double[] ColumnSizes { get => this.columnSizes; set => SetProperty(ref this.columnSizes, value); }


		public int HDivisions
		{
			get => hDivisions;
			set
			{
				SetProperty(ref this.hDivisions, value);
			}
		}

		public int VDivisions
		{
			get => vDivisions;
			set
			{
				SetProperty(ref this.vDivisions, value);
			}
		}

		public void ResizeZones(double[] rows, double[] columns)
		{
			this.RowSizes = rows;
			this.ColumnSizes = columns;

			this.UpdateZones();
			foreach(var zone in Zones)
			{
				this.mainWindow.InsertInZone(zone);
			}
		}

		public DataModel.Geometry.Rect GetZone(int row, int column)
		{
			double top = Sum(this.RowSizes.Take(row))/factor;
			double left = Sum(this.ColumnSizes.Take(column)) / factor;
			double bot = Sum(this.RowSizes.Take(row + 1)) / factor;
			double right = Sum(this.ColumnSizes.Take(column + 1)) / factor;

			return new DataModel.Geometry.Rect
			{
				Bottom = (int)Math.Round(bot),
				Right = (int)Math.Round(right),
				Top = (int)Math.Round(top),
				Left = (int)Math.Round(left),

			};
		}

		public void UpdateZones()
		{
			double rowSum = 0;
			for (int r = 0; r < this.VDivisions; r++)
			{
				double columnSum = 0;
				for (int c = 0; c < this.HDivisions; c++)
				{
					this.Zones[r, c].Rectangle = new RectangleVM(
						new RectD
						{
							Left = 0,
							Top = 0,
							Right = this.ColumnSizes[c],
							Bottom = this.RowSizes[r],
						},
						this.factor
						);
					;

					columnSum += this.ColumnSizes[c];
				}
				rowSum += this.RowSizes[r];
			}
		}

		public IEnumerable<ZoneVM> ZoneList 
		{ get 
			{
				var list = new List<ZoneVM>();
				for (int r = 0; r < this.VDivisions; r++)
				{
					for (int c = 0; c < this.HDivisions; c++)
					{
						list.Add(this.Zones[r, c]);
					}
				}
				return list;
			} 
		}

		public (int row, int columns)? ContainesZone(ZoneVM zoneVM)
		{
			for (int r = 0; r < this.VDivisions; r++)
			{
				for (int c = 0; c < this.HDivisions; c++)
				{
					if (this.Zones[r, c] == zoneVM)
					{
						return (r, c);
					}
				}
			}

				return null;
		}

		public static double Sum(IEnumerable<double> collection)
		{
			double sum = 0;
			foreach (var val in collection)
			{
				sum += val;
			}
			return sum;
		}

		private void PopulateZones()
		{
			var tempZones = new ZoneVM[this.VDivisions, this.HDivisions];
			var tempRows = new int[this.VDivisions];
			var tempRowSizes = new double[this.VDivisions];
			var tempColumns = new int[this.HDivisions];
			var tempColumnSizes = new double[this.HDivisions];

			for (int r = 0; r < this.VDivisions; r++)
			{
				for (int c = 0; c < this.HDivisions; c++)
				{
					tempZones[r, c] = new ZoneVM(this.mainWindow, this, r, c);
				}
			}

			for (int r = 0; r < this.VDivisions; r++)
			{
				tempRows[r] = r;
				tempRowSizes[r] = this.RectangleVm.ConfiguredRectangle.Height / this.VDivisions;
			}

			for (int c = 0; c < this.HDivisions; c++)
			{
				tempColumns[c] = c;
				tempColumnSizes[c] = this.RectangleVm.ConfiguredRectangle.Width / this.HDivisions;
			}
			this.Zones = tempZones;

			this.DefaultHeight = this.RectangleVm.ConfiguredRectangle.Height / this.VDivisions;

			this.Rows = tempRows;
			this.RowSizes = tempRowSizes;

			this.Columns = tempColumns;
			this.ColumnSizes = tempColumnSizes;
		}
	}
}
