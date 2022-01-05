using NotasISPCAN.Features.Service;
using Splat;
using Xamarin.Essentials;

namespace NotasISPCAN.ViewModel
{
    public class LoadingViewModel
    {
        private IRouting routingService;
        private IAuthenticationService AuthenticationService;
        public LoadingViewModel(IRouting routing = null, IAuthenticationService authentication = null)
        {
            routingService = routing ?? Locator.Current.GetService<IRouting>();
            AuthenticationService = authentication ?? Locator.Current.GetService<IAuthenticationService>();
        }
        public async void Init()
        {
            var isAuthenticated = await AuthenticationService.IsLogged();
            if (isAuthenticated)
            {
                string categoria = Preferences.Get("Categoria", "doesnot");
                if (categoria.ToUpper() == "ADMINISTRADOR")
                    await routingService.NavigateTo("///admin");
                else if (categoria.ToUpper() == "PROFESSOR")
                    await routingService.NavigateTo("///professor");
                else if (categoria.ToUpper() == "ESTUDANTE")
                    await routingService.NavigateTo("///main");
                else
                    return;
            }
            else
            {
                await routingService.NavigateTo("///login");
            }
        }
    }
}
