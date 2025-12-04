using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantOrderManagement.Models
{
    public class RegisterRequest
    {
        public string name { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int restaurant_id { get; set; }
        public int roles_id { get; set; }    
    }
}
