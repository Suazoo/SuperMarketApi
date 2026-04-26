using FluentValidation;
using SuperMarketAPI.DTOs;

namespace SuperMarketAPI.Validators;

public class CrearProductoValidator : AbstractValidator<CrearProductoDTO>
{
    public CrearProductoValidator()
    {
        RuleFor(x => x.Nombre)
            .NotEmpty().WithMessage("El nombre del producto es obligatorio")
            .MaximumLength(100).WithMessage("El nombre no puede exceder 100 caracteres");

        RuleFor(x => x.Precio)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a 0")
            .LessThan(1000000).WithMessage("El precio no puede exceder 1,000,000");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo");

        RuleFor(x => x.CategoriaId)
            .GreaterThan(0).WithMessage("Debe especificar una categoría válida");

        RuleFor(x => x.Descripcion)
            .MaximumLength(500).WithMessage("La descripción no puede exceder 500 caracteres")
            .When(x => x.Descripcion != null);
    }
}