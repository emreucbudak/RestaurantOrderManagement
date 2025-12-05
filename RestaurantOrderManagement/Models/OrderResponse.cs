using System.Text.Json.Serialization;

namespace RestaurantOrderManagement.Models;

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

    public bool IsPreparedButtonVisible
    {
        get
        {
            if (string.IsNullOrEmpty(StatusName)) return false;
            return StatusName.ToLower().Contains("hazırla");
        }
    }

    public bool IsDeliveredButtonVisible
    {
        get
        {
            if (string.IsNullOrEmpty(StatusName)) return false;
            return StatusName.ToLower().Contains("hazırlandı");
        }
    }

    public bool IsCancelButtonVisible
    {
        get
        {
            if (string.IsNullOrEmpty(StatusName)) return true;

            var s = StatusName.ToLower();
            return !s.Contains("teslim") && !s.Contains("ptal");
        }
    }

    public bool IsCompleted => !IsCancelButtonVisible;
}