using FluentValidation;
using SuperMarketAPI.DTOs;

namespace SuperMarketAPI.Validators;

public class CrearCategoriaValidator : AbstractValidator<CrearCategoriaDTO>
{
    public CrearCategoriaValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre de la categoría es obligatorio")
            .MaximumLength(50).WithMessage("El nombre no puede exceder 50 caracteres");

        RuleFor(x => x.Descripcion)
            .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres")
            .When(x => x.Descripcion != null);
    }
}