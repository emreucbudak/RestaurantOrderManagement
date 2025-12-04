using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantOrderManagement.Models
{
    public class AddProduct
    {
        public string name { get; set; }
        public decimal price { get; set; }
        public int category_id { get; set; }
        public int restaurant_id { get; set; }
    }
}
