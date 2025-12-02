using System.Threading.Tasks;

namespace RestaurantOrderManagement
{
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();
        }

        private async void onAuthClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("///RestaurantManager");
        }
    }
}
