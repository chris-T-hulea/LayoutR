using DataModel.Entities;
using DataModel.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LayoutR
{
	/// <summary>
	/// View model class for display.
	/// </summary>
	public class DisplayVM : ViewModel
	{

		#region Private Fields

		private const int headerHeight = 10;
		private readonly MainWindowViewModel mainWindow;
		private readonly Display display;
		private readonly double factor = 0.1;
		private readonly int index;


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

		#endregion

		#region Constructors

		/// <summary>
		/// Initializes a new instance of class <see cref="DisplayVM"/>
		/// </summary>
		/// <param name="mainWindow">The main window view model.</param>
		/// <param name="display">The display entity.</param>
		/// <param name="index">The display horizontal index.</param>
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

			this.PopulateZones();

			this.UpdateZones();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the display default height.
		/// </summary>
		public double DefaultHeight { get => this.defaultHeight; set => SetProperty(ref this.defaultHeight, value); }

		/// <summary>
		/// Gets or sets the display pozitional and size view model.
		/// </summary>
		public RectangleVM RectangleVm { get => this.rectangleVm; set => SetProperty(ref this.rectangleVm, value); }

		/// <summary>
		/// Gets or sets the zone belonging to the display.
		/// </summary>
		public ZoneVM[,] Zones { get => this.zones; set => SetProperty(ref this.zones, value); }

		/// <summary>
		/// Gets or sets the currently selected zone.
		/// </summary>
		public ZoneVM SelectedZone { get => this.selectedZone; set => SetProperty(ref this.selectedZone, value); }

		/// <summary>
		/// Gets or sets the name of the display (temporary)
		/// </summary>
		public string Name { get => this.display.IsPrimary ? "Primary" : "Secondary"; }

		/// <summary>
		/// Gets or sets the row headers.
		/// </summary>
		public int[] Rows { get => this.rows; set => SetProperty(ref this.rows, value); }

		/// <summary>
		/// Gets or sets the column headers.
		/// </summary>
		public int[] Columns { get => this.columns; set => SetProperty(ref this.columns, value); }

		/// <summary>
		/// Gets or sets the row heights of each row.
		/// </summary>
		public double[] RowSizes { get => this.rowSizes; set => SetProperty(ref this.rowSizes, value); }

		/// <summary>
		/// Gets or sets the column widths of each column.
		/// </summary>
		public double[] ColumnSizes { get => this.columnSizes; set => SetProperty(ref this.columnSizes, value); }

		/// <summary>
		/// Gets or sets the number of divisions on the horizontal axis.
		/// </summary>
		public int HDivisions
		{
			get => hDivisions;
			set
			{
				SetProperty(ref this.hDivisions, value);
				this.PopulateZones();
				this.UpdateZones();
			}
		}

		/// <summary>
		/// Gets or sets the number of division on the vertical axis.
		/// </summary>
		public int VDivisions
		{
			get => vDivisions;
			set
			{
				SetProperty(ref this.vDivisions, value);
				this.PopulateZones();
				this.UpdateZones();
			}
		}

		#endregion

		#region PublicMethods

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

		public Rect GetZone(int row, int column)
		{
			double top = Sum(this.RowSizes.Take(row))/factor;
			double left = Sum(this.ColumnSizes.Take(column)) / factor;
			double bot = Sum(this.RowSizes.Take(row + 1)) / factor;
			double right = Sum(this.ColumnSizes.Take(column + 1)) / factor;

			return new Rect
			{
				Bottom = Math.Min((int)Math.Round(bot), this.RectangleVm.ActualRectangle.Height),
				Right = Math.Min((int)Math.Round(right), this.RectangleVm.ActualRectangle.Width),
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
		#endregion

		#region Private Methods

		private static double Sum(IEnumerable<double> collection)
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

		#endregion

	}
}
