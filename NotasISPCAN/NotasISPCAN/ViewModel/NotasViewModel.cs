using NotasISPCAN.Commom.Resources;
using NotasISPCAN.Commom.Validation;
using NotasISPCAN.Features.Service;
using NotasISPCAN.Models;
using Splat;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace NotasISPCAN.ViewModel
{
    public class NotasViewModel : BaseViewModel
    {
        INotas notasService;
        NotasValidar Validations;
        public NotasViewModel(string estudantekey, INotas notas = null)
        {
            notasService = notas ?? Locator.Current.GetService<INotas>();
            Cadastrar = new Command(() => Salvar());
            cadeiraKey = Application.Current.Properties["IDCadeira"].ToString();
            Validations = Locator.Current.GetService<NotasValidar>();
            EstudanteKey = estudantekey;
            Task.Run(async () => await CarregaNotas(estudantekey));
        }
        private async Task CarregaNotas(string estudantekey)
        {
            NotasDTO nota = await notasService.listarPorCadeira(cadeiraKey, estudantekey);
            IDNota = nota.Key ?? null;
            _Nota1 = nota.Nota1 ?? null;
            _Nota2 = nota.Nota2 ?? null;
            OnPropertyChanged("_Nota1");
            OnPropertyChanged("_Nota2");
        }
        private string EstudanteKey { get; set; }
        private string cadeiraKey { get; set; }
        public string _Nota1 { get; set; }
        public string _Nota2 { get; set; }
        private string IDNota { get; set; }
        public ICommand Cadastrar { get; set; }
        private async void Salvar()
        {
            string retorno = string.Empty;
            NotasDTO notas = new NotasDTO
            {
                Nota1 = _Nota1 ?? null,
                Nota2 = _Nota2 ?? null,
                KeyCadeira = cadeiraKey,
                KeyAluno = EstudanteKey
            };

            if (string.IsNullOrEmpty(IDNota))
            {
                string verifica = ValidateInsert(notas);
                if (verifica != null)
                {
                    await ExibeMensagem(verifica);
                    return;
                };
                retorno = await notasService.Cadastrar(notas);
                await ExibeMensagem(retorno);
            }
            else
            {
                notas.Key = IDNota;
                string verifica = ValidateUpdate(notas);
                if (verifica != null)
                {
                    await ExibeMensagem(verifica);
                    return;
                };
                retorno = await notasService.Alterar(notas);
                await ExibeMensagem(retorno);
            }

        }
        public async Task ExibeMensagem(string retono)
        {
            await MaterialDialog.Instance.SnackbarAsync(message: retono, actionButtonText: "Ok", msDuration: MaterialSnackbar.DurationLong,
                  new XF.Material.Forms.UI.Dialogs.Configurations.MaterialSnackbarConfiguration
                  {
                      BackgroundColor = System.Drawing.Color.Orange,
                      MessageTextColor = System.Drawing.Color.Black
                  });
        }
        public string ValidateInsert(NotasDTO notas)
        {
            string retorno = null;
            var results = Validations.Validate(notas);
            if (!results.IsValid)
            {
                foreach (var item in results.Errors)
                {
                    if (item.ErrorMessage == statusCode.IdAlunoNotNull)
                        retorno = "Informe o aluno";
                    if (item.ErrorCode == statusCode.IdCadeiraNotNull)
                        retorno = "Informe a cadeira";
                    if (item.ErrorCode == statusCode.Nota1NotNull)
                        retorno = "Nota 1 não pode ser nula";
                    if (item.ErrorCode == statusCode.NotaInvalid)
                        retorno = "Nota 1 inválida";
                    if (item.ErrorCode == statusCode.Nota1MaiorQue20)
                        retorno = "Nota 1 não pode ser superior a 20";
                    if (item.ErrorCode == statusCode.NotaMenorZero)
                        retorno = "Nota não pode ser menor que 0";

                    if (item.ErrorCode == statusCode.Nota2NotNull)
                        retorno = "Nota 2 não pode ser nula";
                    if (item.ErrorCode == statusCode.Nota2MaiorQue20)
                        retorno = "Nota 2 não pode ser superior a 20";
                    if (item.ErrorCode == statusCode.NotaMenorZero)
                        retorno = "Nota não pode ser menor que 0";
                }
            }
            return retorno;
        }
        public string ValidateUpdate(NotasDTO notas)
        {
            string retorno = null;
            var results = Validations.Validate(notas);
            if (!results.IsValid)
            {
                foreach (var item in results.Errors)
                {
                    if (item.ErrorMessage == statusCode.IdDeveSerInformado)
                        retorno = "Informe o id";

                    if (item.ErrorMessage == statusCode.IdAlunoNotNull)
                        retorno = "Informe o aluno";
                    if (item.ErrorCode == statusCode.IdCadeiraNotNull)
                        retorno = "Informe a cadeira";
                    if (item.ErrorCode == statusCode.Nota1NotNull)
                        retorno = "Nota 1 não pode ser nula";
                    if (item.ErrorCode == statusCode.NotaInvalid)
                        retorno = "Nota 1 inválida";
                    if (item.ErrorCode == statusCode.Nota1MaiorQue20)
                        retorno = "Nota 1 não pode ser superior a 20";
                    if (item.ErrorCode == statusCode.NotaMenorZero)
                        retorno = "Nota não pode ser menor que 0";

                    if (item.ErrorCode == statusCode.Nota2NotNull)
                        retorno = "Nota 2 não pode ser nula";
                    if (item.ErrorCode == statusCode.Nota2MaiorQue20)
                        retorno = "Nota 2 não pode ser superior a 20";
                    if (item.ErrorCode == statusCode.NotaMenorZero)
                        retorno = "Nota não pode ser menor que 0";
                }
            }
            return retorno;
        }
    }
}
