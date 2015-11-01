using System;
using System.ComponentModel;

namespace Rumble.Shop
{
	public class ViewModelBase : INotifyPropertyChanged {
		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		public void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged (this, new PropertyChangedEventArgs (propertyName));
		}
		#endregion
	}
}

