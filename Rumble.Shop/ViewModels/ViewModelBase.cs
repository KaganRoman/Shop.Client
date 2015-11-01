using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Rumble.Shop
{
	public class ViewModelBase : INotifyPropertyChanged {
		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			if (PropertyChanged != null)
				PropertyChanged (this, new PropertyChangedEventArgs (propertyName));
		}
		#endregion
	}
}

