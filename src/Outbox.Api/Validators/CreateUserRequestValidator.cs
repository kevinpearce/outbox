using FluentValidation;
using Microsoft.Extensions.Options;
using Outbox.Api.Configuration;
using Outbox.Api.Models;

namespace Outbox.Api.Validators;

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserRequestValidator(IOptions<ValidationSettings> options)
    {
        RuleFor(customer => customer.Name)
            .MaximumLength(options.Value.MaximumNameLength)
            .NotEmpty();
    }
}
