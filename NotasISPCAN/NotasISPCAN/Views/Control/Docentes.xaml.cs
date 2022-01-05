using NotasISPCAN.Features.Collections;
using NotasISPCAN.Features.Enums;
using NotasISPCAN.Models;
using NotasISPCAN.ViewModel;
using Splat;
using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;

namespace NotasISPCAN.Views.Control
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Docentes : ContentPage
    {
        EnumAdmCRUD EnumDocente;
        UsuarioDTO docenteCurrent = null;
        DocenteViewModel DocentesViewModel { get; } = Locator.Current.GetService<DocenteViewModel>();
        List<CadeiraDTO> cadeirasDoDocente = null;
        public Docentes()
        {
            InitializeComponent();
            try
            {
                EnumDocente = EnumAdmCRUD.Cadastrar;
                BindingContext = DocentesViewModel;
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
        private async void btSalvarEditar_Clicked(object sender, EventArgs e)
        {
            string resultado = string.Empty;
            if (CadeirasDocente.Cadeiras.Count <= 0)
            {
                await MaterialDialog.Instance.SnackbarAsync(message: "Nenhuma cadeira selecionada", actionButtonText: "Ok", msDuration: MaterialSnackbar.DurationLong,
                  new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                  {
                      BackgroundColor = Color.Orange,
                      MessageTextColor = Color.Black
                  });
                return;
            }
            UsuarioDTO docente = new UsuarioDTO()
            {
                Name = txtNome.Text,
                Telefone = txtPhone.Text,
                Senha = txtSenha.Text,
                Email = txtEmail.Text,
                Categoria = "Professor"
            };
            List<CadeiraDTO> cadeiras = CadeirasDocente.Cadeiras;
            try
            {
                EnumOperacoes operacao = EnumOperacoes.Cadastrar;

                if (EnumDocente == EnumAdmCRUD.Cadastrar)
                    operacao = EnumOperacoes.Cadastrar;
                else
                {
                    if (docenteCurrent == null)
                        return;
                    docente.Key = docenteCurrent.Key;
                    docente.Token = docenteCurrent.Token;
                    operacao = EnumOperacoes.Editar;
                }
                if (operacao == EnumOperacoes.Cadastrar)
                    resultado = await DocentesViewModel.Cadastrar(docente, cadeiras);
                else if (operacao == EnumOperacoes.Editar)
                    resultado = await DocentesViewModel.Alterar(docente, cadeiras);

                await DocentesViewModel.Carregar();
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
                btCancelarEditar_Clicked(null, EventArgs.Empty);
            }
        }
        private void btCancelarEditar_Clicked(object sender, EventArgs e)
        {
            btCancelarEditar.IsVisible = false;
            EnumDocente = EnumAdmCRUD.Cadastrar;
            btSalvarEditar.Text = "Salvar";
            AddCadeiras.Text = "Adicionar cadeiras";
            LimparCampo();
        }
        private void LimparCampo()
        {
            txtNome.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtSenha.Text = string.Empty;
            txtEmail.Text = string.Empty;

            viewcadeirasResumo.IsVisible = false;
            CadeirasDocente.Cadeiras = new List<CadeiraDTO>();
        }
        private async void ViewDocentes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var result = await DisplayActionSheet("Acção", "Cancelar", null, new string[] { "Editar", "Apagar" });
            if (result == null || result == "Cancelar") return;
            docenteCurrent = e.CurrentSelection[0] as UsuarioDTO;
            try
            {
                if (result.Equals("Editar"))
                {
                    cadeirasDoDocente = await DocentesViewModel.MostrarCadeira(docenteCurrent.Token);
                    txtNome.Text = docenteCurrent.Name;
                    txtPhone.Text = docenteCurrent.Telefone;
                    txtSenha.Text = docenteCurrent.Senha;
                    txtEmail.Text = docenteCurrent.Email;

                    btCancelarEditar.IsVisible = true;
                    EnumDocente = EnumAdmCRUD.Editar;
                    btSalvarEditar.Text = "Actualizar";
                    AddCadeiras.Text = "Ver cadeiras";
                }
                else if (result.Equals("Apagar"))
                {

                    var questao = await DisplayAlert("Notas", "Tens certeza que pretendes Apagar o docente selecionado?", "Sim", "Cancelar");
                    if (!questao)
                        return;

                    var resultado = await DocentesViewModel.Apagar(docenteCurrent);
                    await DocentesViewModel.Carregar();
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
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (CadeirasDocente.Cadeiras.Count > 0)
            {
                viewcadeirasResumo.IsVisible = true;
                viewcadeirasResumo.ItemsSource = CadeirasDocente.Cadeiras;
            }
        }
        private async void AddCadeiras_Clicked(object sender, EventArgs e)
        {
            if (EnumDocente == EnumAdmCRUD.Cadastrar)
                await Navigation.PushAsync(new CadeirasMostrar());
            else
            {
                if (cadeirasDoDocente == null)
                    return;

                await Navigation.PushAsync(new CadeirasMostrar(cadeirasDoDocente));
            }
        }
    }
}