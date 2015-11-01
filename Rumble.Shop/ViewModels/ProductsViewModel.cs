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
			InitItems ();
			InitFeaturedItems ();
		}

		public void RefreshCommand()
		{
			OnPropertyChanged("TotalPrice");
			OnPropertyChanged("AddedItems");
		}

		public double TotalPrice
		{
			get {
				return AddedItems.Sum (i => i.PriceD);
			}
		}

		public IEnumerable<ProductViewModel> AddedItems
		{
			get {
				return Items.Where (i => i.Added).
					Concat(FeaturedItems.Where(i => i.Added)).
					Concat(MainItems.Where(i => i.Added));
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
					Name = "iPhone 5C",
					ImageUrl = "http://www.adweek.com/files/imagecache/node-blog/blogs/apple-colorrules-hed-2013.jpg",
					Price = "649.00",
					PriceD = 649,
				},
				new ProductViewModel {
					Name = "ViewSonic VA24",
					ImageUrl = "http://www.letsgodigital.org/images/artikelen/260/viewsonic-lcd-monitors.jpg",
					Price = "249.00",
					PriceD = 249,
					Details = @"ViewSonic’s VA2037m-LED features an LED backlight 20” (19.5” viewable) widescreen monitor with integrated speakers."
				},
				new ProductViewModel {
					Name = "Apple Mouse",
					ImageUrl = "http://store.storeimages.cdn-apple.com/4391/as-images.apple.com/is/image/AppleInc/aos/published/images/M/B8/MB829/MB829?wid=400&hei=400&fmt=jpeg&qlt=95&op_sharpen=0&resMode=bicub&op_usm=0.5,0.5,0,0&iccEmbed=0&layer=comp&.v=1400690353435",
					Price = "79.99",
					PriceD = 79.99,
					Details = @"Magic Mouse — with its low-profile design and seamless top shell — is so sleek and dramatically different, it brings a whole new feel to the way you get around on your Mac. "
				},
				new ProductViewModel {
					Name = "iPad Mini",
					ImageUrl = "http://i-cdn.phonearena.com/images/articles/98755-image/Apple-iPad-mini-2.jpg",
					Price = "349.00",
					PriceD = 349,
					Details = @"We made iPad mini small. We made it fast. We made it incredibly capable. And now we made it even better. With advancements like Touch ID and iOS 8, and a new gold ﬁnish, there’s even more about iPad mini to love."
				},
				new ProductViewModel {
					Name = "iPhone 6",
					ImageUrl = "http://media.meltystyle.fr/article-1608534-ajust_930-f1373442397/l-iphone-6-mini-tournera-sous-ios-8-d-apres.jpg",
					Price = "649.99",
					PriceD = 649.99,
					Details = "iPhone 6 isn’t simply bigger — it’s better in every way. Larger, yet dramatically thinner. More powerful, but remarkably power efficient. With a smooth metal surface that seamlessly meets the new Retina HD display. "
				},
				new ProductViewModel {
					Name = "MacBook Pro",
					ImageUrl = "http://www1.pcmag.com/media/images/304604-apple-macbook-pro-13-inch-retina-display-top.jpg",
					Price = "1399.99",
					PriceD = 1399.99,
					Details = "A groundbreaking Retina display. All-flash architecture. Fourth-generation Intel processors. Remarkably thin and light 13‑inch and 15‑inch designs. Together, these features take the notebook to a place it’s never been."
				},
				new ProductViewModel {
					Name = "iWatch",
					ImageUrl = "http://s1.ibtimes.com/sites/www.ibtimes.com/files/styles/v2_article_large/public/2014/08/30/iwatch.jpg?itok=kDASCaDK",
					Price = "TBA",
					Details = "High-quality watches have long been defined by their ability to keep unfailingly accurate time, and Apple Watch is no exception. It uses multiple technologies in conjunction with your iPhone to keep time within 50 milliseconds of the definitive global time standard. And it can automatically adjust to the local time when you travel. Apple Watch also presents time in a more meaningful, personal context by sending you notifications and alerts relevant to your life and schedule."
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
			FeaturedItems = await LoadItems ("http://www.rakuten.com/ct/rss/todaysdeals.xml");
		}

		private async Task InitItems()
		{
			Items = await LoadItems ("http://www.rakuten.com/ct/rss/electronics.xml");
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

