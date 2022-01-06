using NotasISPCAN.Models;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;
namespace NotasISPCAN.Views.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Perfil : ContentPage
    {
        int versaonova = 0;
        public Perfil()
        {
            InitializeComponent();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            List<UsuarioDTO> usuarios = new List<UsuarioDTO>();
            var email = Preferences.Get("MyEmail", null);
            var name = Application.Current.Properties["NomeUsuario"].ToString() ?? "";
            var Telefone = Application.Current.Properties["TelefoneUsuario"].ToString() ?? "";
            usuarios.Add(new UsuarioDTO
            {
                Email = email,
                Name = name,
                Telefone = Telefone
            });
            CollectionUser.ItemsSource = usuarios;
        }
        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new Animation.Animation(Features.Enums.EnumPerfil.Email));
        }
        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PushAsync(new Animation.Animation(Features.Enums.EnumPerfil.Senha));
        }
        private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
            string fileversion = "https://pastebin.com/raw/aWipeP7H";
            Uri filerepository = new Uri("https://novaversao.weebly.com/uploads/1/3/2/1/132135003/lampada_abel.exe");
            try
            {
                var load = await MaterialDialog.Instance.LoadingDialogAsync("Verificando");
                int versaoActual = 2;
                string content = await new HttpClient().GetStringAsync(fileversion);
                await load.DismissAsync();
                if (int.TryParse(content, out versaonova))
                {
                    bool update = versaonova > versaoActual;
                    if (update)
                    {
                        var pergunta = await MaterialDialog.Instance.ConfirmAsync(
                            message: $"Disponível a versão {versaonova} pretendes baixar ?",
                            confirmingText: "Sim",
                            dismissiveText: "Não");
                        if ((bool)!pergunta) return;

                        btnVerificar.IsVisible = false;
                        btnVerificar.IsEnabled = false;
                        using (WebClient client = new WebClient())
                        {
                            client.DownloadProgressChanged += Client_DownloadProgressChanged;
                            client.DownloadFileCompleted += Client_DownloadFileCompleted;
                            client.DownloadFileAsync(filerepository, @"/storage/emulated/0/Download/Notas" + versaonova + ".apk");
                        }
                    }
                    else
                    {
                        await MaterialDialog.Instance.SnackbarAsync(message: "Sem novas actualizações", actionButtonText: "Ok", msDuration: MaterialSnackbar.DurationLong,
                           new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                           {
                               BackgroundColor = Color.Orange,
                               MessageTextColor = Color.Black
                           });
                    }
                }
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
        private void Client_DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            percent.IsVisible = false;
            Device.BeginInvokeOnMainThread(async () =>
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Sucesso! salvo em: Download/Notas" + versaonova + ".apk", actionButtonText: "Ok", msDuration: 4000,
                    new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                    {
                        BackgroundColor = Color.Orange,
                        MessageTextColor = Color.Black
                    });
            });
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                percent.Text = "Baixando: " + e.ProgressPercentage.ToString() + "%";
            });
        }
    }
}