using FluentValidation;
using NotasISPCAN.Commom.Resources;
using NotasISPCAN.Models;

namespace NotasISPCAN.Commom.Validation
{
    public class CadeiraValidator : AbstractValidator<CadeiraDTO>
    {
        public CadeiraValidator()
        {
            RuleFor(e => e.Name)
                .NotNull()
                .NotEmpty().WithErrorCode(statusCode.DescricaoIsNotNullOrEmpty)
                .Must((nome) =>
                {
                    bool retorno = true;
                    foreach (char letra in nome)
                    {
                        if (char.IsNumber(letra) || char.IsSymbol(letra))
                            retorno = false;
                    }
                    return retorno;
                }).WithErrorCode(statusCode.DescricaoNotCaracteresEspecial);

            RuleFor(z => z.IDCadeira)
                 .NotNull()
                .NotEmpty().WithErrorCode(statusCode.IdDeveSerInformado);
        }
    }
}
