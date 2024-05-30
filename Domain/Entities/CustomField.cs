namespace Domain.Entities;

/// <summary>
/// Класс для кастомных полей
/// </summary>
public class CustomField<TType> : BaseEntity
{
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Значение
    /// </summary>
    public TType Value { get; set; }

    private CustomField()
    {
    }
}