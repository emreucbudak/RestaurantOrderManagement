using RestaurantOrderManagement.Models;
using System.Buffers.Text;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace RestaurantOrderManagement.Pages;

public partial class ProductCategoryManagementPage : ContentPage
{
    private readonly LoginService loginService;
    public ObservableCollection<ProductCategory> productCategories { get; set; } = new ObservableCollection<ProductCategory>();

    public ProductCategoryManagementPage(LoginService loginService)
    {
        InitializeComponent();
        this.loginService = loginService;
        BindingContext = this;
        RefreshListFromApi();
    }
    private async void OnAddCategoryClicked(object sender, EventArgs e)
    {
        string categoryNameEntry = CategoryNameEntry.Text;
        if (string.IsNullOrWhiteSpace(categoryNameEntry))
        {
            await DisplayAlertAsync("Uyar�", "L�tfen bir kategori ad� girin.", "Tamam");
            return;
        }
        try
        {
            var client = new HttpClient();
            string token = loginService.CurrentSession.AccessToken;
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var categoryData = new { name = categoryNameEntry };
            string json = JsonSerializer.Serialize(categoryData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("http://127.0.0.1:8000/categories/", content);
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlertAsync("Ba�ar�l�", "Kategori ba�ar�yla eklendi.", "Tamam");
                CategoryNameEntry.Text = string.Empty;
                await RefreshListFromApi();
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                await DisplayAlertAsync("Hata", $"Kategori eklenemedi. Sunucu yan�t�: {errorResponse}", "Tamam");
            }


        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
    private async Task RefreshListFromApi()
    {
        try
        {

            using var client = new HttpClient();
            var waiterstList = await client.GetFromJsonAsync<List<ProductCategory>>("http://127.0.0.1:8000/categories/");

            if (waiterstList != null)
            {


                productCategories.Clear();

                foreach (var item in waiterstList)
                {
                    productCategories.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Hata", $"Liste �ekilemedi {ex.Message}", "OK");
        }
    }
    private async void OnDeleteCategoryClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var category = button?.BindingContext as ProductCategory;
        if (category == null)
            return;
        string categoryIdToDelete = category.name;
        try
        {
            var client = new HttpClient();
            string token = loginService.CurrentSession.AccessToken;
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await client.DeleteAsync($"http://127.0.0.1:8000/categories/{categoryIdToDelete}");
            if (response.IsSuccessStatusCode)
            {
                await DisplayAlertAsync("Ba�ar�l�", "Kategori ba�ar�yla silindi.", "Tamam");
                await RefreshListFromApi();
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                await DisplayAlertAsync("Hata", $"Kategori silinemedi. Sunucu yan�t�: {errorResponse}", "Tamam");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Hata", ex.Message, "Tamam");
        }
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///ProductManagement");
    }
}