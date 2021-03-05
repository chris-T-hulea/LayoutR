using DataModel.Entities;
using Prism.Commands;
using System.Windows.Input;

namespace LayoutR
{
	/// <summary>
	/// View model for reprezenting a display zone.
	/// </summary>
	public class ZoneVM : ViewModel
	{
		#region Private Fields

		private readonly MainWindowViewModel mainViewModel;
		private double height;
		private double width;
		private DataModel.Entities.App application;
		private int row;
		private int col;
		private RectangleVM rectangle;

		#endregion

		#region Constructor

		/// <summary>
		/// Initializes a new instance of clas <see cref="ZoneVM"/>
		/// </summary>
		/// <param name="mainWindow"></param>
		/// <param name="displayVM"></param>
		/// <param name="r"></param>
		/// <param name="c"></param>
		public ZoneVM(MainWindowViewModel mainWindow, DisplayVM displayVM, int r,int c)
		{
			this.mainViewModel = mainWindow;
			DisplayVM = displayVM;
			this.Row = r;
			this.Col = c;
			this.ZoneCommand = new DelegateCommand(OnZoneSelected);
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets the command for zone selection.
		/// </summary>
		public ICommand ZoneCommand { get; private set; }

		/// <summary>
		/// Gets or sets the rectangle view model.
		/// </summary>
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

		/// <summary>
		/// Gets or sets the row index of the zone.
		/// </summary>
		public int Row { get => row; set => SetProperty(ref this.row, value); }

		/// <summary>
		/// Gets or sets the column index of the zone.
		/// </summary>
		public int Col { get => col; set => SetProperty(ref this.col, value); }

		/// <summary>
		/// Gets or sets the zones height.
		/// </summary>
		public double Height { get => height; set => SetProperty(ref this.height, value); }

		/// <summary>
		/// Gets or sets the zones width.
		/// </summary>
		public double Width { get => width; set => SetProperty(ref this.width, value); }

		/// <summary>
		/// Gets or sets the current application binded to the zone.
		/// </summary>
		public DataModel.Entities.App Application { get => this.application; set => this.SetProperty(ref this.application, value); }

		/// <summary>
		/// gets or sets the display the zone belongs to.
		/// </summary>
		public DisplayVM DisplayVM { get; }

		#endregion

		#region Public Methods

		/// <inheritdoc/>
		public override string ToString()
		{
			return "name";
		}
		#endregion

		#region Private methods

		private void OnZoneSelected()
		{
			mainViewModel.SelectZone(this);
		}

		#endregion
	}
}
