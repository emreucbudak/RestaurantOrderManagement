using RestaurantOrderManagement.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text;


namespace RestaurantOrderManagement.Pages;

public partial class WaiterManagementPage : ContentPage
{
    private readonly LoginService _login;
    private readonly int _restaurantId;
    public ObservableCollection<Waiter> Waiters { get; set; } = new ObservableCollection<Waiter>();



    public WaiterManagementPage(LoginService login)
    {
        InitializeComponent();
        _login = login;
        WaitersList.ItemsSource = Waiters;

        RefreshListFromApi();
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
                await RefreshListFromApi();
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Hata", $"Garson eklenemedi {ex.Message}", "OK");
        }
    }
    private async Task RefreshListFromApi()
    {
        try
        {
           
            using var client = new HttpClient();
            var waiterstList = await client.GetFromJsonAsync<List<Waiter>>("http://127.0.0.1:8000/auth/waiters");

            if (waiterstList != null)
            {
                

                Waiters.Clear(); 

                foreach (var item in waiterstList)
                {
                    Waiters.Add(item); 
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Hata", $"Liste çekilemedi {ex.Message}", "OK");
        }
    }
}
