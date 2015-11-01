using System;

using Xamarin.Forms;

namespace Rumble.Shop
{
	public class App : Application
	{
		public App ()
		{
			ProductsService.Products.Init().Wait();

			// The root page of your application
			var navPage = new NavigationPage (new MainCanvasPage());
			navPage.BarTextColor = Color.White;
			navPage.BarBackgroundColor = Color.FromHex ("#4780ED");

			MainPage = navPage;
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}

