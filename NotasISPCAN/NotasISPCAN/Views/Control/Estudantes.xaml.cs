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
    public partial class Estudantes : ContentPage
    {
        EnumAdmCRUD EnumEstudantes;
        UsuarioDTO estudanteCurrent = null;
        UsuarioViewModel estudantesViewModel = Locator.Current.GetService<UsuarioViewModel>();
        public Estudantes()
        {
            InitializeComponent();
            EnumEstudantes = EnumAdmCRUD.Cadastrar;
            try
            {
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
            var result = await DisplayActionSheet("Acção", "Cancelar", null, new string[] { "Editar", "Apagar" });
            if (result == null || result == "Cancelar") return;
            estudanteCurrent = e.CurrentSelection[0] as UsuarioDTO;
            try
            {
                if (result.Equals("Editar"))
                {
                    txtNome.Text = estudanteCurrent.Name;
                    txtPhone.Text = estudanteCurrent.Telefone;
                    txtSenha.Text = estudanteCurrent.Senha;
                    txtEmail.Text = estudanteCurrent.Email;

                    btCancelar.IsVisible = true;
                    EnumEstudantes = EnumAdmCRUD.Editar;
                    btSalvar.Text = "Actualizar";
                    txtEmail.IsEnabled = false;
                    txtSenha.IsEnabled = false;
                }
                else if (result.Equals("Apagar"))
                {
                    var questao = await DisplayAlert("Notas", "Tens certeza que pretendes Apagar o estudante selecionado?", "Sim", "Cancelar");
                    if (!questao)
                        return;

                    var resultado = await estudantesViewModel.Apagar(estudanteCurrent);
                    estudantesViewModel.Carregar();
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
        private void LimparCampo()
        {
            txtNome.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtSenha.Text = string.Empty;
            txtEmail.Text = string.Empty;
        }
        private void btCancelar_Clicked(object sender, EventArgs e)
        {
            btCancelar.IsVisible = false;
            EnumEstudantes = EnumAdmCRUD.Cadastrar;
            btSalvar.Text = "Salvar";
            txtEmail.IsEnabled = true;
            txtSenha.IsEnabled = true;
            LimparCampo();
        }
        private async void btSalvar_Clicked(object sender, EventArgs e)
        {
            UsuarioDTO usuario = new UsuarioDTO()
            {
                Name = txtNome.Text.Trim(),
                Telefone = txtPhone.Text,
                Senha = txtSenha.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Categoria = "Estudante"
            };
            try
            {
                EnumOperacoes operacao = EnumOperacoes.Cadastrar;
                string resultado = string.Empty;

                if (EnumEstudantes == EnumAdmCRUD.Cadastrar)
                    operacao = EnumOperacoes.Cadastrar;
                else
                {
                    if (estudanteCurrent == null)
                        return;
                    usuario.Token = estudanteCurrent.Token;
                    usuario.Key = estudanteCurrent.Key;
                    operacao = EnumOperacoes.Editar;
                }

                if (operacao == EnumOperacoes.Cadastrar)
                    resultado = await estudantesViewModel.Cadastrar(usuario);
                else
                    resultado = await estudantesViewModel.Alterar(usuario);

                estudantesViewModel.Carregar();
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
                btCancelar_Clicked(null, EventArgs.Empty);
            }
        }
    }
}