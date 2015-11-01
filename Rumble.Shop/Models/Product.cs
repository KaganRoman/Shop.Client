using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rumble.Shop.Models
{
	public class Product
	{
		public string Name { get; set; }
		public string ImageUrl { get; set; }
		public string Price { get; set; }
		public double PriceD { get; set; }
		public int Rating { get; set; }
		public string Category { get; set; }
		public string Details { get; set; }
		public bool Added { get; set; }
    }
}
