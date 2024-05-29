namespace Application.Dtos.Person;

/// <summary>
/// Дто запроса на  Person
/// </summary>
public abstract class PersonGetByIdRequest : PersonBaseDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; }
}
