using FluentValidation;
using Microsoft.Extensions.Options;
using Outbox.Api.Minimal.User.Models;

namespace Outbox.Api.Minimal.User.Validation;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator(IOptions<UserValidationConfiguration> options)
    {
        RuleFor(customer => customer.Name)
            .MaximumLength(options.Value.MaximumNameLength)
            .NotEmpty();
    }
}
