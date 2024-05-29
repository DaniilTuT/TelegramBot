namespace Application.Dtos.Person;

/// <summary>
/// Дто ответа на запрос по индексу Person
/// </summary>
public abstract class PersonGetByIdResponse : PersonBaseDto
{
    public Guid id { get; init; } 
}