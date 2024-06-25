using Ardalis.GuardClauses;
using Domain.Entities.ValueObjects;
using Domain.Primitives.Enums;
using Domain.Validations.Validators;
using Domain.Validations;

namespace Domain.Entities;

/// <summary>
/// Сущность человека
/// </summary>
public class Person : BaseEntity
{
    /// <summary>
    /// Имя
    /// </summary>
    public FullName FullName { get; set; }

    /// <summary>
    /// Гендер
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Дата рождения
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
    /// Никнейм в телеграм
    /// </summary>
    public string Telegram { get; set; }

    /// <summary>
    /// ID чата в котором создана сущность
    /// </summary>
    public long ChatId { get; set; }

    /// <summary>
    /// Кастомные поля
    /// </summary>
    public List<CustomField<string>> CustomFields { get; set; }

    /// <summary>
    /// Конструктор
    /// </summary>
    public Person(
        Guid id,
        FullName fullName,
        Gender gender,
        DateTime birthDate,
        string phoneNumber,
        string telegram,
        long chatId)
    {
        SetId(id);
        FullName = Guard.Against.Null(fullName);
        Gender = new EnumValidator<Gender>(nameof(gender), [Gender.None]).ValidateWithErrors(gender);
        BirthDay = new BirthDayValidator(nameof(birthDate)).ValidateWithErrors(birthDate);
        PhoneNumber = new PhoneNumberValidator(nameof(phoneNumber)).ValidateWithErrors(phoneNumber);
        Telegram = new TelegramValidator(nameof(telegram)).ValidateWithErrors(telegram);
        ChatId = chatId;
    }

    /// <summary>
    /// Обновление Person.
    /// </summary>
    public Person Update(
        string firstName,
        string lastName,
        string middleName,
        string phoneNumber,
        Gender gender,
        DateTime birthDate,
        string telegram)
    {
        FullName.Update(firstName, lastName, middleName);
        PhoneNumber = new PhoneNumberValidator(nameof(phoneNumber)).ValidateWithErrors(phoneNumber);
        Gender = new EnumValidator<Gender>(nameof(gender), [Gender.None]).ValidateWithErrors(gender);
        BirthDay = new BirthDayValidator(nameof(birthDate)).ValidateWithErrors(birthDate);
        Telegram = new TelegramValidator(nameof(telegram)).ValidateWithErrors(telegram);

        return this;
    }

    private Person()
    {
    }

    public void ConsoleWriteLine()
    {
        Console.WriteLine("Full Name: " + this.FullName.FirstName + ' ' + this.FullName.LastName + ' ' +
                          this.FullName.MiddleName);
        Console.WriteLine("Gender: " + this.Gender);
        Console.WriteLine("BirthDay: " + this.BirthDay);
        Console.WriteLine("Tg: " + this.Telegram);
        Console.WriteLine("Phone Number: " + this.PhoneNumber);
        Console.WriteLine("Id: " + this.Id + "\n");
        Console.WriteLine("ChatId: " + this.ChatId + "\n");
    }

    public override string ToString()
    {
        return "Ф.И.О.: " + this.FullName.FirstName + ' ' + this.FullName.LastName + ' ' + this.FullName.MiddleName +
               "\n" +
               "Пол: " + this.Gender + "\n" +
               "Дата Рождения: " + this.BirthDay.Date + "\n" +
               "Телеграм тэг: " + this.Telegram + "\n" +
               "Номер телефона: " + this.PhoneNumber;
    }
}