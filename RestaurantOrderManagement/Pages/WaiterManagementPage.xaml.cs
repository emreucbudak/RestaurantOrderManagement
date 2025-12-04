using RestaurantOrderManagement.Models;
using System.Text;

namespace RestaurantOrderManagement.Pages;

public partial class WaiterManagementPage : ContentPage
{
    private readonly LoginService _login;
    private readonly int _restaurantId;



    public WaiterManagementPage(LoginService login)
    {
        InitializeComponent();
        _login = login;


        _restaurantId = _login.CurrentSession.RestoranId;
    }

    private async void OnWaiterAddedButtonClicked(object sender, EventArgs e)
    {


        try
        {
            string name = WaiterName.Text;
            string username = WaiterUsername.Text;
            string password = WaiterPassword.Text;

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlertAsync("Hata", "Lütfen tüm bilgileri doldurun", "OK");
                return;
            }


            var addWaiter = new RegisterRequest
            {
                name = name,
                username = username,
                password = password,
                restaurant_id = _restaurantId,
                roles_id = 2
            };


            using var client = new HttpClient();
            var json = System.Text.Json.JsonSerializer.Serialize(addWaiter);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            var response = await client.PostAsync("http://127.0.0.1:8000/auth/register/user", content);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlertAsync("Baþarýlý", "Garson baþarýyla eklendi", "OK");


                WaiterName.Text = "";
                WaiterUsername.Text = "";
                WaiterPassword.Text = "";
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Hata", $"Garson eklenemedi {ex.Message}", "OK");
        }
    }
}
