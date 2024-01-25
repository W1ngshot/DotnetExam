using DotnetExam.Features.Auth.Register;
using DotnetExam.Infrastructure;
using DotnetExam.Models.Main;
using DotnetExam.Services.Configs;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace DotnetExam.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterEndpoint.RegisterDto>
{
    private readonly UserManager<AppUser> _userManager;

    public RegisterDtoValidator(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
        
        RuleFor(request => request.Login)
            .NotEmpty()
            .WithMessage(ValidationFailedMessages.EmptyField)
            .MinimumLength(1)
            .WithMessage(ValidationFailedMessages.TooShortField)
            .MaximumLength(50)
            .WithMessage(ValidationFailedMessages.TooLongField)
            .Matches(@"^[a-zA-Z0-9]+$")
            .WithMessage(ValidationFailedMessages.WrongSymbols)
            .MustAsync(IsUniqueLoginAsync)
            .WithMessage(ValidationFailedMessages.AlreadyUsed);;

        RuleFor(request => request.Password)
            .NotEmpty()
            .WithMessage(ValidationFailedMessages.EmptyField)
            .MaximumLength(60)
            .WithMessage(ValidationFailedMessages.TooLongField)
            .MinimumLength(PasswordConfig.MinimumLength)
            .WithMessage(ValidationFailedMessages.TooShortField)
            .Matches(@"^[a-zA-Z0-9\W]+$")
            .WithMessage(ValidationFailedMessages.WrongSymbols)
            .Matches("[a-z]")
            .When(_ => PasswordConfig.RequireLowercase, ApplyConditionTo.CurrentValidator)
            .WithMessage(ValidationFailedMessages.RequiresLowercase)
            .Matches("[A-Z]")
            .When(_ => PasswordConfig.RequireUppercase, ApplyConditionTo.CurrentValidator)
            .WithMessage(ValidationFailedMessages.RequiresUppercase)
            .Matches(@"\d")
            .When(_ => PasswordConfig.RequireDigit, ApplyConditionTo.CurrentValidator)
            .WithMessage(ValidationFailedMessages.RequiresDigit)
            .Matches(@"\W")
            .When(_ => PasswordConfig.RequireNonAlphanumeric, ApplyConditionTo.CurrentValidator)
            .WithMessage(ValidationFailedMessages.RequiresNonAlphanumeric);
    }
    
    private async Task<bool> IsUniqueLoginAsync(string login, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(login);
        return user is null;
    }
}