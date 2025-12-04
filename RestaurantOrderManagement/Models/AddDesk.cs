using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantOrderManagement.Models
{
    public class AddDesk
    {
        public string name { get; set; }
        public bool is_available { get; set; }
        public int restaurant_id { get; set; }

    }
}
