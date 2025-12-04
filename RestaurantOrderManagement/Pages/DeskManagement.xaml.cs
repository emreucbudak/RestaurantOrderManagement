using RestaurantOrderManagement.Models;
using System.Buffers.Text;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RestaurantOrderManagement.Pages;

public partial class DeskManagement : ContentPage
{
	private LoginService loginService;
	public ObservableCollection<Desks> Waiters { get; set; } = new ObservableCollection<Desks>();
	public DeskManagement(LoginService loginService)
	{
		InitializeComponent();
		this.loginService = loginService;
		BindingContext = this;
		RefreshListFromApi();
	}
	private async void OnBackButtonClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///RestaurantManager");
	}
	private async void OnAddDeskClicked(object sender, EventArgs e)
	{
		string deskName = DeskName.Text;
		try
		{
			using var client = new HttpClient();
			var newDesk = new AddDesk()
			{
				name = deskName,
				is_available = true,
				restaurant_id = loginService.CurrentSession.RestoranId
			};
			string accessToken = loginService.CurrentSession.AccessToken;
			client.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("Bearer", accessToken);
			var response = await client.PostAsJsonAsync("http://127.0.0.1:8000/tables/", newDesk);
			if (response.IsSuccessStatusCode)
			{
				await DisplayAlertAsync("Baþarýlý", "Masa eklendi", "OK");
				DeskName.Text = string.Empty;
				await RefreshListFromApi();

			}
			else
			{
				await DisplayAlertAsync("Hata", "Masa eklenemedi", "OK");
			}
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("Hata", $"Masa eklenemedi {ex.Message}", "OK");
		}
	}
	private async Task RefreshListFromApi()
	{
		try
		{
			using var client = new HttpClient();
			string accessToken = loginService.CurrentSession.AccessToken;
			client.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("Bearer", accessToken);
			string url = $"http://127.0.0.1:8000/tables?restaurant_id={loginService.CurrentSession.RestoranId}";

			var response = await client.GetAsync(url);
			if (response.IsSuccessStatusCode)
			{
				var desks = await response.Content.ReadFromJsonAsync<List<Desks>>();
				Waiters.Clear();
				if (desks is not null)
				{
					foreach (var desk in desks)
					{
						Waiters.Add(desk);
					}
				}
			}
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("Hata", $"Masalar yüklenemedi {ex.Message}", "OK");
		}
	}
	private async void OnDeleteDeskClicked(object sender, EventArgs e)
	{
		var button = sender as Button;
		var desk = button?.BindingContext as Desks;
		if (desk is null)
			return;
		string deskName = desk.name;
		try
		{
			using var client = new HttpClient();
			string accessToken = loginService.CurrentSession.AccessToken;
			client.DefaultRequestHeaders.Authorization =
				new AuthenticationHeaderValue("Bearer", accessToken);
			var response = await client.DeleteAsync($"http://127.0.0.1:8000/tables/{deskName}");
			if (response.IsSuccessStatusCode)
			{
				await DisplayAlertAsync("Baþarýlý", "Masa silindi", "OK");
				await RefreshListFromApi();
			}
		}
		catch (Exception ex)
		{
			await DisplayAlertAsync("Hata", $"Masa silinemedi {ex.Message}", "OK");
		}
	}
}