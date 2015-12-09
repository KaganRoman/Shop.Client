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

			ItemsSource = new List<string> {"Home", "Featured", "Products", "My Cart", "My Orders" };
		}

		protected override Page CreateDefault (object item)
		{
			var icon = "";
			DataTemplate template = null;
			if (item == "Home")
			{
				icon = "home.png";
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
			if (item == "My Cart") {
				icon = "cart.png";
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

		private void ToolbarItem_Clicked(object sender, EventArgs e)
		{
			var item = sender as ToolbarItem;
			if(item == null) return;

			if (item.Text == "Home")
				CurrentPage = Children[0];
			if (item.Text == "Cart")
				CurrentPage = Children[3];
		}

		private void Picker_SelectedIndexChanged(object sender, EventArgs e)
		{
			var picker = sender as Picker;
			if (null == picker)
				return;
			var product = picker.BindingContext as ProductViewModel;
			if (product == null)
				return;

			// Ugly hack to prevent endless loop
			var total = (BindingContext as ProductsViewModel).TotalPrice;
			var page = CurrentPage as ContentPage;
			var totalEl = page.Content.FindByName<Label> ("_totalLbl");
			if(totalEl != null && string.Format("{0:N2}",total) != totalEl.Text)
				(BindingContext as ProductsViewModel).RefreshCommand();
		}
	}
}

