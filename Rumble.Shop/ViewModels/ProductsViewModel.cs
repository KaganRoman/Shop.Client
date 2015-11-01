using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using System.Xml.Linq;
using System.ComponentModel;
using Xamarin.Forms;

namespace Rumble.Shop
{
	public class ProductsViewModel : ViewModelBase
	{
		Action<object> _selectItemAction;
		public ProductsViewModel(Action<object> selectItemAction)
		{
			_selectItemAction = selectItemAction;

			Items = new List<ProductViewModel> ();
			FeaturedItems = new List<ProductViewModel> ();

			InitMainItems ();
			InitFeaturedItems ();
			InitItems ();
		}

		public void RefreshCommand()
		{
			OnPropertyChanged("TotalPrice");
			OnPropertyChanged("AddedItems");
		}

		public double TotalPrice
		{
			get {
				return AddedItems.Sum (i => i.TotalPrice);
			}
		}

		public IEnumerable<ProductViewModel> AddedItems
		{
			get {
				return Items.Where (i => i.Added);
			}
		}

		public List<ProductViewModel> FeaturedItems { get; set; }

		private List<ProductViewModel> _items;
		public List<ProductViewModel> Items 
		{ 	
			get { return _items; }
			set {
				if (value != _items) {
					_items = value;
					OnPropertyChanged("Items");
					OnPropertyChanged("GroupedItems");
					OnPropertyChanged("TotalPrice");
					OnPropertyChanged("AddedItems");
				}
			}
		}
		public IEnumerable<IGrouping<string, ProductViewModel>> GroupedItems { 
			get 
			{
				var items = Items.GroupBy (k => k.Category).ToList();
				var keys = items.Select (k => k.Key).ToList();
				return items;
			} 
		}

		public ProductViewModel MainItem { get { return MainItems [0];} }

		public List<ProductViewModel> MainItems { get; set; }
		public IEnumerable<IGrouping<string, ProductViewModel>> MainGroupedItems { 
			get 
			{
				var items = MainItems.GroupBy (k => k.Category);
				return items;
			} 
		}

		public class Grouping : List<Tuple<ProductViewModel, ProductViewModel>>
		{
			public string Key { get; private set; }
			public int Height { get; set; }
			public ProductViewModel Item { get; set; }
			public Grouping(string key, int height, IEnumerable<Tuple<ProductViewModel, ProductViewModel>> items)
			{
				Key = key;
				Height = height;
				AddRange(items);
			}
		}
		public IEnumerable<Grouping> GroupedMainItems
		{
			get
			{
				var items = MainTuppledItems.ToList ();
				var l = new List<Grouping> ();
				l.Add (new Grouping("0", 270, new List<Tuple<ProductViewModel, ProductViewModel>>{ items[0] }) { Item = MainItem});
				l.Add (new Grouping("1", 0, new List<Tuple<ProductViewModel, ProductViewModel>>{ items[1] }));
				l.Add (new Grouping("2", 0, new List<Tuple<ProductViewModel, ProductViewModel>>{ items[2] }));
				return l;
			}
		}

		public List<Tuple<ProductViewModel, ProductViewModel>> MainTuppledItems {
			get {
				return new List<Tuple<ProductViewModel, ProductViewModel>> {
					new Tuple<ProductViewModel, ProductViewModel>(MainItems [1], MainItems [2]),
					new Tuple<ProductViewModel, ProductViewModel>(MainItems [3], MainItems [4]),
					new Tuple<ProductViewModel, ProductViewModel>(MainItems [5], MainItems [6]),
				};
			}
		}

