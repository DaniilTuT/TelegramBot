namespace Application.Dtos.Person;

public abstract class PersonUpdateResponse : PersonBaseDto
{
    public Guid id { get; init; } 
}