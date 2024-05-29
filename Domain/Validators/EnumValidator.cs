using Domain.Primitives;
using FluentValidation;

namespace Domain.Validations.Validators;

/// <summary>
/// Валидация перечислений
/// </summary>
/// <typeparam name="TEnum">Перечисление.</typeparam>
public class EnumValidator<TEnum> : AbstractValidator<TEnum> where TEnum : Enum
{
    public EnumValidator(string paramName, params TEnum[] defaultValues) 
    {
        foreach (var value in defaultValues)
        {
            RuleFor(e => e)
                .NotEqual(value).WithMessage(ValidationMessages.DefaultEnum(paramName));
        }
    }
}