using System;
using System.Threading.Tasks;
using NotasISPCAN.Features.Enums;
using NotasISPCAN.ViewModel;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Splat;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace NotasISPCAN.Views.Animation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Animation : PopupPage
    {
        EnumPerfil opcao = EnumPerfil.Email;
        UsuarioViewModel ViewModel { get; } = Locator.Current.GetService<UsuarioViewModel>();
        public Animation(EnumPerfil enumperfil)
        {
            InitializeComponent();
            opcao = enumperfil;
            if (opcao == EnumPerfil.Senha)
            {
                UsernameEntry.Placeholder = "Digite a senha antiga";
            }
            UsernameEntry.Focused += UsernameEntry_Focused;
        }

        private void UsernameEntry_Focused(object sender, FocusEventArgs e)
        {
            UsernameEntry.Placeholder = "";
        }

        protected override void OnAppearingAnimationBegin()
        {
            base.OnAppearingAnimationBegin();

            FrameContainer.HeightRequest = -1;

            if (!IsAnimationEnabled)
            {
                CloseImage.Rotation = 0;
                CloseImage.Scale = 1;
                CloseImage.Opacity = 1;

                LoginButton.Scale = 1;
                LoginButton.Opacity = 1;

                UsernameEntry.TranslationX = 0;
                UsernameEntry.Opacity = 1;

                return;
            }

            CloseImage.Rotation = 30;
            CloseImage.Scale = 0.3;
            CloseImage.Opacity = 0;

            LoginButton.Scale = 0.3;
            LoginButton.Opacity = 0;

            UsernameEntry.TranslationX = -10;
            UsernameEntry.Opacity = 0;
        }

        protected override async Task OnAppearingAnimationEndAsync()
        {
            if (!IsAnimationEnabled)
                return;

            var translateLength = 400u;

            await Task.WhenAll(
                UsernameEntry.TranslateTo(0, 0, easing: Easing.SpringOut, length: translateLength),
                UsernameEntry.FadeTo(1));

            await Task.WhenAll(
                CloseImage.FadeTo(1),
                CloseImage.ScaleTo(1, easing: Easing.SpringOut),
                CloseImage.RotateTo(0),
                LoginButton.ScaleTo(1),
                LoginButton.FadeTo(1));
        }

        protected override async Task OnDisappearingAnimationBeginAsync()
        {
            if (!IsAnimationEnabled)
                return;

            var taskSource = new TaskCompletionSource<bool>();

            var currentHeight = FrameContainer.Height;

            await Task.WhenAll(
                UsernameEntry.FadeTo(0),
                LoginButton.FadeTo(0));

            FrameContainer.Animate("HideAnimation", d =>
            {
                FrameContainer.HeightRequest = d;
            },
            start: currentHeight,
            end: 170,
            finished: async (d, b) =>
            {
                await Task.Delay(300);
                taskSource.TrySetResult(true);
            });

            await taskSource.Task;
        }

        private async void OnLogin(object sender, EventArgs e)
        {
            string resultado = string.Empty;
            try
            {
                if (opcao == EnumPerfil.Email)
                    resultado = await ViewModel.UpdateEmail(UsernameEntry.Text);
                else
                {
                    var senha = Preferences.Get("MySenha", null);
                    if (senha == UsernameEntry.Text)
                        resultado = await ViewModel.UpdateSenha(UsernameEntry.Text);
                    else
                    {
                        CloseAllPopup();
                        await MaterialDialog.Instance.SnackbarAsync(message: "Senha errada", actionButtonText: "Ok", msDuration: MaterialSnackbar.DurationLong,
                        new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                        {
                            BackgroundColor = Color.Orange,
                            MessageTextColor = Color.Black
                        });
                        return;
                    }
                }

                CloseAllPopup();
                await MaterialDialog.Instance.SnackbarAsync(message: resultado, actionButtonText: "Ok", msDuration: MaterialSnackbar.DurationLong,
                new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                {
                    BackgroundColor = Color.Orange,
                    MessageTextColor = Color.Black
                });
            }
            catch (Exception)
            {
                CloseAllPopup();
                await MaterialDialog.Instance.SnackbarAsync(message: "Erro, contacte o administrador", actionButtonText: "Ok", msDuration: MaterialSnackbar.DurationLong,
                   new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                   {
                       BackgroundColor = Color.Orange,
                       MessageTextColor = Color.Black
                   });
            }
        }

        private void OnCloseButtonTapped(object sender, EventArgs e)
        {
            CloseAllPopup();
        }

        protected override bool OnBackgroundClicked()
        {
            CloseAllPopup();

            return false;
        }

        private async void CloseAllPopup()
        {
            await PopupNavigation.Instance.PopAllAsync();
        }
    }
}

