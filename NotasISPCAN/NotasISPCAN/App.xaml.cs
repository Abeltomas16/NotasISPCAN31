using Firebase.Auth;
using Firebase.Database;
using NotasISPCAN.Commom.Validation;
using NotasISPCAN.Features.Service;
using NotasISPCAN.Relatorio;
using NotasISPCAN.Relatorio.Strategy;
using NotasISPCAN.ViewModel;
using Splat;
using Xamarin.Forms;

namespace NotasISPCAN
{
    public partial class App : Application
    {
        public App()
        {
            Dependences();
            InitializeComponent();
            XF.Material.Forms.Material.Init(this);
            MainPage = new AppShell();
        }
        private void Dependences()
        {

            Locator.CurrentMutable.RegisterLazySingleton<IRouting>(() => new IRoutingService());
            Locator.CurrentMutable.RegisterLazySingleton<IAuthenticationService>(() => new LoadingPageService());
            Locator.CurrentMutable.RegisterLazySingleton<ILogin>(() => new LoginService());
            Locator.CurrentMutable.RegisterLazySingleton<IDocente>(() => new DocenteService());
            Locator.CurrentMutable.RegisterLazySingleton<IUsuario>(() => new UsuarioService());
            Locator.CurrentMutable.RegisterLazySingleton<IActividades>(() => new ActividadesService());
            Locator.CurrentMutable.RegisterLazySingleton<ICadeira>(() => new CadeiraService());
            Locator.CurrentMutable.RegisterLazySingleton<INotas>(() => new NotasService());
            Locator.CurrentMutable.RegisterLazySingleton<IEstudante>(() => new EstudanteService());

            Locator.CurrentMutable.Register(() => new PdfGenerateContext<PdfProfessorStrategy>());
            Locator.CurrentMutable.Register(() => new CadeiraValidator());
            Locator.CurrentMutable.Register(() => new CadeiraViewModel("Cadeiras"), "Cadeira");
            Locator.CurrentMutable.Register(() => new CadeiraViewModel("CadeirasMostrar"), "CadeirasMostrar");
            Locator.CurrentMutable.Register(() => new LoadingViewModel());
            Locator.CurrentMutable.Register(() => new LoginViewModel());
            Locator.CurrentMutable.Register(() => new ActividadesViewModel());
            Locator.CurrentMutable.Register(() => new UsuarioViewModel());
            Locator.CurrentMutable.Register(() => new DocenteViewModel());
            Locator.CurrentMutable.Register(() => new EstudantesViewModel());
            Locator.CurrentMutable.Register(() => new LoginValidator());
            Locator.CurrentMutable.Register(() => new ActividadesValidator());
            Locator.CurrentMutable.Register(() => new Usuariovalidator());
            Locator.CurrentMutable.Register(() => new NotasValidar());
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