		private void InitMainItems()
		{
			MainItems = new List<ProductViewModel> {
				new ProductViewModel {
					Name = "",
					ImageUrl = "",
					Details = "",
					Price = "",
					PriceD = 0,
					Category = "מיוחדים",
				},
				new ProductViewModel {
					Category = "מיוחדים",
					Name = "המבורגר אמריקאי",
					ImageUrl = "http://shop.vacuumsealersunlimited.com/images/13612318500351088375803.jpeg",
					Price = " 40 ש' לק\"ג",
					PriceD = 40,
					Details = @"חדש!! המבורגר אמריקאי קטלני - תרכובת עצמית ובלעדית"
				},
				new ProductViewModel {
					Category = "מיוחדים",
					Name = "שווארמה בקר",
					ImageUrl = "http://www.mega.co.il/resources/9919.jpg",
					Price = " 40 ש' לק\"ג",
					PriceD = 40,
					Details = @"חדש!! שווארמה בקר יחודית במינה"
				},
				new ProductViewModel {
					Category = "מיוחדים",
					Name = "קבב הבית טרי",
					ImageUrl = "http://www.alfredos.co.il/wp-content/uploads/2014/11/קבב-ביתי1.jpg",
					Price = " 45 ש' לק\"ג",
					PriceD = 45,
					Details = "קבב הבית טרי (מתכון מיוחד) "
				},
				new ProductViewModel {
					Category = "מיוחדים",
					Name = "שניצל שייטל",
					ImageUrl = "http://www.ynet.co.il/PicServer2/13062011/3308683/20_wa.jpg",
					Details = "שניצל שייטל, בקר פרוס דק ורך",
					Price = " 50 ש' לק\"ג",
					PriceD = 50,
				},
				new ProductViewModel {
					Category = "מיוחדים",
					Name = "סטייק אנטריקוט דבש",
					ImageUrl = "http://www.edenteva.co.il/Data/ImportedProductsNew/230021.jpg",
					Details = "סטייק אנטריקוט דבש",
					Price = " 100 ש' לק\"ג",
					PriceD = 100,
				},
				new ProductViewModel {
					Category = "מיוחדים",
					Name = "סטייק סינטה",
					ImageUrl = "http://img.mako.co.il/2008/01/12/01/raw-meat_c.jpg",
					Details = "סטייק סינטה גבוהה ושמנה",
					Price = " 85 ש' לק\"ג",
					PriceD = 85,
				},
			};

			foreach (var item in MainItems)
				item.ClickCommand = new Command (_selectItemAction);
		}

		public class Feed
		{
			public string Title { get; set; }
			public string Link { get; set; }
			public string Description { get; set; }
			public DateTime PublicationDate { get; set; }
			public string GUID { get; set; }
		}

		private async Task InitFeaturedItems()
		{
			FeaturedItems = new List<ProductViewModel> {
				new ProductViewModel {
					Category = "מבצעים",
					Name = @"10 ק""ג בשר טחון טרי",
					ImageUrl = "http://orders.webyarok.biz/wp-content/uploads/2012/10/tachun_bakar1.jpg",
					Price = "300 ש' 10 ק\"ג",
					PriceD = 300,
					Details = @" בשר טחון טרי (לטחינה במקום)",
					Discount = 30
				},
				new ProductViewModel
				{
					Name = "4 קג אסאדו",
					ImageUrl = "http://www.jackspratsbutchery.com.au/wp-content/uploads/2012/07/Rib-eye-roast-grass-fed-beef-Jack-Sprats-Butchery-Large1.jpg",
					Details = "אסאדו של מבכירות היישר ממקום הגידול ברמת הגולן ",
					Price = "100 ש' 4 ק\"ג",
					PriceD = 100,
					Discount = 15,
					Category = "מבצעים",
				},
				new ProductViewModel
				{
					Name = "3 קג בשר טחון",
					ImageUrl = "http://orders.webyarok.biz/wp-content/uploads/2012/10/tachun_bakar1.jpg",
					Details = "טחינה במקום - לקציצות בשר ברוטב/ לקובה עיראקית/לבולונז/לקבב",
					Price = "105 ש' 3 ק\"ג",
					PriceD = 105,
					Discount = 15,
					Category = "מבצעים",
				},
				new ProductViewModel
				{
					Name = "3 קג קבב הבית טרי",
					ImageUrl = "http://www.alfredos.co.il/wp-content/uploads/2014/11/קבב-ביתי1.jpg",
					Details = "קבב הבית טרי (מתכון מיוחד)",
					Price = "120 ש' 3 ק\"ג",
					PriceD = 120,
					Discount = 20,
					Category = "מבצעים",
				},
				new ProductViewModel
				{
					Name = "3 קג שניצל שייטל",
					ImageUrl = "http://www.ynet.co.il/PicServer2/13062011/3308683/20_wa.jpg",
					Details = "שניצל שייטל, בקר פרוס דק ורך",
					Price = "135 ש' 3 ק\"ג",
					PriceD = 135,
					Discount = 20,
					Category = "מבצעים",
				},
				new ProductViewModel
				{
					Name = "3 קג אוסובוקו עסיסי",
					ImageUrl = "http://www.yepmarket.co.il/uploadPictures/osoboko.jpg",
					Details = "אוסובוקו עסיסי במיוחד",
					Price = "105 ש' 3 ק\"ג",
					PriceD = 105,
					Discount = 15,
					Category = "מבצעים",
				},
				new ProductViewModel
				{
					Name = "3 קג בשר פרוסות/קוביות",
					ImageUrl = "http://blog.tapuz.co.il/tastefood/images/%7B04BBA43C-4579-45E7-ACED-B0F8D15A99E4%7D.jpg",
					Details = "בשר פרוסות/קוביות (כתף/צ'אך/אווזית)",
					Price = "105 ש' 3 ק\"ג",
					PriceD = 105,
					Discount = 15,
					Category = "מבצעים",
				},
				new ProductViewModel
				{
					Name = "3 קג סטייק אנטריקוט",
					ImageUrl = "http://www.edenteva.co.il/Data/ImportedProductsNew/230021.jpg",
					Details = "סטייק אנטריקוט דבש",
					Price = "285 ש' 3 ק\"ג",
					PriceD = 285,
					Discount = 25,
					Category = "מבצעים",
				},
				new ProductViewModel
				{
					Name = "3 קג סטייק סינטה",
					ImageUrl = "http://img.mako.co.il/2008/01/12/01/raw-meat_c.jpg",
					Details = "סטייק סינטה גבוהה ושמנה",
					Price = "240 ש' 3 ק\"ג",
					PriceD = 240,
					Discount = 25,
					Category = "מבצעים",
				},

			};
			foreach (var item in FeaturedItems)
				item.ClickCommand = new Command (_selectItemAction);

			//FeaturedItems = await LoadItems ("http://www.rakuten.com/ct/rss/todaysdeals.xml");
		}

