using NotasISPCAN.Features.Enums;
using NotasISPCAN.Models;
using NotasISPCAN.ViewModel;
using Splat;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace NotasISPCAN.Views.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cadeiras : ContentPage
    {
        private EnumAdmCRUD EnumCadeiras;
        CadeiraDTO cadeiraCurrent = null;
        CadeiraViewModel CadeiraViewModel { get; } = Locator.Current.GetService<CadeiraViewModel>("Cadeira");
        public Cadeiras()
        {
            InitializeComponent();
            try
            {
                EnumCadeiras = EnumAdmCRUD.Cadastrar;
                //Passo o tipo de telas do cadastro, porque há dois tipos:CADEIRA MOSTRAR e TELA DE INSERÇÃO DAS CADEIRAS
                BindingContext = CadeiraViewModel;
                txtNome.Text = String.Empty;
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
        private async void ViewCadeiras_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var result = await DisplayActionSheet("Acção", "Cancelar", null, new string[] { "Editar", "Apagar" });
            if (result == null || result == "Cancelar") return;
            cadeiraCurrent = e.CurrentSelection[0] as CadeiraDTO;
            if (result.Equals("Editar"))
            {
                txtNome.Text = cadeiraCurrent.Name;
                btCancelarEditar.IsVisible = true;

                EnumCadeiras = EnumAdmCRUD.Editar;
                btSalvarEditar.Text = "Actualizar";
            }
            else if (result.Equals("Apagar"))
            {
                try
                {
                    var questao = await DisplayAlert("Notas", "Tens certeza que pretendes Apagar a cadeira selecionada?", "Sim", "Cancelar");
                    if (!questao)
                        return;

                    var resultado = await CadeiraViewModel.Apagar(cadeiraCurrent.IDCadeira);
                    await CadeiraViewModel.Carregar();
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
            }
        }
        private void btCancelarEditar_Clicked(object sender, EventArgs e)
        {
            btCancelarEditar.IsVisible = false;
            EnumCadeiras = EnumAdmCRUD.Cadastrar;
            btSalvarEditar.Text = "Salvar";
            LimparCampo();
        }
        private void LimparCampo()
        {
            txtNome.Text = string.Empty;
        }
        private async void btSalvarEditar_Clicked(object sender, EventArgs e)
        {
            CadeiraDTO cadeira = new CadeiraDTO() { Name = txtNome.Text.Trim() };
            try
            {
                EnumOperacoes operacao = EnumOperacoes.Cadastrar;
                string resultado = string.Empty;

                if (EnumCadeiras == EnumAdmCRUD.Cadastrar)
                    operacao = EnumOperacoes.Cadastrar;
                else
                {
                    if (cadeiraCurrent == null)
                        return;
                    cadeira.IDCadeira = cadeiraCurrent.IDCadeira;
                    cadeira.Docente = cadeiraCurrent.Docente;
                    operacao = EnumOperacoes.Editar;
                }
                resultado = await CadeiraViewModel.CadastrarEditar(cadeira, operacao);

                await CadeiraViewModel.Carregar();
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
        }
    }
}
