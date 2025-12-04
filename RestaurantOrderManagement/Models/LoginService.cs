using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantOrderManagement.Models
{
    public class LoginService
    {
        public SessionData CurrentSession { get; set; } = new SessionData();
    }
}
