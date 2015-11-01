using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Rumble.Shop.Models;
using Xamarin.Forms;

namespace Rumble.Shop.ViewModels
{
	public class OrderViewModel : ViewModelBase
	{
		public OrderViewModel(Order p, Action<object> action)
		{
			Order = p;
			ClickCommand = new Command(action);
		}

		public Order Order { get; set; }

		public ICommand ClickCommand { get; set; }


		public string Name => Order.Name;
		public string ImageUrl => Order.ImageUrl;
		public string Price => Order.Price;
		public double PriceD => Order.PriceD;
		public string Status => Order.Status;
	}
}
