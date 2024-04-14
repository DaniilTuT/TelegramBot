using Domain.Primitives;
using Domain.ValueObjects;
using FluentValidation;

namespace Domain.Validators;

public class FullNameValidator : AbstractValidator<FullName>
{
    public FullNameValidator()
    {
        RuleFor(x => x.FirstName)
            .NotNull().WithMessage(x => ValidationMessages.NotNull(nameof(x)))
            .NotEmpty().WithMessage(x => ValidationMessages.NotEmpty(nameof(x)))
            .Matches(@"[a-zA-Zа-яА-Я]+").WithMessage(x => ValidationMessages.InvalidName(nameof(x)))
            ;
        RuleFor(x => x.LastName)
            .NotNull().WithMessage(x => ValidationMessages.NotNull(nameof(x)))
            .NotEmpty().WithMessage(x => ValidationMessages.NotEmpty(nameof(x)))
            .Matches(@"[a-zA-Zа-яА-Я]+").WithMessage(x => ValidationMessages.InvalidName(nameof(x)))
            ;
        RuleFor(x => x.MiddleName)
            .Matches(@"^[a-zA-Zа-яА-Я]+").WithMessage(x => ValidationMessages.InvalidName(nameof(x)))
            ;
        
    }
}