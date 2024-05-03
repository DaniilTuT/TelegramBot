using Application.Dtos;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

public class PersonServices
{
    private readonly IPersonRepository _personRepository;
    
    public PersonServices(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }
    
    public PersonGetByIdResponse GetById(Guid id)
    {
        var person = _personRepository.GetById(id);
        
        var config = new MapperConfiguration(cfg => cfg.CreateMap<Person, PersonGetByIdResponse>());
        
        var mapper = new Mapper(config);

        var personDto = mapper.Map<PersonGetByIdResponse>(person);
        
        return personDto;
    }
    public List<PersonGetResponse> Get()
    {
        var persons = _personRepository.Get();
        
        var config = new MapperConfiguration(cfg => cfg.CreateMap<Person, PersonGetResponse>());
        
        var mapper = new Mapper(config);

        return persons.Select(person => mapper.Map<PersonGetResponse>(person)).ToList();
    }
    
    public PersonCreateResponse Create(Person person)
    {
        _personRepository.Create(person);
        
        var config = new MapperConfiguration(cfg => cfg.CreateMap<Person, PersonCreateResponse>());
        
        var mapper = new Mapper(config);

        var personDto = mapper.Map<PersonCreateResponse>(person);
        
        return personDto;
    }

    public PersonUpdateResponse Update(PersonUpdateRequest personUpdateRequest)
    {
        var person = _personRepository.GetById(personUpdateRequest.Id);


        person.Update(personUpdateRequest.FirstName,
            personUpdateRequest.LastName,
            personUpdateRequest.MiddleName, 
            personUpdateRequest.PhoneNumber);

        _personRepository.Update(person);

        var config = new MapperConfiguration(cfg => cfg.CreateMap<Person, PersonUpdateResponse>());
        
        var mapper = new Mapper(config);

        var response = mapper.Map<PersonUpdateResponse>(person);
        
        return response;
    }

    public void Delete(Guid id)
    {
        _personRepository.Delete(id);
    }
    
    
}

