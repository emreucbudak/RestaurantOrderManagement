namespace RestaurantOrderManagement.Pages;

public partial class RestaurantManagerPanel : ContentPage
{
	public RestaurantManagerPanel()
	{
		InitializeComponent();
	}
	private async void OnCikisClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///MainPage");
	}
	private async void OnAddOrdersClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//AddOrders");
    }
	private async void OnOrderListClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//OrderList");
    }
	private async void  OnWaiterManagementClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//WaiterManagement");
    }
	private async void OnDeskManagementClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//DeskManagement");
    }
	private async void OnProductManagementClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//ProductManagement");
    }

}