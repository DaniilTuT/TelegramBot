using Application.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Dal.EntityFramework;
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
       var person=_telegramBotDbContext.persons.FirstOrDefault(x => x.Id == id);
       return person;
    }

    public List<Person> GetAll()
    {
        var persons = _telegramBotDbContext.persons.ToList();
       return persons;
    }

    public void Create(Person entity)
    {
        _telegramBotDbContext.persons.AddAsync(entity);
        _telegramBotDbContext.SaveChangesAsync();
    }

    public bool Update(Person entity)
    {
        _telegramBotDbContext.Entry(entity).State = EntityState.Modified;
        _telegramBotDbContext.SaveChangesAsync();
        return true;
    }

    public void Delete(Person entity)
    {
        var entitys =  _telegramBotDbContext.persons.Find(entity.Id);
        _telegramBotDbContext.persons.Remove(entitys);
        _telegramBotDbContext.SaveChangesAsync();
    }

    public List<CustomField<string>> GetCustomFields()
    {
        throw new NotImplementedException();
    }
}