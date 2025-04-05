using FluentValidation;
using Learnify.Core.Dto.Auth;

namespace Learnify.Core.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(r => r.Email).EmailAddress();
        RuleFor(r => r.Password).Must(ValidatePassword).WithMessage("Password doesn't satisfy the rules");
        RuleFor(r => r.ConfirmPassword)
            .Equal(x => x.Password)
            .WithMessage("Passwords do not match");
        RuleFor(r => r.Username).NotNull().NotEmpty().MinimumLength(3).MaximumLength(20);
    }

    private bool ValidatePassword(string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return false;
        }
        
        if (password.Length is < 8 or > 30)
        {
            return false;
        }
        
        if (!password.Any(char.IsUpper))
        {
            return false;
        }

        if (!password.Any(char.IsLower))
        {
            return false;
        }

        if (password.Count(char.IsDigit) < 2)
        {
            return false;
        }

        return true;
    }
}