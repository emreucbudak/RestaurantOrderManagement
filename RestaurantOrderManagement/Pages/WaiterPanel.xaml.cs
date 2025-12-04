namespace RestaurantOrderManagement.Pages;

public partial class WaiterPanel : ContentPage
{
	public WaiterPanel()
	{
		InitializeComponent();
	}
	private async void OnLogOutClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///MainPage");
    }
}