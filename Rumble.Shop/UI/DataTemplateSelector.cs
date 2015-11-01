using System;
using Xamarin.Forms;

namespace Rumble.Shop.UI
{
	public class DataTemplateSelector
	{
		public virtual DataTemplate SelectTemplate(object item, BindableObject container)
		{
			return null;
		}
	}
}

