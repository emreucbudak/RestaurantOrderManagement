using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace RestaurantOrderManagement.Models
{
    public class OrderResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("total_price")]
        public double TotalPrice { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("table_name")]
        public string TableName { get; set; }

        [JsonPropertyName("status_name")]
        public string StatusName { get; set; }

        [JsonPropertyName("payment_type_name")]
        public string PaymentTypeName { get; set; }

        public bool IsPreparedButtonVisible => StatusName == "Hazırlanıyor";

        public bool IsDeliveredButtonVisible => StatusName == "Hazırlandı";

        public bool IsCancelButtonVisible => StatusName != "Teslim Edildi" && StatusName != "İptal Edildi";

        public bool IsCompleted => !IsCancelButtonVisible;
    }
}
