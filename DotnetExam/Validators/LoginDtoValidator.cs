using DotnetExam.Features.Auth.Login;
using DotnetExam.Infrastructure;
using FluentValidation;

namespace DotnetExam.Validators;

public class LoginDtoValidator : AbstractValidator<LoginEndpoint.LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(request => request.Login)
            .NotEmpty()
            .WithMessage(ValidationFailedMessages.EmptyField);

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage(ValidationFailedMessages.EmptyField);
    }
}