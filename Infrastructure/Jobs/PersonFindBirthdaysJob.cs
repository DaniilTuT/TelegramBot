using Application.Interfaces.Repositories;
using Quartz;

namespace Infrastructure1.Jobs;

public class PersonFindBirthdaysJob : IJob
{
    private readonly IPersonRepository _personRepository;

    public PersonFindBirthdaysJob(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }
    
    public Task Execute(IJobExecutionContext context)
    {
        var persons = _personRepository.GetAllByBirthday();
        foreach (var person in  persons)
        {
            person.ConsoleWriteLine();
        }
        return Task.CompletedTask;
    }
}