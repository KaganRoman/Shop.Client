using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Collections;
using Rumble.Shop.Models;

namespace Rumble.Shop
{
	public partial class MainCanvasPage : TabbedPage
	{
		public MainCanvasPage ()
		{
			BindingContext = new ProductsViewModel (SelectItem);

			InitializeComponent ();

			ItemsSource = new List<string> {"Home", "Featured", "Products", "My Bag", "My Orders" };
		}

		protected override Page CreateDefault (object item)
		{
			var icon = "";
			DataTemplate template = null;
			if (item == "Home")
			{
				icon = "featured.png";
				template = Resources["HomeTemplate"] as DataTemplate;
			}
			if (item == "Featured") {
				icon = "featured.png";
				template = Resources["FeaturedTemplate"] as DataTemplate;
			}
			if (item == "Products") {
				icon = "products.png";
				template = Resources["ProductsTemplate"] as DataTemplate;
			}
			if (item == "My Bag") {
				icon = "bag.png";
				template = Resources["BagTemplate"] as DataTemplate;
			}
			if (item == "My Orders") {
				icon = "orders.png";
				template = Resources["OrdersTemplate"] as DataTemplate;
			}

			var view = template != null ? template.CreateContent () as View : new ContentView ();
			view.BindingContext = BindingContext;
			return new ContentPage { Content = view, Title = (string)item, Icon = icon};
		}

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			(BindingContext as ProductsViewModel).RefreshCommand();
		}

		private void ContinueShopping_Clicked(object sender, EventArgs e)
		{
			SelectedItem = (ItemsSource as IList)[1];
		}

		private void ViewOffer_Clicked(object sender, EventArgs e)
		{
			OpenFeatured ();
		}

		private void Featured_Clicked(object sender, EventArgs e)
		{
			var view = sender as View;
			if (null == view)
				return;
			var item = view.BindingContext;
			SelectItem (item);
		}

		private async void SelectItem(object item)
		{
			var productPage = new ProductPage () { BindingContext = item };
			await Navigation.PushAsync (productPage);
		}

		private async void OpenFeatured()
		{
			var featuredPage = new FeaturedProductsPage () { 
				BindingContext = new FeaturedProductsViewModel ((BindingContext as ProductsViewModel).FeaturedItems)
			};
			await Navigation.PushAsync (featuredPage);
		}

		private async void Checkout_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync (new CheckoutPage());
		}

		private void Category_Clicked(object sender, EventArgs e)
		{
			(BindingContext as ProductsViewModel).CurrentCategory = (sender as BindableObject).BindingContext as Category;
            CurrentPage = Children[2];
		}
	}
}

