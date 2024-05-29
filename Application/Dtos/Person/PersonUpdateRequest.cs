namespace Application.Dtos.Person;

public abstract class PersonUpdateRequest: PersonBaseDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; }
}