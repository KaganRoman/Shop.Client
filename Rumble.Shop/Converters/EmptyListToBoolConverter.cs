using System;
using Xamarin.Forms;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

namespace Rumble.Shop.Converters
{
	public class EmptyListToBoolConverter : IValueConverter
	{
		#region IValueConverter implementation
		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var e = value as IEnumerable<object>;
			return e != null && e.Any();
		}
		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException ();
		}
		#endregion
	}
}

