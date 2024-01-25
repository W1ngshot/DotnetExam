using DotnetExam.Features.Auth.RefreshTokens;
using DotnetExam.Infrastructure;
using FluentValidation;

namespace DotnetExam.Validators;

public class RefreshTokensDtoValidator : AbstractValidator<RefreshTokensEndpoint.RefreshTokensDto>
{
    public RefreshTokensDtoValidator()
    {
        RuleFor(dto => dto.Token)
            .NotEmpty()
            .WithMessage(ValidationFailedMessages.EmptyField);

        RuleFor(dto => dto.RefreshToken)
            .NotEmpty()
            .WithMessage(ValidationFailedMessages.EmptyField);
    }
}