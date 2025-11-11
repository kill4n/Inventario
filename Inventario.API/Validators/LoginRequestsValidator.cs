using FluentValidation;
using Inventario.API.Requests;

namespace Inventario.API.Validators.Auth;

public class LoginRequestsValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestsValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
            .MaximumLength(50).WithMessage("El nombre de usuario no puede exceder los 50 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.");
    }

}