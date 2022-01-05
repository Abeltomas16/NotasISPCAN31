using FluentValidation;
using NotasISPCAN.Commom.Resources;
using NotasISPCAN.Models;

namespace NotasISPCAN.Commom.Validation
{
    public class Usuariovalidator : AbstractValidator<UsuarioDTO>
    {
        public Usuariovalidator()
        {
            RuleFor(u => u.Name)
                .NotNull()
                .NotEmpty().WithErrorCode(statusCode.NomeIsNullOrEmpty)
                .Must((nome) =>
                {
                    bool retorno = true;
                    foreach (char letra in nome)
                    {
                        if (char.IsNumber(letra) || char.IsSymbol(letra))
                            retorno = false;
                    }
                    return retorno;
                }).WithErrorCode(statusCode.NomeNotCaractereEspecial);

            RuleFor(x => x.Telefone)
                .NotNull()
                .NotEmpty().WithErrorCode(statusCode.TelefoneIsNotNullOrEmpty)
                .Must((tlf) =>
                {
                    if (tlf.Trim().Length < 9)
                        return false;

                    foreach (char item in tlf)
                    {
                        if (!char.IsDigit(item))
                            return false;
                    }
                    return true;
                }).WithErrorCode(statusCode.TelefoneCaractereMinimo);

            RuleFor(y => y.Email)
                .EmailAddress().WithErrorCode(statusCode.EmailInvalid);

            RuleFor(c => c.Categoria)
                .Must((cat) =>
                {
                    bool retorno = (cat.ToUpper() == "PROFESSOR" || cat.ToUpper() == "ESTUDANTE") ? true : false;
                    return retorno;
                }).WithErrorCode(statusCode.CategoriaInvalido);

            RuleFor(b => b.Senha)
                .NotNull()
                .NotEmpty().WithErrorCode(statusCode.SenhaIsNullOrEmpty)
                .Must((senha) =>
                {
                    bool retorno = senha.Length < 3 ? false : true;
                    return retorno;
                }).WithErrorCode(statusCode.SenhaCaractereMinimo);

            RuleFor(m => m.Key)
                .NotNull()
                .NotEmpty().WithErrorCode(statusCode.IdDeveSerInformado);

            RuleFor(m => m.Token)
                .NotNull()
                .NotEmpty().WithErrorCode(statusCode.TokenDeveSerInformado);
        }
    }
}
