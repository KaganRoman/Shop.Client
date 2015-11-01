using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rumble.Shop.Models
{
	public class Order
	{
		public string Name { get; set; }
		public string ImageUrl { get; set; }
		public string Price { get; set; }
		public double PriceD { get; set; }
		public string Status { get; set; }
	}
}
