namespace RestaurantOrderManagement.Pages;

public partial class WaiterOrderPage : ContentPage
{
	public WaiterOrderPage()
	{
		InitializeComponent();
	}
	private async void OnBackButtonClicked (object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///WaiterPanel");
    }
}