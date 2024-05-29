using Application.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Dal.Repositories;

public class PersonRepository: IPersonRepository
{
    private readonly TelegramBotDbContext _telegramBotDbContext;

    public PersonRepository(TelegramBotDbContext telegramBotDbContext)
    {
        _telegramBotDbContext = telegramBotDbContext;
    }

    public Person? GetById(Guid id)
    {
        
        var person=_telegramBotDbContext.Persons.FirstOrDefault(x => x.Id == id);
        return person;
    }

    public List<Person> Get()
    {
        var persons = _telegramBotDbContext.Persons.ToList();
        return persons;
    }

    public Person Create(Person entity)
    {
        _telegramBotDbContext.Persons.AddAsync(entity);
        _telegramBotDbContext.SaveChangesAsync();
        return entity;
    }

    public Person Update(Person entity)
    {
        _telegramBotDbContext.Entry(entity).State = EntityState.Modified;
        _telegramBotDbContext.SaveChangesAsync();
        return entity;
    }

    public void Delete(Guid id)
    {
        var entity =  _telegramBotDbContext.Persons.Find(id);
        _telegramBotDbContext.Persons.Remove(entity);
        _telegramBotDbContext.SaveChangesAsync();
    }

    public List<CustomField<string>> GetCustomFields()
    {
        throw new NotImplementedException();
    }
}