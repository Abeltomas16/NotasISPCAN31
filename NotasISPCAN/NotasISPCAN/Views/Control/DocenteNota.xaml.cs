using NotasISPCAN.Models;
using NotasISPCAN.ViewModel;
using Splat;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace NotasISPCAN.Views.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DocenteNota : ContentPage
    {
        public DocenteNota()
        {
            InitializeComponent();
            try
            {
                UsuarioViewModel estudantesViewModel = Locator.Current.GetService<UsuarioViewModel>();
                BindingContext = estudantesViewModel;
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
        private async void ViewEstudante_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                UsuarioDTO estudante = (e.CurrentSelection.FirstOrDefault() as UsuarioDTO);

                if (estudante == null) return;

                Locator.CurrentMutable.Register(() => new NotasViewModel(estudante.Key));

                await Shell.Current.Navigation.PushAsync(new NotasDetalhes(estudante));
            }
            catch (Exception)
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Erro, contacte o administrador", actionButtonText: "Ok", msDuration: MaterialSnackbar.DurationLong,
                    new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                    {
                        BackgroundColor = Color.Orange,
                        MessageTextColor = Color.Black
                    });
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {

        }
    }
}