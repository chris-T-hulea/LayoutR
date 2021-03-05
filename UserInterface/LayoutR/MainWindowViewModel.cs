using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Threading;
using UIUtils;
using WindowController.Interfaces;

namespace LayoutR
{
	public class MainWindowViewModel : ViewModel
	{
		#region Privatee fields

		private readonly IWindowConrtoller windowController;
		private readonly IDisplayService displayService;
		private DataModel.Entities.App selectedApplication;
		private DisplayVM display;
		private ZoneVM zoneVM;
		private readonly DispatcherTimer timer;

		#endregion

		#region Constructor

		public MainWindowViewModel(IWindowConrtoller windowController, IDisplayService displayService)
		{
			this.windowController = windowController;
			this.displayService = displayService;
			this.timer = new DispatcherTimer();
			this.SetupTimer();

			this.Applications = new BulkObservableCollection<DataModel.Entities.App>();
			this.Displays = new BulkObservableCollection<DisplayVM>();

			this.ReloadDisplays();

			this.SelectedZoneCommand = new DelegateCommand<ZoneVM>(this.OnZoneSelected);

			OnTimerElapsed(null, null);
			SetupTimer();
		}

		#endregion

		#region Properties

		/// <summary>
		/// The list of available applications.
		/// </summary>
		public BulkObservableCollection<DataModel.Entities.App> Applications { get; private set; }

		/// <summary>
		/// The list of available displays.
		/// </summary>
		public BulkObservableCollection<DisplayVM> Displays { get; private set; }

		/// <summary>
		/// The currently selected application.
		/// </summary>
		public DataModel.Entities.App SelectedApplication
		{
			get => this.selectedApplication;
			set
			{
				this.SetProperty(ref this.selectedApplication, value);
			}
		}

		/// <summary>
		/// The currently selected zone.
		/// </summary>
		public ZoneVM SelectedZone
		{
			get => this.zoneVM;
			set
			{
				this.SetProperty(ref this.zoneVM, value);
			}
		}

		/// <summary>
		/// The currently seleected display.
		/// </summary>
		public DisplayVM SelectedDisplay
		{
			get => this.display;
			set
			{
				this.SetProperty(ref this.display, value);
			}
		}
		/// <summary>
		/// The command for selecting a zone.
		/// </summary>
		public DelegateCommand<ZoneVM> SelectedZoneCommand { get; set; }

		#endregion

		#region Public Methods

		/// <summary>
		/// Selcts the zone and inserst the application in it.
		/// </summary>
		/// <param name="zone"></param>
		public void SelectZone(ZoneVM zone)
		{
			this.SelectedZone = zone;
			this.SelectedZone.Application = this.SelectedApplication;
			var display = Displays.First(dis => dis.ContainesZone(zone) != null);
			this.windowController.SetApplicationBounds(selectedApplication, zone.Rectangle.ActualRectangle, display.RectangleVm.ActualRectangle.Left, display.RectangleVm.ActualRectangle.Top);
		}

		#endregion

		#region Private Methods

		private void SetupTimer()
		{
			this.timer.Interval = TimeSpan.FromMilliseconds(500); 
			this.timer.Tick += OnTimerElapsed;
			this.timer.Tick += this.OnTimerElapsed;
			this.timer.Start();
		}

		private void OnTimerElapsed(object sender, EventArgs e)
		{
			IEnumerable<DataModel.Entities.App> source = this.windowController.GetAllApplication().OrderBy(app => app.Name);
			var common = this.Applications.Where(app => source.Any(inApp => inApp.Pointer == app.Pointer)).ToList();
			var selectedApp = this.SelectedApplication;

			if(common.Count() == source.Count())
			{
				return;
			}

			using (this.Applications.SuppressiNotifications())
			{
				this.Applications.Clear();
				this.Applications.AddRange(common);
				this.Applications.AddRange(source.Where(app=>common.All(inApp => inApp.Id != app.Id)));
			}

			selectedApp = this.Applications.FirstOrDefault(app => app?.Id == selectedApp?.Id);
			if (selectedApp != null)
			{
				this.SelectedApplication = selectedApp;
			}
		}
		private void ReloadDisplays()
		{
			var allDisplays = this.displayService.GetAllDisplays().ToList();
			var tempDisplays = new List<DisplayVM>();
			for (int i = 0; i < allDisplays.Count; i++)
			{
				tempDisplays.Add(new DisplayVM(this, allDisplays[i], i));
			}

			using (this.Displays.SuppressiNotifications())
			{
				this.Displays.Clear();
				this.Displays.AddRange(tempDisplays);
			}
		}

		private void OnZoneSelected(ZoneVM zone)
		{
			this.SelectedDisplay = this.Displays.First(disp => disp.ContainesZone(zone) != null);
			this.SelectedDisplay.SelectedZone = zone;
			this.SelectedZone = zone;
		}

		internal void InsertInZone(ZoneVM zoneVM)
		{
			DisplayVM displayVM = zoneVM.DisplayVM;
			var r = zoneVM.Row;
			var c = zoneVM.Col;
			var rect = displayVM.GetZone(r, c);
			if (this.SelectedApplication == null)
			{
				if (zoneVM.Application != null)
				{
					this.windowController.SetApplicationBounds(zoneVM.Application, rect, zoneVM.DisplayVM.RectangleVm.ActualRectangle.Left, zoneVM.DisplayVM.RectangleVm.ActualRectangle.Top);
				}
				return;
			}
			zoneVM.Application = this.SelectedApplication;
			this.SelectedApplication = null;
			this.windowController.SetApplicationBounds(zoneVM.Application, rect, zoneVM.DisplayVM.RectangleVm.ActualRectangle.Left, zoneVM.DisplayVM.RectangleVm.ActualRectangle.Top);

			ZoneVM found = null;
			this.Displays.ToList().ForEach(dis => found = found ?? dis.ZoneList.FirstOrDefault(zone => zone.Application == zoneVM.Application && zone != zoneVM));

			if (found != null)
			{
				found.Application = null;
			}
		}

		#endregion
	}
}