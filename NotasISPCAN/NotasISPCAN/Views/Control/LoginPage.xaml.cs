using NotasISPCAN.ViewModel;
using Rg.Plugins.Popup.Services;
using Splat;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NotasISPCAN.Views.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel ViewModel { get; set; } = Locator.Current.GetService<LoginViewModel>();
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = ViewModel;
        }
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
        private async void AjudaFacebook(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new Animation.Animation(Features.Enums.EnumPerfil.Email));
        }
        private void MenuItem2_Clicked(object sender, EventArgs e)
        {
            var texto = (ToolbarItem)sender;
            DisplayAlert("info", texto.Text, "ok");
        }
    }
}
