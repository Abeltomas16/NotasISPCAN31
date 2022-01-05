using NotasISPCAN.ViewModel;
using Splat;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace NotasISPCAN.Views.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActividadesTabbed : ContentPage
    {
        ActividadesViewModel Service { get; } = Locator.Current.GetService<ActividadesViewModel>();
        public ActividadesTabbed()
        {
            InitializeComponent();
            try
            {
                BindingContext = Service;
            }
            catch (Exception)
            {
                MaterialDialog.Instance.SnackbarAsync(message: "Erro, contacte o administrador", actionButtonText: "Ok", msDuration: MaterialSnackbar.DurationLong,
                    new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                    {
                        BackgroundColor = Color.Orange,
                        MessageTextColor = Color.Black
                    });
            }

        }
        private async void MenuPerfil_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Perfil());
        }

        private async void MenuSobre_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Sobre());
        }

        private void MenuLogout_Clicked(object sender, EventArgs e)
        {
            Preferences.Remove("MyEmail");
            Preferences.Remove("MySenha");
            Preferences.Remove("MyFirebaseToken");
            Preferences.Remove("Categoria");
            Application.Current.Properties.Clear();

            Process.GetCurrentProcess().Kill();
        }
    }
}