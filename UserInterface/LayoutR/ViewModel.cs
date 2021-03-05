using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LayoutR
{
	public class ViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected bool SetProperty<T>(ref T backingField, T value, [CallerMemberName] string propertyname = null)
		{
			// Check if the value and backing field are actualy different
			if (EqualityComparer<T>.Default.Equals(backingField, value))
			{
				return false;
			}

			// Setting the backing field and the RaisePropertyChanged
			backingField = value;
			RaisePropertyChanged(propertyname);
			return true;
		}

		protected void RaisePropertyChanged(string propertyname)
		{
			this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
		}
	}
}