using Domain.Entities;
using Domain.Validators;
using FluentValidation;

namespace Domain.ValueObjects;

/// <summary>
/// Ф.И.О.
/// </summary>
public class FullName: BaseValueObject
{
    public FullName(string firstName, string lastName, string middleName)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        var fullNameValidaator = new FullNameValidator();
        fullNameValidaator.ValidateAndThrow(this);
    }
    
    /// <summary>
    /// Имя 
    /// </summary>
    public string FirstName { get; private set; }
    /// <summary>
    /// Фамилия 
    /// </summary>
    public string LastName { get; private set; }
    
    /// <summary>
    /// Может быть отчеством
    /// </summary>
    public string? MiddleName { get; private set; } = null;

    public FullName Update(string? firstName, string? lastName, string? middleName)
    {
        if (firstName is not null)
        {
            FirstName = firstName;
        }
        if (lastName is not null)
        {
            LastName = lastName;
        }
        if (middleName is not null)
        {
            MiddleName = middleName;
        }
        var fullNameValidaator = new FullNameValidator();
        fullNameValidaator.ValidateAndThrow(this);

        return this;
    }
}