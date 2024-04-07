using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services;

public class PersonServices
{
    private readonly IPersonRepository _personRepository;
    
    public PersonServices(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }
    
    //TODO : Реализовать CRUD
    public PersonGetByIdResponse GetById(Guid id)
    {
        var person = _personRepository.GetById(id);
        
        
        //TODO: AutoMapper
        return person;
    }
}