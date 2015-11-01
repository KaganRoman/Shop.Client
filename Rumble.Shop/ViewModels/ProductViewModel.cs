using System;
using System.Windows.Input;
using Rumble.Shop.Models;
using Xamarin.Forms;

namespace Rumble.Shop
{
	public class ProductViewModel : ViewModelBase
	{
		public ProductViewModel(Product p, Action<object> action)
		{
			Product = p;
			ClickCommand = new Command(action);
		}

		public Product Product { get; set; }

		public ICommand ClickCommand { get; set; }


		public string Name => Product.Name;
		public string ImageUrl => Product.ImageUrl;
		public string Price => Product.Price;
		public double PriceD => Product.PriceD;
		public int Rating => Product.Rating;
		public string Category => Product.Category;
		public string Details => Product.Details;

		public bool Added { 
			get { 
				return Product.Added; 
			}
			set { 
				if (Product.Added != value) {
					Product.Added = value;
					OnPropertyChanged ("Added");
				}
			}
		}
	}
}

