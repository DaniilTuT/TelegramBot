namespace Domain.ValueObjects;

/// <summary>
/// Ф.И.О.
/// </summary>
public class FullName
{
    public FullName(string firstName, string lastName, string middleName)
    {
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
    }
    /// <summary>
    /// Имя 
    /// </summary>
    public string FirstName { get; set; }
    /// <summary>
    /// Фамилия 
    /// </summary>
    public string LastName { get; set; }
    
    /// <summary>
    /// Может быть отчеством
    /// </summary>
    public string? MiddleName { get; set; } = null;
}