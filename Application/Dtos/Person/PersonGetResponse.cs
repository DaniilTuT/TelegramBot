namespace Application.Dtos.Person;

public abstract class PersonGetResponse: PersonBaseDto
{
    public Guid id { get; init; } 
}