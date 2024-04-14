using Domain.Primitives;
using FluentValidation;

namespace Domain.Validators;

public class PhoneNumberValidator: AbstractValidator<string>
{
    public PhoneNumberValidator()
    {
        RuleFor(x => x)
            .NotNull().WithMessage(ValidationMessages.NotNull)
            .NotEmpty().WithMessage(ValidationMessages.NotEmpty)
            .Matches(@"[+]{1}37377[4-9]{1}[0-9]{5}$").WithMessage(x => ValidationMessages.InvalidPhoneNumber(nameof(x)));
    }
}