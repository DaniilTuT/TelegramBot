namespace Application.Dtos.Person;

public abstract class PersonDeleteRequest: PersonBaseDto
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; init; }
}