using AuthIdentity.Core.Dto;
using FluentValidation;

namespace AuthIdentity.Core.Validators;

public class LoginRequestValidator: AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(e => e.Email).EmailAddress();
        RuleFor(l => l.Password).NotNull().NotEmpty();
    }
}