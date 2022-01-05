using NotasISPCAN.Models;
using NotasISPCAN.ViewModel;
using Splat;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace NotasISPCAN.Views.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NotasDetalhes : ContentPage
    {
        NotasViewModel _notasViewModel = Locator.Current.GetService<NotasViewModel>();
        public NotasDetalhes(UsuarioDTO estudante)
        {
            InitializeComponent();
            try
            {
                BindingContext = _notasViewModel;
                NomeEstudante.Text = estudante.Name;
                NomeCadeira.Text = Application.Current.Properties["Nomecadeira"].ToString();
                NomeProf.Text = Application.Current.Properties["NomeUsuario"].ToString();
            }
            catch (Exception)
            {
                XF.Material.Forms.UI.Dialogs.MaterialDialog.Instance.SnackbarAsync(message: "Erro, contacte o administrador", actionButtonText: "Ok", msDuration: XF.Material.Forms.UI.Dialogs.MaterialSnackbar.DurationLong,
                  new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                  {
                      BackgroundColor = Color.Orange,
                      MessageTextColor = Color.Black
                  });
            }
        }
    }
}