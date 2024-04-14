using Domain.Primitives;
using FluentValidation;

namespace Domain.Validators;

public class BirthDayValidator: AbstractValidator<DateTime>
{
    public BirthDayValidator()
    {
        RuleFor(x => x.Year)
            .InclusiveBetween(DateTime.Now.Year-150, DateTime.Now.Year).WithMessage(x=> ValidationMessages.InvalidDate(nameof(x)));
    }
}