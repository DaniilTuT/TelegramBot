using Domain.Primitives.Enums;

namespace Application.Dtos.Person;

/// <summary>
/// Базовое дто для Person
/// </summary>
public class BasePersonDto
{
    /// <summary>
    /// Полное имя
    /// </summary>
    public FullNameDto FullName { get; init; }
    
    /// <summary>
    /// Гендер
    /// </summary>
    public Gender Gender { get; init; }
    
    /// <summary>
    /// Дата рождения
    /// </summary>
    public DateTime BirthDay { get; init; }

    /// <summary>
    /// Возраст
    /// </summary>
    public int Age => DateTime.Now.Year - BirthDay.Year;

    /// <summary>
    /// Номер телефона
    /// </summary>
    public string PhoneNumber { get; init; }
    
    /// <summary>
    /// Никнейм в телеграм
    /// </summary>
    public string Telegram { get; init; }
}