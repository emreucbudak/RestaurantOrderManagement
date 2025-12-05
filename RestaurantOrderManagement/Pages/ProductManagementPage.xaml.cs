using RestaurantOrderManagement.Models;
using System.Collections.ObjectModel;
using System.Net.Http.Json;

namespace RestaurantOrderManagement.Pages;

public partial class ProductManagementPage : ContentPage
{
    private readonly LoginService loginService;
    public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();
    public ProductManagementPage(LoginService loginService)
    {
        InitializeComponent();
        BindingContext = this;
        this.loginService = loginService;
        LoadAllProducts();
        LoadCategoriesData();
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///ProductManagement");
    }
    private async Task LoadCategoriesData()
    {
        try
        {
            using var client = new HttpClient();
            var waiterstList = await client.GetFromJsonAsync<List<ProductCategory>>("http://127.0.0.1:8000/categories/");
            if (waiterstList is null)
            {
                throw new Exception("serverdan data ?ekilemedi !");
            }
            CategoryPicker.ItemsSource = waiterstList;


        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
    private async void OnProductAddedButtonClicked(object sender, EventArgs e)
    {
        var selectedCategory = CategoryPicker.SelectedItem as ProductCategory;
        if (selectedCategory is null)
        {
            await DisplayAlertAsync("Do?rulama Hatas?", "L?tfen Kategori Se?", "OK");
            return;
        }
        int categoryId = selectedCategory.id;
        if (int.TryParse(ProductPriceEntry.Text, out int convertedPrice))
        {
            var newProduct = new AddProduct
            {
                name = ProductNameEntry.Text,
                category_id = categoryId,
                price = convertedPrice,
                restaurant_id = loginService.CurrentSession.RestoranId
            };
            try
            {
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginService.CurrentSession.AccessToken);
                var response = await client.PostAsJsonAsync("http://127.0.0.1:8000/products/", newProduct);
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlertAsync("Ba?ar?l?", "?r?n ba?ar?yla eklendi.", "Tamam");
                    ProductNameEntry.Text = string.Empty;
                    ProductPriceEntry.Text = string.Empty;
                    CategoryPicker.SelectedItem = null;
                    await LoadAllProducts();

                }

            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Hata", ex.Message, "Tamam");

            }
        }
        else
        {
            await DisplayAlertAsync("Hata", "L?tfen ge?erli bir fiyat giriniz.", "Tamam");
            return;
        }

    }
    private async Task LoadAllProducts()
    {
        try
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginService.CurrentSession.AccessToken);
            var productsList = await client.GetFromJsonAsync<List<Product>>("http://127.0.0.1:8000/products/");
            if (productsList is null)
            {
                throw new Exception("serverdan data ?ekilemedi !");
            }
            Products.Clear();
            foreach (var product in productsList)
            {
                Products.Add(product);
            }
            ;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
    private async void OnDeleteProductClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var productToDelete = button?.CommandParameter as Product;
        if (productToDelete is null)
        {
            await DisplayAlertAsync("Hata", "Silinecek ?r?n bilgisi al?namad?.", "Tamam");
            return;
        }
        try
        {
            int deleteProductId = productToDelete.id;
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", loginService.CurrentSession.AccessToken);
            var response = await client.DeleteAsync($"http://127.0.0.1:8000/products/{deleteProductId}");
            if (response.IsSuccessStatusCode)
            {
                await LoadAllProducts();
                await DisplayAlertAsync("Ba?ar?l?", "?r?n ba?ar?yla silindi.", "Tamam");
            }
            else
            {
                await DisplayAlertAsync("Hata", "?r?n silinirken bir hata olu?tu.", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Hata", ex.Message, "Tamam");
        }
    }
}