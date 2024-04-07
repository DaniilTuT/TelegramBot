using Domain.ValueObjects;
using System.Text.RegularExpressions;
using Domain.Validators;

namespace Domain.Entities;

/// <summary>
/// Person - класс, представляющий человека с основными данными, такими как имя, дата рождения, возраст, номер телефона и телеграм-аккаунт.
/// </summary>


public class Person: BaseEntity
{
    public Person(FullName fullName, DateTime birthDay, string phoneNumber, string telegram)
    {
    //     FullName = ValidateFullName(fullName);
    //     BirthDay = ValidateBirthDay(birthDay);
    //     PhoneNumber = ValidatePhoneNumber(phoneNumber);
    //     Telegram = ValidateTelegram(telegram);
    var fullNameValidaator = new FullNameValidator();
    fullNameValidaator.Validate(fullName);
    FullName = fullName;
    
    //TODO: Провалидировать все поля
    
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
    /// ID Telegrama
    /// </summary>
    public string Telegram { get; set; }

    public List<CustomField<string>> CustomFields { get; set; }
    
    private DateTime ValidateBirthDay(DateTime birthday)
    {
        var age = (DateTime.Now.Month - birthday.Month <= 0 && DateTime.Now.Day - birthday.Day <= 0)
            ? DateTime.Now.Year - birthday.Year
            : DateTime.Now.Year - birthday.Year - 1;
        return (age <= 150) ? birthday : throw new ArgumentException();
    }

    private string ValidatePhoneNumber(string number)
    {
        const string numberPattern = @"[+]{1}37377[4-9]{1}[0-9]{5}$";
        if (Regex.IsMatch(number, numberPattern, RegexOptions.IgnoreCase))
        {
            return number;
        }

        throw new AggregateException("Неправильно введен номер телефона");
    }

    private string ValidateTelegram(string tg)
    {
        const string tgPattern = @"^@\S+";
        if (!Regex.IsMatch(tg, tgPattern))
        {
            throw new ArgumentException("Некоректно введенн телеграм тэг используйте только буквы латинского алфавита или кирилицу.");
        }

        return tg;
    }
    
    private FullName ValidateFullName(FullName fullName)
    {
        
        if (string.IsNullOrEmpty(fullName.FirstName) || string.IsNullOrEmpty(fullName.LastName))
        {
            throw new ArgumentNullException();
        }
        const string namePattern = @"[a-zA-Zа-яА-Я]+";
        if (fullName.MiddleName is not null)
        {
            if (fullName.MiddleName == string.Empty)
            {
                throw new ArgumentException("Вы не ввели отчество");
            }

            if (!Regex.IsMatch(fullName.MiddleName, namePattern))
            {
                throw new ArgumentException("Некоректно введенно отчество используйте только буквы латинского алфавита или кирилицу.");

            }
        }
        
        if (!Regex.IsMatch(fullName.FirstName, namePattern ) || !Regex.IsMatch(fullName.LastName, namePattern))
        {
            throw new ArgumentException("Некоректно введенно имя используйте только буквы латинского алфавита или кирилицу.");
        }
        
        // TODO: проверить допустимы только буквы русского и англ алфавита

        return fullName;
    }
}