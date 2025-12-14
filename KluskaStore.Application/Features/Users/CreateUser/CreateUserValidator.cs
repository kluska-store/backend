using System.Data;
using FluentValidation;

namespace KluskaStore.Application.Features.Users.CreateUser;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.Cpf).NotEmpty().Length(11);
        RuleFor(c => c.Username).NotEmpty();
        RuleFor(c => c.Phone).NotEmpty();
        RuleFor(c => c.Birthday).LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow));
        RuleFor(c => c.RawPassword).NotEmpty();
    }
}
