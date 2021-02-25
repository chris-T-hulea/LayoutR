using DataModel.Entities;
using DataModel.Geometry;
using Prism.Commands;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace LayoutR
{
	public class ZoneVM : ViewModel
	{
		private readonly MainWindowViewModel mainViewModel;
		private double height;
		private double width;
		private Screen screen;
		private int row;
		private int col;
		private RectangleVM rectangle;

		public ZoneVM(MainWindowViewModel mainWindow, DisplayVM displayVM, int r,int c)
		{
			this.mainViewModel = mainWindow;
			DisplayVM = displayVM;
			this.Row = r;
			this.Col = c;
			this.ZoneCommand = new DelegateCommand(OnZoneSelected);
		}

		public ICommand ZoneCommand { get; private set; }

		public RectangleVM Rectangle 
		{ 
			get => rectangle; 
			set 
			{
				SetProperty(ref this.rectangle, value);
				this.Height = rectangle.ActualRectangle.Height;
				this.Width = rectangle.ActualRectangle.Width;
			} 
		}
		public int Row { get => row; set => SetProperty(ref this.row, value); }
		public int Col { get => col; set => SetProperty(ref this.col, value); }
		public double Height { get => height; set => SetProperty(ref this.height, value); }
		public double Width { get => width; set => SetProperty(ref this.width, value); }

		//public Thickness Offset { get => this.offset; set => this.SetProperty(ref this.offset, value); }
		public Screen Screen { get => this.screen; set => this.SetProperty(ref this.screen, value); }

		public DisplayVM DisplayVM { get; }

		private void OnZoneSelected()
		{
			mainViewModel.SelectZone(this);
		}

		public override string ToString()
		{
			return "name";
		}
	}
}
