using Domain.ValueObjects;
using System.Text.RegularExpressions;
using Domain.Primitives;
using Domain.Validators;
using FluentValidation;

namespace Domain.Entities;

/// <summary>
/// Person - класс, представляющий человека с основными данными, такими как имя, дата рождения, возраст, номер телефона и телеграм-аккаунт.
/// </summary>


public class Person: BaseEntity
{
    public Person(FullName fullName, DateTime birthDay, string phoneNumber, string telegram, Gender gender)
    {
    var fullNameValidaator = new FullNameValidator();
    fullNameValidaator.ValidateAndThrow(fullName);
    FullName = fullName;

    var phoneNumberValidator = new PhoneNumberValidator();
    phoneNumberValidator.ValidateAndThrow(phoneNumber);
    PhoneNumber = phoneNumber;

    var telegramValidator = new TelegramValidator();
    telegramValidator.ValidateAndThrow(telegram);
    Telegram = telegram;

    var birthDayValidator = new BirthDayValidator();
    birthDayValidator.ValidateAndThrow(birthDay);
    BirthDay = birthDay;
    
    
    Gender = gender;

    }
    
    /// <summary>
    /// Имя Фамиллия и Очество(опционально)
    /// </summary>
    public FullName FullName { get; set; }
    /// <summary>
    /// Дата дня рождения
    /// </summary>
    public DateTime BirthDay { get; set; }
    /// <summary>
    /// Возраст
    /// </summary>
    public int Age => (DateTime.Now.Month - BirthDay.Month >= 0 && DateTime.Now.Day - BirthDay.Day >= 0)
        ? DateTime.Now.Year - BirthDay.Year
        : DateTime.Now.Year - BirthDay.Year - 1;
    
    /// <summary>
    /// Номер телефона
    /// </summary>
    public string PhoneNumber { get; set; }
    /// <summary>
    /// Гендер человека
    /// </summary>
    public Gender Gender { get; set; }
    
    /// <summary>
    /// ID Telegrama
    /// </summary>
    public string Telegram { get; set; }

    public List<CustomField<string>> CustomFields { get; set; }

    public Person Update(string? firstName, string? lastName, string? middleName, string phoneNumber)
    {
        if (firstName is not null)
        {
            this.FullName.Update(firstName, lastName, middleName);
        }
        
        
        
        
        return this;
    }
}