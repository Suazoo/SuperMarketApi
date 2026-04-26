using FluentValidation;
using SuperMarketAPI.DTOs;

namespace SuperMarketAPI.Validators;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenDTO>
{
    public RefreshTokenValidator()
    {
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("El token es obligatorio");

        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("El refresh token es obligatorio");
    }
}