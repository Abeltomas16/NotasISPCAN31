using NotasISPCAN.Features.Service;
using Xamarin.Forms;

namespace NotasISPCAN.ViewModel
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            VerificaaLogin();
        }
        private async void VerificaaLogin()
        {
            await Shell.Current.GoToAsync("//LoginView");
        }
    }
}
