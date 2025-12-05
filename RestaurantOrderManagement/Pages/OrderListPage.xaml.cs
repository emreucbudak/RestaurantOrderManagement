namespace RestaurantOrderManagement.Pages;

public partial class OrderListPage : ContentPage
{
	public OrderListPage()
	{
		InitializeComponent();
	}
	private async void OnBackButtonClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///RestaurantManager");
    }
}