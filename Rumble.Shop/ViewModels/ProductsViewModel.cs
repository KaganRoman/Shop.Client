using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using System.Xml.Linq;
using System.ComponentModel;
using Rumble.Shop.Models;
using Rumble.Shop.ViewModels;
using Xamarin.Forms;

namespace Rumble.Shop
{
	public class ProductsViewModel : ViewModelBase
	{
		private readonly Action<object> _selectItemAction;

		public ProductsViewModel(Action<object> selectItemAction)
		{
			_selectItemAction = selectItemAction;
			Items = new List<ProductViewModel>(ProductsService.Products.Items.Select(p => new ProductViewModel(p, selectItemAction)));
			FeaturedItems = new List<ProductViewModel>(ProductsService.Products.FeaturedItems.Select(p => new ProductViewModel(p, selectItemAction)));
			MainItems = new List<ProductViewModel>(ProductsService.Products.MainItems.Select(p => new ProductViewModel(p, selectItemAction)));
			Orders = new List<OrderViewModel>(ProductsService.Products.Orders.Select(p => new OrderViewModel(p, selectItemAction)));
			CategoryItems = FeaturedItems;
		}


		public void RefreshCommand()
		{
			OnPropertyChanged("TotalPrice");
			OnPropertyChanged("AddedItems");
		}

		public double TotalPrice
		{
			get {
				return AddedItems.Sum (i => i.Product.PriceD);
			}
		}

		public IEnumerable<ProductViewModel> AddedItems
		{
			get
			{
				return ProductsService.Products.AllItems.Where(i => i.Added).Select(p => new ProductViewModel(p, _selectItemAction));
			}
		}

		public List<OrderViewModel> Orders { get; set; }

		public List<ProductViewModel> FeaturedItems { get; set; }

		private List<ProductViewModel> _categoryItems;
        public List<ProductViewModel> CategoryItems
		{
			get { return _categoryItems; }
	        set
	        {
		        if (_categoryItems != value)
		        {
			        _categoryItems = value;
					OnPropertyChanged();
					OnPropertyChanged("GroupedItems");
				}
			}
		}

		private Category _category;
		public Category CurrentCategory
		{
			get { return _category; }
			set
			{
				if (_category != value)
				{
					_category = value;
					CategoryItems = new List<ProductViewModel>();
                    ProductsService.Products.GetCategoryItems(_category).ContinueWith((task => 
						CategoryItems = task.Result.Select(p => new ProductViewModel(p, _selectItemAction)).ToList()
					));
				}
			}
		}

		private List<ProductViewModel> _items;
		public List<ProductViewModel> Items 
		{ 	
			get { return _items; }
			set {
				if (value != _items) {
					_items = value;
					OnPropertyChanged();
					OnPropertyChanged("GroupedItems");
					OnPropertyChanged("TotalPrice");
					OnPropertyChanged("AddedItems");
				}
			}
		}
		public IEnumerable<IGrouping<string, ProductViewModel>> GroupedItems { 
			get 
			{
				var items = CategoryItems.GroupBy (k => k.Product.Category).ToList();
				var keys = items.Select (k => k.Key).ToList();
				return items;
			} 
		}

		public ProductViewModel MainItem { get { return MainItems [0];} }

		public List<ProductViewModel> MainItems { get; set; }
		public IEnumerable<IGrouping<string, ProductViewModel>> MainGroupedItems { 
			get 
			{
				var items = MainItems.GroupBy (k => k.Product.Category);
				return items;
			} 
		}

		public class Grouping : List<object>
		{
			public string Key { get; private set; }
			public int Height { get; set; }
			public ProductViewModel Item { get; set; }
			public Grouping(string key, int height, IEnumerable<object> items)
			{
				Key = key;
				Height = height;
				AddRange(items);
			}
		}
		public IEnumerable<Grouping> HomeItems
		{
			get
			{
				var l = new List<Grouping>
				{
					new Grouping("0", 280, ProductsService.Products.Categories.GetRange(0,1))
					{
						Item = MainItem
					},

					new Grouping("1", 1, ProductsService.Products.Categories.GetRange(1,1))
					{
					},

					new Grouping("2", 1, ProductsService.Products.Categories.GetRange(2,5))
					{
					},

				};
				return l;
			}
		}
	}
}

