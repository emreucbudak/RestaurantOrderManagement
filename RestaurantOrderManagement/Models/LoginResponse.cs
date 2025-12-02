using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantOrderManagement.Models
{
    public class LoginResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string username { get; set; }
        public string role { get; set; }
        public int restaurant_id { get; set; }
    }

}
