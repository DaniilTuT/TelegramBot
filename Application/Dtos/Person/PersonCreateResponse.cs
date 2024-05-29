namespace Application.Dtos.Person;

/// <summary>
/// Дто ответа на создание Person
/// </summary>
public abstract class PersonCreateResponse : PersonBaseDto
{
    public Guid id { get; init; } 
}