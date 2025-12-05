using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace RestaurantOrderManagement.Models
{
    public class OrderProduct : INotifyPropertyChanged
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal price { get; set; }

        private int _quantity = 0;
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Product'tan OrderProduct'a dönüştürme
        public static OrderProduct FromProduct(Product product)
        {
            return new OrderProduct
            {
                id = product.id,
                name = product.name,
                price = product.price,
                Quantity = 0
            };
        }
    }
}
