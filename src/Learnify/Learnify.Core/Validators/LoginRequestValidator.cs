using FluentValidation;
using Learnify.Core.Dto.Auth;

namespace Learnify.Core.Validators;

public class LoginRequestValidator: AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(e => e.Email).EmailAddress();
        RuleFor(l => l.Password).NotNull().NotEmpty();
    }
}