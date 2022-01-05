using FluentValidation;
using NotasISPCAN.Commom.Resources;
using NotasISPCAN.Models;

namespace NotasISPCAN.Commom.Validation
{
    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleFor(e => e.email)
                .EmailAddress().WithErrorCode(statusCode.EmailInvalid);

            RuleFor(s => s.Senha)
                .NotNull()
                .NotEmpty().WithErrorCode(statusCode.SenhaIsNullOrEmpty);
        }
    }
}
