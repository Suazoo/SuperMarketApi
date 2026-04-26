using FluentValidation;
using SuperMarketAPI.DTOs;

namespace SuperMarketAPI.Validators;

public class RegisterAdminValidator : AbstractValidator<RegisterAdminDTO>
{
    public RegisterAdminValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre es obligatorio")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("El email es obligatorio")
            .EmailAddress().WithMessage("El formato del email no es válido");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("La contraseña es obligatoria")
            .MinimumLength(6).WithMessage("La contraseña debe tener al menos 6 caracteres");

        RuleFor(x => x.Rol)
            .Must(rol => rol == "Admin" || rol == "User")
            .WithMessage("El rol debe ser 'Admin' o 'User'");
    }
}