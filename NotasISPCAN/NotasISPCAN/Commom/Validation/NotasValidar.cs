using FluentValidation;
using NotasISPCAN.Commom.Resources;
using NotasISPCAN.Models;

namespace NotasISPCAN.Commom.Validation
{
    public class NotasValidar : AbstractValidator<NotasDTO>
    {
        public NotasValidar()
        {
            RuleFor(n => n.Key)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty().WithErrorCode(statusCode.IdDeveSerInformado);

            RuleFor(ka => ka.KeyAluno)
                 .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty().WithErrorCode(statusCode.IdAlunoNotNull);

            RuleFor(kc => kc.KeyCadeira)
                 .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty().WithErrorCode(statusCode.IdCadeiraNotNull);

            RuleFor(n1 => n1.Nota1)
                 .Cascade(CascadeMode.Stop).NotNull()
                .NotEmpty().WithErrorCode(statusCode.Nota1NotNull)
                .Must((n) =>
                {
                    bool retorno = double.TryParse(n, out double saida) ? true : false;
                    return retorno;
                }).WithErrorCode(statusCode.NotaInvalid)
                .Must((n) =>
                {
                    double valor = double.Parse(n);
                    bool retorno = valor > 20 ? false : true;
                    return retorno;
                }).WithErrorCode(statusCode.Nota1MaiorQue20)
                .Must((r) =>
                {
                    double valor = double.Parse(r);
                    bool retorno = valor < 0 ? false : true;
                    return retorno;
                }).WithErrorCode(statusCode.NotaMenorZero);

            RuleFor(n2 => n2.Nota2)
                  .Cascade(CascadeMode.Stop)
                  .NotNull()
                .NotEmpty().WithErrorCode(statusCode.Nota2NotNull)
                .Must((nn) =>
                {
                    bool retorno = double.TryParse(nn, out double saida) ? true : false;
                    return retorno;
                }).WithErrorCode(statusCode.NotaInvalid)
                .Must((vl) =>
                {
                    double valor = double.Parse(vl);
                    bool retorno = valor > 20 ? false : true;
                    return retorno;
                }).WithErrorCode(statusCode.Nota2MaiorQue20)
                .Must((ra) =>
                {
                    double valor = double.Parse(ra);
                    bool retorno = valor < 0 ? false : true;
                    return retorno;
                }).WithErrorCode(statusCode.NotaMenorZero);
        }
    }
}