using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Rumble.Shop.Models;
using Xamarin.Forms;

namespace Rumble.Shop
{

	class ProductsService
	{
		public static ProductsService Products = new ProductsService();

		public List<Product> FeaturedItems { get; set; }
		public List<Product> Items { get; set; }
		public List<Product> MainItems { get; set; }
		public List<Category> Categories { get; set; }


		private Dictionary<Category, List<Product>> _categoryItems;

		public async Task Init()
		{
			_categoryItems = new Dictionary<Category, List<Product>>();
            MainItems = InitMainItems();
			Categories = InitCategories();
			FeaturedItems = await LoadItems("http://www.rakuten.com/ct/rss/todaysdeals.xml").ConfigureAwait(false);
			Items = await GetCategoryItems(Categories.First()).ConfigureAwait(false);
		}


		private List<Category> InitCategories()
		{
			return new List<Category>
			{
				new Category
				{
					Name = "ELECTRONICS",
					Url = "http://www.rakuten.com/ct/rss/electronics.xml",
					ImageUrl = "http://img1.r10.io/PIC/91227708/0/1/125/91227708.jpg"
				},
				new Category
				{
					Name = "COMPUTERS",
					Url = "http://www.rakuten.com/ct/rss/computers.xml",
					ImageUrl = "http://img1.r10.io/PIC/93102125/0/1/125/93102125.jpg"
				},
				new Category
				{
					Name = "CELL PHONES",
					Url = "http://www.rakuten.com/ct/rss/cellphones.xml",
					ImageUrl = "http://img1.r10.io/PIC/84388180/0/1/125/84388180.jpg"
				},
				new Category
				{
					Name = "TV AND HDTV",
					Url = "http://www.rakuten.com/ct/rss/tvandhdtv.xml",
					ImageUrl = "http://img1.r10.io/PIC/90760527/0/1/125/90760527.jpg"
				},
				new Category
				{
					Name = "DIGITAL CAMERAS",
					Url = "http://www.rakuten.com/ct/rss/digitalcameras.xml",
					ImageUrl = "http://img1.r10.io/PIC/87269247/0/1/125/87269247.jpg"
				},
				new Category
				{
					Name = "SOFTWARE",
					Url = "http://www.rakuten.com/ct/rss/software.xml",
					ImageUrl = "http://img1.r10.io/PIC/98708465/0/1/125/98708465.jpg"
				},
				new Category
				{
					Name = "VIDEO GAMES",
					Url = "http://www.rakuten.com/ct/rss/videogames.xml",
					ImageUrl = "http://img1.r10.io/PIC/67903091/0/1/125/67903091.jpg"
				},
			};
		}

