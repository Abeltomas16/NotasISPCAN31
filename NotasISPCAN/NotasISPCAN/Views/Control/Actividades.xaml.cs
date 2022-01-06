using NotasISPCAN.Features.Enums;
using NotasISPCAN.Models;
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
    public partial class Actividades : ContentPage
    {
        private EnumAdmCRUD EnumActividades;
        ActividadeDTO actividadeCurrent = null;
        ActividadesViewModel Service { get; } = Locator.Current.GetService<ActividadesViewModel>();
        public Actividades()
        {
            InitializeComponent();
            try
            {
                EnumActividades = EnumAdmCRUD.Cadastrar;
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
        private async void ViewActividades_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var result = await DisplayActionSheet("Acção", "Cancelar", null, new string[] { "Editar", "Apagar" });
            if (result == null || result == "Cancelar") return;
            actividadeCurrent = e.CurrentSelection[0] as ActividadeDTO;
            try
            {
                if (result.Equals("Editar"))
                {
                    labelDescricao.Text = actividadeCurrent.Descricao;
                    labelIMG.Text = actividadeCurrent.IMGURL;
                    btCancelarEditar.IsVisible = true;

                    EnumActividades = EnumAdmCRUD.Editar;
                    btSalvarEditar.Text = "Actualizar";
                }
                else if (result.Equals("Apagar"))
                {
                    var questao = await DisplayAlert("Notas", "Tens certeza que pretendes Apagar a actividade selecionada?", "Sim", "Cancelar");
                    if (!questao)
                        return;

                    var resultado = await Service.Apagar(actividadeCurrent.IDActividade);
                    await Service.Carregar();
                    await MaterialDialog.Instance.SnackbarAsync(message: resultado, actionButtonText: "Ok", msDuration: MaterialSnackbar.DurationLong,
                       new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                       {
                           BackgroundColor = Color.Orange,
                           MessageTextColor = Color.Black
                       });
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
        private void MaterialButtonCancelar_Clicked(object sender, EventArgs e)
        {
            btCancelarEditar.IsVisible = false;
            EnumActividades = EnumAdmCRUD.Cadastrar;
            btSalvarEditar.Text = "Salvar";
            LimparCampo();
        }
        private void LimparCampo()
        {
            labelDescricao.Text = string.Empty;
            labelIMG.Text = string.Empty;
            labelDescricao.Placeholder = "Descrição";
            labelIMG.Placeholder = "URL da imagem";
        }
        private async void MaterialButton_Clicked(object sender, EventArgs e)
        {
            ActividadeDTO actividades = new ActividadeDTO()
            {
                Descricao = labelDescricao.Text,
                IMGURL = labelIMG.Text,
                DataCadastro = DateTime.Now
            };
            try
            {
                string resultado = string.Empty;

                if (EnumActividades == EnumAdmCRUD.Cadastrar)
                    resultado = await Service.Cadastrar(actividades);
                else
                {
                    if (actividadeCurrent == null)
                        return;

                    actividades.IDActividade = actividadeCurrent.IDActividade;
                    resultado = await Service.Editar(actividades);
                }
                await Service.Carregar();
                await MaterialDialog.Instance.SnackbarAsync(message: resultado, actionButtonText: "Ok", msDuration: MaterialSnackbar.DurationLong,
                new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                {
                    BackgroundColor = Color.Orange,
                    MessageTextColor = Color.Black
                });
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
            finally
            {
                MaterialButtonCancelar_Clicked(null, EventArgs.Empty);
            }
        }

        private async void MenuSobre_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Sobre());
        }

        private async void MenuPerfil_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Perfil());
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
