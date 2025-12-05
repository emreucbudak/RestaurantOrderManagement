namespace RestaurantOrderManagement.Pages;

public partial class WaiterOrderListPage : ContentPage
{
	public WaiterOrderListPage()
	{
		InitializeComponent();
	}
	private async void OnBackButtonClicked (object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///WaiterPanel");
    }
}