		private List<Product> InitMainItems()
		{
			return new List<Product> {
				new Product {
					Name = "iPhone 5C",
					ImageUrl = "http://www.adweek.com/files/imagecache/node-blog/blogs/apple-colorrules-hed-2013.jpg",
					Price = "649.00",
					PriceD = 649,
				},
				new Product {
					Name = "ViewSonic VA24",
					ImageUrl = "http://www.letsgodigital.org/images/artikelen/260/viewsonic-lcd-monitors.jpg",
					Price = "249.00",
					PriceD = 249,
					Details = @"ViewSonic’s VA2037m-LED features an LED backlight 20” (19.5” viewable) widescreen monitor with integrated speakers."
				},
				new Product {
					Name = "Apple Mouse",
					ImageUrl = "http://store.storeimages.cdn-apple.com/4391/as-images.apple.com/is/image/AppleInc/aos/published/images/M/B8/MB829/MB829?wid=400&hei=400&fmt=jpeg&qlt=95&op_sharpen=0&resMode=bicub&op_usm=0.5,0.5,0,0&iccEmbed=0&layer=comp&.v=1400690353435",
					Price = "79.99",
					PriceD = 79.99,
					Details = @"Magic Mouse — with its low-profile design and seamless top shell — is so sleek and dramatically different, it brings a whole new feel to the way you get around on your Mac. "
				},
				new Product {
					Name = "iPad Mini",
					ImageUrl = "http://i-cdn.phonearena.com/images/articles/98755-image/Apple-iPad-mini-2.jpg",
					Price = "349.00",
					PriceD = 349,
					Details = @"We made iPad mini small. We made it fast. We made it incredibly capable. And now we made it even better. With advancements like Touch ID and iOS 8, and a new gold ﬁnish, there’s even more about iPad mini to love."
				},
				new Product {
					Name = "iPhone 6s",
					ImageUrl = "http://media.meltystyle.fr/article-1608534-ajust_930-f1373442397/l-iphone-6-mini-tournera-sous-ios-8-d-apres.jpg",
					Price = "649.99",
					PriceD = 649.99,
					Details = "iPhone 6s isn’t simply bigger — it’s better in every way. Larger, yet dramatically thinner. More powerful, but remarkably power efficient. With a smooth metal surface that seamlessly meets the new Retina HD display. "
				},
				new Product {
					Name = "MacBook Pro",
					ImageUrl = "http://www1.pcmag.com/media/images/304604-apple-macbook-pro-13-inch-retina-display-top.jpg",
					Price = "1399.99",
					PriceD = 1399.99,
					Details = "A groundbreaking Retina display. All-flash architecture. Fourth-generation Intel processors. Remarkably thin and light 13‑inch and 15‑inch designs. Together, these features take the notebook to a place it’s never been."
				},
				new Product {
					Name = "Apple Watch 2",
					ImageUrl = "http://s1.ibtimes.com/sites/www.ibtimes.com/files/styles/v2_article_large/public/2014/08/30/iwatch.jpg?itok=kDASCaDK",
					Price = "TBA",
					Details = "High-quality watches have long been defined by their ability to keep unfailingly accurate time, and Apple Watch is no exception. It uses multiple technologies in conjunction with your iPhone to keep time within 50 milliseconds of the definitive global time standard. And it can automatically adjust to the local time when you travel. Apple Watch also presents time in a more meaningful, personal context by sending you notifications and alerts relevant to your life and schedule."
				},
			};
		}

		private async Task<List<Product>> LoadItems(string url)
		{
			var items = new List<Product>();
			try
			{
				using (var clientHandler = new HttpClientHandler())
				using (var httpClient = new HttpClient(clientHandler))
				{
					var response = await httpClient.GetAsync(url, new CancellationToken()).ConfigureAwait(false);
					var rss = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
					XDocument doc = XDocument.Parse(rss);
					foreach (var item in doc.Element("rss").Element("channel").Elements("item"))
					{
						var product = new Product
						{
							Name = item.Element("title").Value
						};
						foreach (var n in item.Elements())
						{
							if (n.Name.ToString() == "{http://www.rakuten.com/rss/module/productV2/}price")
								product.Price = n.Value;
							if (n.Name.ToString() == "{http://www.rakuten.com/rss/module/productV2/}categoryName")
								product.Category = n.Value;
							if (n.Name.ToString() == "{http://www.rakuten.com/rss/module/productV2/}description")
								product.Details = n.Value;
							if (n.Name.ToString() == "{http://www.rakuten.com/rss/module/productV2/}imagelink")
							{
								product.ImageUrl = n.Value;
								product.Rating = n.Value.Length % 5 + 1;
							}
						}
						if (product.Name.Length > 70)
							product.Name = product.Name.Substring(0, 50 + product.Name.Substring(50).IndexOf(' '));
						product.Name = product.Name.Trim();
						if (product.Name.EndsWith(","))
							product.Name = product.Name.Substring(0, product.Name.Length - 1);
						if (product.Details.Length > 500)
						{
							var ind = product.Details.IndexOf("\n");
							if (ind > 0 && ind < 500)
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
			catch (Exception e)
			{
			}
			return new List<Product>();
		}

		public async Task<List<Product>> GetCategoryItems(Category category)
		{
			if(category == null) return new List<Product>();

			if (_categoryItems.ContainsKey(category))
				return _categoryItems[category];

			var categoryItems = await LoadItems(category.Url).ConfigureAwait(false);
			lock (_categoryItems)
			{
				_categoryItems[category] = categoryItems;
			}
			return categoryItems;
		}
	}
}
