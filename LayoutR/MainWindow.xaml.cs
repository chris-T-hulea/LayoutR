using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using Unity;

namespace LayoutR
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		bool done = false;
		double? heightToApply = null;
		bool userTriggered = false;

		IEnumerable<DataGrid> grids;

		public MainWindow()
		{
			var container = new UnityContainer();

			this.RunBootstrapper(container);

			this.DataContext = container.Resolve<MainWindowViewModel>();
			this.InitializeComponent();
			Setup();

		}


		private void RunBootstrapper(Unity.UnityContainer container)
		{
			container.RegisterInstance<IUnityContainer>(container);
			container.Resolve<MainBootstrapper>().Run();
		}

		private void CellDoubleClicked(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			DataGridCell cell = (DataGridCell)sender;
			var arr = (Gu.Wpf.DataGrid2D.Array2DRowView)cell.DataContext;
			DataGrid grid = GetPropValue((cell.Column), "DataGridOwner") as DataGrid;
			if (!(grid?.DataContext is DisplayVM display))
				return;

			int cellColumn = cell.Column.DisplayIndex;
			int cellRow = arr.Index;
			MainWindowViewModel viewModel = DataContext as MainWindowViewModel;

			viewModel.SelectedZoneCommand.Execute(display.Zones[cellRow, cellColumn]);
			viewModel.InsertInZone(display.Zones[cellRow, cellColumn]);
		}

		private async void Setup()
		{
			ItemsControl container = null;
			{
				while (true)
				{
					await Task.Delay(100);
					container = (ItemsControl)this.FindName("Container");
					if (container != null)
					{
						break;
					}

				}
			}
		}
		public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
		{
			if (depObj != null)
			{
				for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
				{
					DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
					if (child != null && child is T)
					{
						yield return (T)child;
					}

					foreach (T childOfChild in FindVisualChildren<T>(child))
					{
						yield return childOfChild;
					}
				}
			}
		}


		public void FrameworkElement_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			DataGridCell cell = (DataGridCell)sender;
			var arr = (Gu.Wpf.DataGrid2D.Array2DRowView)cell.DataContext;
			DataGrid grid = GetPropValue((cell.Column), "DataGridOwner") as DataGrid;
			if (!(grid?.DataContext is DisplayVM display))
				return;

			int rowC = grid.ItemContainerGenerator.Items.Count;
			int columnC = grid.Columns.Count;

			double[] rows = new double[rowC];
			double[] columns = new double[columnC];

			int cellColumn = cell.Column.DisplayIndex;
			int cellRow = arr.Index;

			for (int i = 0; i < rowC; i++)
			{
				rows[i] = ((DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(i))?.ActualHeight ?? 0;
			}

			for (int i = 0; i < columnC; i++)
			{
				columns[i] = grid.Columns[i].ActualWidth;
			}
			if (!done)
			{
				if(this.grids == null)
				{
					this.grids = FindVisualChildren<DataGrid>(this);
				}

				foreach (var item in this.grids)
				{

					//item.LinkRowHeightsToUserChange();
					//double currentSize = 0;
					//double diff = 0;
					//DataGridRow dataGridRow = null;
					//for (int i = 0; i < rowC; i++)
					//{
					//	DisplayVM currentDisplay = (DisplayVM)item.DataContext;
					//	dataGridRow = (DataGridRow)item.ItemContainerGenerator.ContainerFromIndex(i);

					//	if (double.IsNaN(dataGridRow.Height))
					//	{
					//		dataGridRow.Height = currentDisplay.Zones[i, 0].Rectangle.ConfiguredRectangle.Height;
					//		dataGridRow.InvalidateVisual();
					//		dataGridRow.Height = double.NaN;
					//	}
					//}
				}
				done = true;
			}
			display.ResizeZones(rows, columns);
					//break;
					//currentSize += dataGridRow.Height;
					//diff = (item.ActualHeight - item.ColumnHeaderHeight) - currentSize;
					//if (diff < 0)
					//{
					//	dataGridRow.Height = dataGridRow.Height + diff;
					//	currentSize = 0;
					//	diff = 0;
					//}

				//if (diff > 0)
				//{
				//	dataGridRow.Height = diff;
				//}


		}
		public static object GetPropValue(object src, string propName)
		{
			return src.GetType().GetProperty(propName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)?.GetValue(src, null);
		}

		private void OnResize(object sender, SizeChangedEventArgs sizeArgs)
		{
			DataGridRowHeader gridRow = sender as DataGridRowHeader;
			DataGrid grid = this.grids?.Where(g => FindVisualChildren<DataGridRowHeader>(g).Contains(gridRow)).FirstOrDefault();

			if (userTriggered && sizeArgs.HeightChanged)
				heightToApply = sizeArgs.NewSize.Height;


			ApplySize(grid);

		}
		private void OnPreviewMouseDownEvent(object sender, MouseButtonEventArgs sizeArgs) => userTriggered = true;
		private void OnMouseLeaveEvent(object sender, MouseEventArgs sizeArgs)
		{

			DataGridRow gridRow = sender as DataGridRow;

			DataGrid grid = this.grids.Where(g => FindVisualChildren<DataGridRow>(g).Contains(gridRow)).FirstOrDefault();

			ApplySize(grid);
		}

		private static void ApplySize(DataGrid grid)
		{
			if (grid == null)
			{
				return;
			}

			if (!(grid?.DataContext is DisplayVM display))
				return;

			int rowC = grid.ItemContainerGenerator.Items.Count;
			int columnC = grid.Columns.Count;

			double[] rows = new double[rowC];
			double[] columns = new double[columnC];

			for (int i = 0; i < rowC; i++)
			{
				rows[i] = ((DataGridRow)grid.ItemContainerGenerator.ContainerFromIndex(i))?.ActualHeight ?? 0;
			}

			for (int i = 0; i < columnC; i++)
			{
				columns[i] = grid.Columns[i].ActualWidth;
			}
		}
	}
}

		// TODO Cristi use somtheing from here for resizing
		
		//foreach (var item in FindVisualChildren<DataGrid>(this))
		//{
		//	int rowC = item.ItemContainerGenerator.Items.Count;
		//	double currentSize = 0;
		//	double diff = 0;
		//	DataGridRow dataGridRow = null;
		//	for (int i = 0; i < rowC; i++)
		//	{
		//		dataGridRow = (DataGridRow)item.ItemContainerGenerator.ContainerFromIndex(i);

		//		if (double.IsNaN(dataGridRow.Height))
		//		{
		//			dataGridRow.Height = (item.ActualHeight - item.ColumnHeaderHeight) / item.Items.Count;
		//		}
		//		currentSize += dataGridRow.Height;
		//		diff = (item.ActualHeight - item.ColumnHeaderHeight) - currentSize;
		//		if (diff < 0)
		//		{
		//			dataGridRow.Height = dataGridRow.Height + diff;
		//			currentSize = 0;
		//			diff = 0;
		//		}
		//	}

		//	if (diff > 0)
		//	{
		//		dataGridRow.Height = diff;
		//	}
		//}
