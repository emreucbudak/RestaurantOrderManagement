namespace RestaurantOrderManagement.Pages;

public partial class ProductManagement : ContentPage
{
	public ProductManagement()
	{
		InitializeComponent();
	}
	private async void OnProductManagementPageClicked (object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///ProductManagementPage");
	}
	private async void OnProductCategoryManagementPageClicked (object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///ProductCategoryManagementPage");
    }
    private async void OnBackButtonClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("///RestaurantManager");
    }
}