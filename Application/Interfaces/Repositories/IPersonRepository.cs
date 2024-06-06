using System.Collections;
using Domain.Entities;

namespace Application.Interfaces.Repositories;

/// <summary>
/// Репозиторий Person
/// </summary>
public interface IPersonRepository : IBaseRepository<Person>
{
    /// <summary>
    /// Получение кастомных полей
    /// </summary>
    public List<CustomField<string>> GetCustomFields();

    public List<Person> GetAllByBirthday();
}