using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Threading.Tasks;

namespace Rumble.Shop
{
	public partial class ProductPage : ContentPage
	{
		private ProductViewModel Product { get {  return BindingContext as ProductViewModel; } }

		public ProductPage ()
		{
			InitializeComponent ();

			var tapGestureRecognizer = new TapGestureRecognizer ();
			tapGestureRecognizer.Tapped += async (sender, e) =>  await AddToCart();
			_addToBag.GestureRecognizers.Add (tapGestureRecognizer);

			_addCart.Clicked += async (object sender, EventArgs e) => await AddToCart();
		}

		private async Task AddToCart()
		{
			Product.Added = !Product.Added;
			SelectLabelProperties();
			_addToBag.Opacity = 0.5;
			await _addToBag.FadeTo(1, 1000);
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();
			SelectLabelProperties ();
		}

		private void SelectLabelProperties()
		{
			//_addToBag.BackgroundColor = Product.Added ? Color.FromHex("#EBEBEB") : Color.FromHex("#4780ED");
			//_addToBagLabel.TextColor = Product.Added ? Color.Black : Color.White;
			if(Product != null)
				_addToBagLabel.Text = Product.Added ? "REMOVE FROM BAG" : "ADD TO BAG";
		}
	}
}

