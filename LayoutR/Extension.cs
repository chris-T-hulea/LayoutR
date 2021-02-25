using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace LayoutR
{
	public static class Extension
	{
		public static void LinkRowHeightsToUserChange(this DataGrid dataGrid)
		{
			double? heightToApply = null;
			bool userTriggered = false;

			if (dataGrid.RowHeaderStyle != null)
				dataGrid.RowHeaderStyle = new Style(typeof(DataGridRowHeader));
			if (dataGrid.RowStyle == null)
				dataGrid.RowStyle = new Style(typeof(DataGridRow));

				dataGrid.RowStyle.Setters.Add(new EventSetter()
			{
				Event = DataGridRow.SizeChangedEvent,
				Handler = new SizeChangedEventHandler((r, sizeArgs) =>
				{
					if (userTriggered && sizeArgs.HeightChanged)
						heightToApply = sizeArgs.NewSize.Height;
				})
			});
			dataGrid.RowHeaderStyle.Setters.Add(new EventSetter()
			{
				Event = DataGridRowHeader.PreviewMouseDownEvent,
				Handler = new MouseButtonEventHandler(
							(rh, e) => userTriggered = true)
			});
			dataGrid.RowHeaderStyle.Setters.Add(new EventSetter()
			{
				Event = DataGridRowHeader.MouseLeaveEvent,
				Handler = new MouseEventHandler((o, mouseArgs) =>
				{
					if (heightToApply.HasValue)
					{
						userTriggered = false;
						var itemsSource = dataGrid.ItemsSource;
						dataGrid.ItemsSource = null;
						dataGrid.RowHeight = heightToApply.Value;
						dataGrid.ItemsSource = itemsSource;
						heightToApply = null;
					}
				})
			});
		}
	}
}
