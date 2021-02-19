using DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using UIUtils;
using WindowController.Interfaces;

namespace LayoutR
{
	public class MainWindowViewModel : ViewModel
	{
		private readonly IWindowConrtoller windowController;
		private readonly IDisplayService displayService;
		private Screen screen;
		private DisplayVM display;
		private int top;
		private int right;
		private int bot;
		private int height;
		private ZoneVM zoneVM;
		private readonly DispatcherTimer timer;

		public MainWindowViewModel(IWindowConrtoller windowController, IDisplayService displayService)
		{
			this.windowController = windowController;
			this.displayService = displayService;
			this.timer = new DispatcherTimer();
			this.SetupTimer();

			this.Screens = new BulkObservableCollection<Screen>();
			this.Displays = new BulkObservableCollection<DisplayVM>(this.displayService.GetAllDisplays().Select(display => new DisplayVM(this, display)));
			this.SelectedDisplay = this.Displays[1];
			OnTimerElapsed(null, null);
		}

		public BulkObservableCollection<Screen> Screens { get; private set; }
		public BulkObservableCollection<DisplayVM> Displays { get; private set; }

		public Screen SelectedScreen
		{
			get => this.screen;
			set
			{
				this.SetProperty(ref this.screen, value);
				this.SetBounds();
			}
		}
		public ZoneVM SelectedZone
		{
			get => this.zoneVM;
			set
			{
				this.SetProperty(ref this.zoneVM, value);
			}
		}
		public DisplayVM SelectedDisplay
		{
			get => this.display;
			set
			{
				this.SetProperty(ref this.display, value);
			}
		}

		public void SelectZone(ZoneVM zone)
		{
			this.SelectedZone = zone;
			this.SelectedZone.Screen = this.SelectedScreen;
			this.windowController.SetScreenBounds(screen, zone.Rectangle, Displays[1].Rectangle.Left, Displays[1].Rectangle.Top);
		}

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
	}
}