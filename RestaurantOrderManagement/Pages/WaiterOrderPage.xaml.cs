using RestaurantOrderManagement.Models;
using System.Net.Http.Json;
using static RestaurantOrderManagement.Pages.AddOrders;

namespace RestaurantOrderManagement.Pages;

public partial class WaiterOrderPage : ContentPage
{
    private readonly LoginService loginService;
    public WaiterOrderPage(LoginService loginService)
    {
        InitializeComponent();
        this.loginService = loginService;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadAllAvailableDesks();
        await LoadAllProducts();
    }
    private async void OnBackButtonClicked (object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///WaiterPanel");
    }
    private async Task LoadAllProducts()
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginService.CurrentSession.AccessToken);

            var products = await client.GetFromJsonAsync<List<Models.Product>>($"http://127.0.0.1:8000/products/");

            if (products is null)
            {
                throw new Exception("ürün datalarý çekilemedi!");
            }

            var orderProducts = products.Select(p => Models.OrderProduct.FromProduct(p)).ToList();

            ProductsCollectionView.ItemsSource = orderProducts;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Hata", ex.Message, "OK");
        }
    }
    private async Task LoadAllAvailableDesks()
    {
        try
        {
            using var client = new HttpClient();
            var response = await client.GetFromJsonAsync<List<Models.OrderDesks>>($"http://127.0.0.1:8000/tables/available?restaurant_id={loginService.CurrentSession.RestoranId}");
            if (response is null)
            {
                throw new Exception("masa datalarý çekilemedi!");
            }
            TablePicker.ItemsSource = response;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
    private void OnIncreaseQuantity(object sender, TappedEventArgs e)
    {
        var element = (VisualElement)sender;
        var product = (Models.OrderProduct)element.BindingContext;

        if (product != null)
        {
            product.Quantity++;
        }
    }

    private void OnDecreaseQuantity(object sender, TappedEventArgs e)
    {
        var element = (VisualElement)sender;
        var product = (Models.OrderProduct)element.BindingContext;

        if (product != null && product.Quantity > 0)
        {
            product.Quantity--;
        }
    }
    private async void OnSaveOrderClicked(object sender, EventArgs e)
    {
        var selectedTable = (Models.OrderDesks)TablePicker.SelectedItem;
        if (selectedTable == null)
        {
            await DisplayAlertAsync("Uyarý", "Lütfen bir masa seçiniz!", "Tamam");
            return;
        }

        var allProducts = (List<Models.OrderProduct>)ProductsCollectionView.ItemsSource;
        var selectedItems = allProducts.Where(p => p.Quantity > 0).ToList();

        if (!selectedItems.Any())
        {
            await DisplayAlertAsync("Uyarý", "Sepette hiç ürün yok!", "Tamam");
            return;
        }

        var orderRequest = new CreateOrderRequest
        {
            table_id = selectedTable.id,
            payment_type_id = 1,
            items = selectedItems.Select(p => new OrderItemRequest
            {
                product_id = p.id,
                quantity = p.Quantity
            }).ToList()
        };

        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginService.CurrentSession.AccessToken);

            var response = await client.PostAsJsonAsync("http://127.0.0.1:8000/orders/", orderRequest);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlertAsync("Baþarýlý", "Sipariþ mutfaða iletildi!", "Tamam");

                await Shell.Current.GoToAsync("///WaiterPanel");
            }
            else
            {
                var errorMsg = await response.Content.ReadAsStringAsync();
                await DisplayAlertAsync("Hata", $"Sipariþ oluþturulamadý: {errorMsg}", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Hata", $"Baðlantý hatasý: {ex.Message}", "Tamam");
        }
    }
    public class CreateOrderRequest
    {
        public int table_id { get; set; }
        public int payment_type_id { get; set; }
        public List<OrderItemRequest> items { get; set; }
    }

    public class OrderItemRequest
    {
        public int product_id { get; set; }
        public int quantity { get; set; }
    }



}