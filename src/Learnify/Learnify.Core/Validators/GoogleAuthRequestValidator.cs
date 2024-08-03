using FluentValidation;
using Learnify.Core.Dto.Auth;

namespace Learnify.Core.Validators;

public class GoogleAuthRequestValidator: AbstractValidator<GoogleAuthRequest>
{
    public GoogleAuthRequestValidator()
    {
        RuleFor(r => r.Code).NotNull().NotEmpty();
        RuleFor(r => r.RedirectUrl).NotNull().NotEmpty();
        RuleFor(r => r.CodeVerifier).NotNull().NotEmpty();
        RuleFor(r => r.Role).IsInEnum();
    }
}