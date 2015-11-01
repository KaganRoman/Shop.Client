using System;
using Xamarin.Forms;

namespace Rumble.Shop.UI
{
	public class FullWidthImage : Image
	{
		protected override SizeRequest OnSizeRequest(double widthConstraint, double heightConstraint)
		{
			return new SizeRequest(new Size(widthConstraint, heightConstraint));
		}
	}
}

