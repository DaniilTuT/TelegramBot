using Domain.Entities;

namespace Application.Interfaces;

/// <summary>
/// Базовый репозиторий
/// </summary>
/// <typeparam name="TEntity">Сущность.</typeparam>
public interface IRepository<TEntity> where TEntity : BaseEntity
{
    /// <summary>
    /// Получение по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Сущность.</returns>
    public TEntity GetById(Guid id);
    
    /// <summary>
    /// Получение всех
    /// </summary>
    /// <returns>Список сущностей.</returns>
    public List<TEntity> Get();

    /// <summary>
    /// Создание
    /// </summary>
    /// <param name="entity">Сущность на добавление.</param>
    /// <returns>Созданная сущнсоть.</returns>
    public TEntity Create(TEntity entity);

    /// <summary>
    /// Обновление
    /// </summary>
    /// <param name="entity">Сущность на обновление.</param>
    /// <returns>Обновленная сущсноть.</returns>
    public TEntity Update(TEntity entity);

    /// <summary>
    /// Удаление
    /// </summary>
    /// <param name="id">Id удаляемой сущности</param>
    /// <returns>Результат удаления.</returns>
    public void Delete(Guid id);

    public void SaveChanges()
    {
        
    }
}