using System;
using System.Windows.Input;

namespace Rumble.Shop
{
	public class ProductViewModel : ViewModelBase
	{
		public string Name { get; set;}
		public string ImageUrl { get; set; }
		public string Price { get; set; }
		public double PriceD { get; set; }
		public int Rating { get; set; }
		public string Category { get; set; }
		public string Details { get; set; }

		public ICommand ClickCommand { get; set; }

		private bool _added;
		public bool Added { 
			get { 
				return _added; 
			}
			set { 
				if (_added != value) {
					_added = value;
					OnPropertyChanged ("Added");
				}
			}
		}

	}
}

