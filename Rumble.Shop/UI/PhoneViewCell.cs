using System;
using Xamarin.Forms;
using System.Windows.Input;

namespace Rumble.Shop.UI
{
	// iOS currently can't measure height of the listview cell 
	// http://forums.xamarin.com/discussion/18400/how-to-set-variable-height-in-a-listview
	public class PhoneViewCell : ViewCell
	{
		public static readonly BindableProperty ClickCommandProperty =
			BindableProperty.Create<PhoneViewCell,ICommand> (
				p => p.ClickCommand, null);

		public ICommand ClickCommand {
			get { return (ICommand)GetValue (ClickCommandProperty); }
			set { SetValue (ClickCommandProperty, value); }
		}

		public static readonly BindableProperty CellHeightProperty =
			BindableProperty.Create<PhoneViewCell,int> (p => p.CellHeight, 0);

		public int CellHeight {
			get { return (int)GetValue (CellHeightProperty); }
			set { SetValue (CellHeightProperty, value); }
		}

		protected override void OnTapped()
		{
			if (ClickCommand != null)
				ClickCommand.Execute (BindingContext);
		}

		protected override void OnBindingContextChanged ()
		{
			base.OnBindingContextChanged ();

			//if (Device.OS != TargetPlatform.iOS)  // don't bother on the other platforms
			//	return;

			Height = CellHeight;
		}
	}
}

