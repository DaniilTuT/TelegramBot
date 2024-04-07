using System.Diagnostics;

namespace Domain.Entities;

/// <summary>
/// Базовый класс для всех сущностей
/// </summary>

public abstract class BaseEntity
{
    
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }
        if (obj is not BaseEntity entity)
        {
            return false;
        }

        if (ReferenceEquals(this, obj)) return true;
        if (entity.Id != this.Id)
        {
            return false;
        }

        return true;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }
}