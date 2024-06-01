using Application.Interfaces.Repositories;

namespace Infrastructure1.Jobs;

public class PersonFindBirthdaysJob : IJob
{
    private readonly IPersonRepository _personRepository;

    public PersonFindBirthdaysJob(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }
    
    public Task Execute(IJobExecuteContext context)
    { ///TODO: MAIN SUKA Сделать логику получения человека у которого сегодня в текущий день  день рождения и вывод на консолб
      
        var persons = _personRepository.GetAll();
        foreach (var person in  persons)
        {
            
        }
    }
}