namespace Application.Dtos;

public class PersonUpdateRequest
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Имя 
    /// </summary>
    public string? FirstName { get; set; }
    
    /// <summary>
    /// Фамилия 
    /// </summary>
    public string? LastName { get; set; }
    
    /// <summary>
    /// Может быть отчеством
    /// </summary>
    public string? MiddleName { get; set; } = null;
    
    // <summary>
    /// Номер телефона
    /// </summary>
    public string? PhoneNumber { get; set; }

}