namespace RestaurantOrderManagement.Pages;

public partial class ProductManagement : ContentPage
{
	public ProductManagement()
	{
		InitializeComponent();
	}

    private async void OnProductManagementClicked (object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///ProductManagementPage");
	}
	private async void OnProductCategoryManagementClicked (object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("///ProductCategoryManagementPage");
	}
}