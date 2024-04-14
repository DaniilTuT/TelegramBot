using Domain.Primitives;
using FluentValidation;

namespace Domain.Validators;

public class TelegramValidator: AbstractValidator<string>
{
    public TelegramValidator()
    {
        RuleFor(x => x)
            .NotNull().WithMessage(ValidationMessages.NotNull)
            .NotEmpty().WithMessage(ValidationMessages.NotEmpty)
            .Matches(@"^@\S+").WithMessage(x => ValidationMessages.InvalidTelegramName(nameof(x)));
    }
}