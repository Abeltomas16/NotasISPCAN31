using NotasISPCAN.Commom.Resources;
using NotasISPCAN.Commom.Validation;
using NotasISPCAN.Features.Service;
using NotasISPCAN.Models;
using Splat;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using XF.Material.Forms.UI.Dialogs;

namespace NotasISPCAN.ViewModel
{
    public class UsuarioViewModel : BaseViewModel
    {
        IUsuario Idata;
        Usuariovalidator Validations;
        public UsuarioViewModel(IUsuario usuario = null)
        {
            Idata = usuario ?? Locator.Current.GetService<IUsuario>();
            Validations = Locator.Current.GetService<Usuariovalidator>();
            Carregar();
        }
        public int Total { get; private set; }
        public ObservableCollection<UsuarioDTO> Estudantes { get; set; }
        public async void Carregar()
        {
            IMaterialModalPage load = null;
            try
            {
                load = await MaterialDialog.Instance.LoadingDialogAsync(message: "Carregando");
                var estudantes = await Idata.ListarTodos();
                Estudantes = new ObservableCollection<UsuarioDTO>(estudantes.Where(y => y.Categoria == "Estudante"));
                OnPropertyChanged("Estudantes");
                Total = Estudantes.Count;
                OnPropertyChanged("Total");
                await load.DismissAsync();
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
                if (!(load is null))
                    await load.DismissAsync();
            }
        }
        public async Task<string> Cadastrar(UsuarioDTO usuario)
        {
            var results = Validations.Validate(usuario);
            if (!results.IsValid)
            {
                foreach (var item in results.Errors)
                {
                    if (item.ErrorCode == statusCode.NomeIsNullOrEmpty ||
                        item.ErrorCode == statusCode.NomeNotCaractereEspecial)
                        return "Nome inválido";
                    if (item.ErrorCode == statusCode.TelefoneIsNotNullOrEmpty)
                        return "Telefone inválido";
                    if (item.ErrorCode == statusCode.TelefoneCaractereMinimo)
                        return "Telefone requer 9 digítos";
                    if (item.ErrorCode == statusCode.EmailInvalid)
                        return "Email inválido";
                    if (item.ErrorCode == statusCode.CategoriaInvalido)
                        return "categoria inválida";
                    if (item.ErrorCode == statusCode.SenhaIsNullOrEmpty)
                        return "Informe a senha";
                    if (item.ErrorCode == statusCode.SenhaCaractereMinimo)
                        return "A senha tem que 4 caracteres no minímo";
                }
            }
            var load = await MaterialDialog.Instance.LoadingDialogAsync(message: "Salvando");
            var retorno = await Idata.Cadastrar(usuario);
            await load.DismissAsync();
            return retorno;
        }
        public async Task<string> Alterar(UsuarioDTO usuario)
        {
            var results = Validations.Validate(usuario);
            if (!results.IsValid)
            {
                foreach (var item in results.Errors)
                {
                    if (item.ErrorCode == statusCode.NomeIsNullOrEmpty ||
                        item.ErrorCode == statusCode.NomeNotCaractereEspecial)
                        return "Nome inválido";
                    if (item.ErrorCode == statusCode.TelefoneIsNotNullOrEmpty)
                        return "Telefone inválido";
                    if (item.ErrorCode == statusCode.TelefoneCaractereMinimo)
                        return "Telefone requer 9 digítos";
                    if (item.ErrorCode == statusCode.EmailInvalid)
                        return "Email inválido";
                    if (item.ErrorCode == statusCode.CategoriaInvalido)
                        return "categoria inválida";
                    if (item.ErrorCode == statusCode.SenhaIsNullOrEmpty)
                        return "Informe a senha";
                    if (item.ErrorCode == statusCode.SenhaCaractereMinimo)
                        return "A senha tem que 4 caracteres no minímo";
                    if (item.ErrorCode == statusCode.IdDeveSerInformado)
                        return "Informe o id";
                    if (item.ErrorCode == statusCode.IdDeveSerInformado)
                        return "Informe o token";
                }
            }
            var load = await MaterialDialog.Instance.LoadingDialogAsync(message: "Actualizando");
            var retorno = await Idata.Alterar(usuario, usuario.Key);
            await load.DismissAsync();
            return retorno;
        }
        public async Task<string> Apagar(UsuarioDTO usuarioDTO)
        {
            var results = Validations.Validate(usuarioDTO);
            if (!results.IsValid)
            {
                foreach (var item in results.Errors)
                {
                    if (item.ErrorCode == statusCode.EmailInvalid)
                        return "Email inválido";
                    if (item.ErrorCode == statusCode.CategoriaInvalido)
                        return "categoria inválida";
                    if (item.ErrorCode == statusCode.SenhaIsNullOrEmpty)
                        return "Informe a senha";
                    if (item.ErrorCode == statusCode.SenhaCaractereMinimo)
                        return "A senha tem que 4 caracteres no minímo";
                    if (item.ErrorCode == statusCode.IdDeveSerInformado)
                        return "Informe o id";
                    if (item.ErrorCode == statusCode.IdDeveSerInformado)
                        return "Informe o token";
                }
            }
            var load = await MaterialDialog.Instance.LoadingDialogAsync(message: "Apagando");
            var resultado = await Idata.Apagar(usuarioDTO);
            await load.DismissAsync();
            return resultado;
        }
        public async Task<string> UpdateEmail(string newEmail)
        {
            string retorno = await Idata.AlterarEmail(newEmail);
            return retorno;
        }
        public async Task<string> UpdateSenha(string newSenha)
        {
            string retorno = await Idata.AlterarSenha(newSenha);
            return retorno;
        }
    }
}
