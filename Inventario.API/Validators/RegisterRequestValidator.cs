using FluentValidation;
using Inventario.API.Requests;

namespace Inventario.API.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("El nombre de usuario es obligatorio.")
            .MinimumLength(3).WithMessage("El nombre de usuario debe tener al menos 3 caracteres.")
            .MaximumLength(20).WithMessage("El nombre de usuario no puede exceder los 20 caracteres.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria.")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres.")
            .MaximumLength(100).WithMessage("La contraseña no puede exceder los 100 caracteres.");

        RuleFor(x => x.Email)
            .EmailAddress().WithMessage("El correo electrónico no es válido.")
            .When(x => !string.IsNullOrEmpty(x.Email));

        RuleFor(x => x.Role)
            .Must(role => role == null || role == "User" || role == "Admin")
            .WithMessage("El rol debe ser 'User' o 'Admin' si se proporciona.");
    }
}