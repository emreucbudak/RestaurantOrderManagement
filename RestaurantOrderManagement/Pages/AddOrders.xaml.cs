namespace RestaurantOrderManagement.Pages;

public partial class AddOrders : ContentPage
{
	public AddOrders()
	{
		InitializeComponent();
	}
	private async void OnBackButtonClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///RestaurantManager");
    }
}