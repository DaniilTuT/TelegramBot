using Domain.ValueObjects;
using FluentValidation;

namespace Domain.Validators;

public class FullNameValidator : AbstractValidator<FullName>
{
    public FullNameValidator()
    {
        //TODO: Провалидировать ввсе поля
        RuleFor(x => x.FirstName)
            .NotNull().WithMessage(x => ValidationMessages.NotNull(nameof(x)))
            .NotEmpty().WithMessage(x => ValidationMessages.NotEmpty(nameof(x)))
            .Matches(@"[a-zA-Zа-яА-Я]+").WithMessage()
            ;
        RuleFor(x => x.LastName)
            .NotNull();
        
        
    }
}