		private async Task InitItems()
		{
			Items = FeaturedItems.Concat (MainItems.Skip(1)).ToList ();
		}

		private async Task<List<ProductViewModel>> LoadItems(string url)
		{
			var items = new List<ProductViewModel> ();
			try
			{
				using (var clientHandler = new HttpClientHandler())
				using (var httpClient = new HttpClient(clientHandler))
				{
					var response = await httpClient.GetAsync(url, new CancellationToken());
					var rss = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					XDocument doc = XDocument.Parse(rss);
					foreach(var item in doc.Element("rss").Element("channel").Elements("item"))
					{
						var product = new ProductViewModel { Name = item.Element("title").Value, 
							ClickCommand = new Command(_selectItemAction) };
						foreach(var n in item.Elements())
						{
							if(n.Name.ToString() == "{http://www.rakuten.com/rss/module/productV2/}price")
								product.Price = n.Value;
							if(n.Name.ToString() == "{http://www.rakuten.com/rss/module/productV2/}categoryName")
								product.Category = n.Value;
							if(n.Name.ToString() == "{http://www.rakuten.com/rss/module/productV2/}description")
								product.Details = n.Value;
							if(n.Name.ToString() == "{http://www.rakuten.com/rss/module/productV2/}imagelink")
							{
								product.ImageUrl = n.Value;
								product.Rating = n.Value.Length%5+1;
							}
						}
						if(product.Name.Length > 70)
							product.Name = product.Name.Substring(0, 50 + product.Name.Substring(50).IndexOf(' '));
						product.Name = product.Name.Trim();
						if(product.Name.EndsWith(","))
							product.Name = product.Name.Substring(0, product.Name.Length - 1);
						if(product.Details.Length > 500)
						{
							var ind = product.Details.IndexOf("\n");
							if(ind > 0 && ind < 500)
								product.Details = product.Details.Substring(0, ind);
							else
								product.Details = product.Details.Substring(0, 500 + product.Details.Substring(500).IndexOf(' ')) + "...";
						}
						double price = 0;
						double.TryParse(product.Price, out price);
						product.PriceD = price;
						items.Add(product);
					}
					return items;
				}
			}
			catch(Exception e)
			{
			}
			return new List<ProductViewModel> ();
		}
	}
}

