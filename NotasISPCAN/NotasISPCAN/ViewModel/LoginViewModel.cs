using NotasISPCAN.Commom.Resources;
using NotasISPCAN.Commom.Validation;
using NotasISPCAN.Features.Service;
using NotasISPCAN.Models;
using Splat;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace NotasISPCAN.ViewModel
{
    public class LoginViewModel
    {
        public class er
        {
            public string message { get; set; }
        }
        private string Senha = string.Empty;
        private string Email = string.Empty;
        public string _email
        {
            set { Email = value; }
        }
        public string _senha
        {
            set { Senha = value; }
        }
        LoginValidator Validations;
        IRouting routing;
        ILogin LoginNegocios;
        IUsuario usuarioService;
        ICadeira cadeiraService;
        public ICommand ExecuteSignIn { get; }
        public LoginViewModel()
        {
            LoginNegocios = Locator.Current.GetService<ILogin>();
            this.routing = Locator.Current.GetService<IRouting>();
            this.usuarioService = Locator.Current.GetService<IUsuario>();
            this.cadeiraService = Locator.Current.GetService<ICadeira>();
            Validations = Locator.Current.GetService<LoginValidator>();
            ExecuteSignIn = new Command(() => ValidarEntrada(this.Email, this.Senha));
        }
        internal async void ValidarEntrada(string email, string senha)
        {
            IMaterialModalPage load = null;
            try
            {
                Login _login = new Login { email = email, Senha = senha };
                var results = Validations.Validate(_login);
                if (!results.IsValid)
                {
                    foreach (var item in results.Errors)
                    {
                        if (item.ErrorCode == statusCode.EmailInvalid)
                        {
                            await MaterialDialog.Instance.SnackbarAsync("Email inválido", MaterialSnackbar.DurationShort, new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                            {
                                BackgroundColor = Color.Red
                            });
                        }
                        else if (item.ErrorCode == statusCode.SenhaIsNullOrEmpty)
                        {
                            await MaterialDialog.Instance.SnackbarAsync("Informe a senha", MaterialSnackbar.DurationShort, new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                            {
                                BackgroundColor = Color.Red
                            });
                        }
                        return;
                    }
                }
                load = await MaterialDialog.Instance.LoadingDialogAsync(message: "Verificando");
                var resultado = await LoginNegocios.SignIn(email, Senha);
                if (resultado != null)
                {
                    UsuarioDTO categoria = await usuarioService.Pesquisar(resultado);
                    await load.DismissAsync();
                    Application.Current.Properties["NomeUsuario"] = categoria.Name;
                    Application.Current.Properties["TelefoneUsuario"] = categoria.Telefone;
                    Preferences.Set("Categoria", categoria.Categoria);
                    if (categoria.Categoria == "Estudante")
                    {
                        Application.Current.Properties["IDEstudante"] = categoria.Key;
                        await routing.NavigateTo("///main");
                    }
                    else if (categoria.Categoria == "Administrador")
                        await routing.NavigateTo("///admin");
                    else if (categoria.Categoria == "Professor")
                    {
                        //carregar todas cadeiras que ele leciona
                        var cadeiras = await cadeiraService.MostrarPorID(categoria.Token);
                        if (cadeiras.Count <= 0)
                        {
                            await MaterialDialog.Instance.SnackbarAsync("O Srº Professor Ainda não leciona uma disciplina, por essa razão, não vai poder efectuar o login, Obrigado pela comprensão!", 10000);
                            return;
                        }
                        else if (cadeiras.Count == 1)
                        {
                            Application.Current.Properties["IDCadeira"] = cadeiras.FirstOrDefault().IDCadeira;
                            Application.Current.Properties["Nomecadeira"] = cadeiras.FirstOrDefault().Name;
                        }
                        else
                        {
                            var resultadoAction = await MaterialDialog.Instance.SelectActionAsync(title: "Seleciona uma Cadeira para continuares",
                                                                                                          actions: cadeiras.Select(x => x.Name).ToArray());
                            if (resultadoAction == -1) return;
                            var _cadeira = cadeiras[resultadoAction];
                            Application.Current.Properties["IDCadeira"] = _cadeira.IDCadeira;
                            Application.Current.Properties["Nomecadeira"] = _cadeira.Name;
                        }
                        await routing.NavigateTo("///professor");
                    }
                }
            }
            catch (Exception)
            {
                if (!(load is null))
                    await load.DismissAsync();

                await MaterialDialog.Instance.SnackbarAsync("E-mail ou senha incorreta", MaterialSnackbar.DurationShort);
            }
        }
    }
}
