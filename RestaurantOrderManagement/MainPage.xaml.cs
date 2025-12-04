using RestaurantOrderManagement.Models;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestaurantOrderManagement
{
    public partial class MainPage : ContentPage
    {
        private readonly LoginService _loginService;

        public MainPage(LoginService loginService)
        {
            InitializeComponent();
            _loginService = loginService;
        }

        private async void onAuthClicked(object sender, EventArgs e)
        {
            try
            {
                var client = new HttpClient();
                var loginRequest = new LoginRequest()
                {
                    username = Username.Text,
                    password = Password.Text,
                };
                var json = JsonSerializer.Serialize(loginRequest);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://127.0.0.1:8000/auth/login", content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    var respons = JsonSerializer.Deserialize<LoginResponse>(result);
                    if (respons is not null && respons.role is "restaurant_manager" )
                    {
                        Username.Text = string.Empty;
                        Password.Text = string.Empty;
                        _loginService.CurrentSession.AccessToken = respons.access_token;
                        _loginService.CurrentSession.RestoranId = respons.restaurant_id;
                        await Shell.Current.GoToAsync("///RestaurantManager");
                    }
                    if (respons is not null && respons.role is "waiter")
                    {
                        Username.Text = string.Empty;
                        Password.Text = string.Empty;
                        _loginService.CurrentSession.AccessToken = respons.access_token;
                        _loginService.CurrentSession.RestoranId = respons.restaurant_id;
                        await Shell.Current.GoToAsync("///WaiterPanel");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlertAsync("Error", ex.Message, "OK");



            }
        }
    }
}
