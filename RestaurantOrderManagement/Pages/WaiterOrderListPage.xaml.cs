using RestaurantOrderManagement.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace RestaurantOrderManagement.Pages;

public partial class WaiterOrderListPage : ContentPage
{
    private readonly LoginService loginService;
    private ObservableCollection<OrderResponse> ordersList;

    public WaiterOrderListPage(LoginService loginService)
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
        await Shell.Current.GoToAsync("///WaiterPanel");
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
            await DisplayAlertAsync("Hata", $"Sipariþler yüklenemedi: {ex.Message}", "Tamam");
        }
    }
}