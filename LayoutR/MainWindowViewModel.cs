using DataModel.Entities;
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
		private Screen screen;
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

			this.Screens = new BulkObservableCollection<Screen>();
			this.Displays = new BulkObservableCollection<DisplayVM>();

			this.ReloadDisplays();

			this.SelectedZoneCommand = new DelegateCommand<ZoneVM>(this.OnZoneSelected);

			OnTimerElapsed(null, null);
		}

		#endregion

		#region Properties

		/// <summary>
		/// The list of available screens.
		/// </summary>
		public BulkObservableCollection<Screen> Screens { get; private set; }

		/// <summary>
		/// The list of available displays.
		/// </summary>
		public BulkObservableCollection<DisplayVM> Displays { get; private set; }

		/// <summary>
		/// The currently selected screen.
		/// </summary>
		public Screen SelectedScreen
		{
			get => this.screen;
			set
			{
				this.SetProperty(ref this.screen, value);
				this.SetBounds();
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
		/// Selcts the zone and inserst the screen in it.
		/// </summary>
		/// <param name="zone"></param>
		public void SelectZone(ZoneVM zone)
		{
			this.SelectedZone = zone;
			this.SelectedZone.Screen = this.SelectedScreen;
			var display = Displays.First(dis => dis.ContainesZone(zone) != null);
			this.windowController.SetScreenBounds(screen, zone.Rectangle.ActualRectangle, display.RectangleVm.ActualRectangle.Left, display.RectangleVm.ActualRectangle.Top);
		}

		#endregion

		#region Private Methods

		private void SetBounds()
		{
			if (this.SelectedScreen != null)
			{
				//(this.Left, this.Top, this.Right, this.Bot) = this.windowController.GetScreenBounds(SelectedScreen);
			}
		}

		private void SetupTimer()
		{
			this.timer.Interval = TimeSpan.FromSeconds(1); // milliseconds
			this.timer.Tick += OnTimerElapsed;
			this.timer.Tick += this.OnTimerElapsed;
			//this.timer.Start();
		}

		private void OnTimerElapsed(object sender, EventArgs e)
		{
			using (this.Screens.SuppressiNotifications())
			{
				IEnumerable<Screen> source = this.windowController.GetAllWindows().OrderBy(screen => screen.Name);
				this.Screens.Clear();
				this.Screens.AddRange(source);
			}
		}

		private static object GetPropValue(object src, string propName)
		{
			return src.GetType().GetProperty(propName,BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)?.GetValue(src, null);
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
			if (this.SelectedScreen == null)
			{
				if (zoneVM.Screen != null)
				{
					this.windowController.SetScreenBounds(zoneVM.Screen, rect, zoneVM.DisplayVM.RectangleVm.ActualRectangle.Left, zoneVM.DisplayVM.RectangleVm.ActualRectangle.Top);
				}
				return;
			}
			zoneVM.Screen = this.SelectedScreen;
			this.SelectedScreen = null;
			this.windowController.SetScreenBounds(zoneVM.Screen, rect, zoneVM.DisplayVM.RectangleVm.ActualRectangle.Left, zoneVM.DisplayVM.RectangleVm.ActualRectangle.Top);

			ZoneVM found = null;
			this.Displays.ToList().ForEach(dis => found = found ?? dis.ZoneList.FirstOrDefault(zone => zone.Screen == zoneVM.Screen && zone != zoneVM));

			if (found != null)
			{
				found.Screen = null;
			}
		}

		#endregion
	}
}