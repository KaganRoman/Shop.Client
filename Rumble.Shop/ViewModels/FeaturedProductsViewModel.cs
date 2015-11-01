using System;
using System.Collections.Generic;

namespace Rumble.Shop
{
	public class FeaturedProductsViewModel : ViewModelBase
	{
		public List<ProductViewModel> Items { get; set;}
		public FeaturedProductsViewModel (List<ProductViewModel> products)
		{
			Items = products;
		}
	}
}

