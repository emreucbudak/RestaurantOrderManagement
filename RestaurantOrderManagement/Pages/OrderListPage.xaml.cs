using RestaurantOrderManagement.Models;
using System.Net.Http.Json;
using System.Collections.ObjectModel;

namespace RestaurantOrderManagement.Pages;

public partial class OrderListPage : ContentPage
{
    private readonly LoginService loginService;
    private ObservableCollection<OrderResponse> ordersList;

    public OrderListPage(LoginService loginService)
    {
        this.loginService = loginService;
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadOrders();
    }

    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///RestaurantManager");
    }

    private async Task LoadOrders()
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginService.CurrentSession.AccessToken);

            var response = await client.GetFromJsonAsync<List<OrderResponse>>("http://127.0.0.1:8000/orders/getorder");

            if (response != null)
            {
                ordersList = new ObservableCollection<OrderResponse>(response);
                OrdersCollectionView.ItemsSource = ordersList;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Hata", $"Sipariþler yüklenemedi: {ex}", "Tamam");
        }
    }

    private async void OnPreparedClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var order = button?.CommandParameter as OrderResponse;
        if (order != null)
        {
            await UpdateOrderStatus(order.Id, "Hazýrlandý");
        }
    }

    private async void OnDeliveredClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var order = button?.CommandParameter as OrderResponse;
        if (order != null)
        {
            await UpdateOrderStatus(order.Id, "Teslim Edildi");
        }
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var order = button?.CommandParameter as OrderResponse;

        if (order != null)
        {
            bool answer = await DisplayAlertAsync("Onay", "Sipariþi iptal etmek istediðinize emin misiniz?", "Evet", "Hayýr");
            if (answer)
            {
                await UpdateOrderStatus(order.Id, "Ýptal Edildi");
            }
        }
    }

    private async Task UpdateOrderStatus(int orderId, string newStatus)
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginService.CurrentSession.AccessToken);

            var url = $"http://127.0.0.1:8000/orders/{orderId}?status={newStatus}";

            var response = await client.PutAsync(url, null);

            if (response.IsSuccessStatusCode)
            {
                await LoadOrders();
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                await DisplayAlertAsync("Hata", $"Güncelleme baþarýsýz: {error}", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Hata", $"Baðlantý hatasý: {ex.Message}", "Tamam");
        }
    }
}