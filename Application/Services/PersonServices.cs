using Application.Dtos.Person;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.Services;

/// <summary>
/// Сервис для Person
/// </summary>
public class PersonServices
{
    private readonly IPersonRepository _personRepository;
    private readonly IMapper _mapper;

    
    public PersonServices(IPersonRepository personRepository, IMapper mapper)
    {
        _personRepository = personRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Получение Person
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Персона.</returns>
    public PersonGetByIdResponse GetById(Guid id)
    {
        var person = GetByIdOrThrow(id);
        return _mapper.Map<PersonGetByIdResponse>(person);
    }

    /// <summary>
    /// Получение всех Person
    /// </summary>
    /// <returns>Список всех Person.</returns>
    public List<PersonGetResponse> Get()
    {
        var persons = _personRepository.Get();
        return _mapper.Map<List<PersonGetResponse>>(persons);
    }

    /// <summary>
    /// Создание Person
    /// </summary>
    /// <param name="personCreateRequest">Person на создание.</param>
    /// <returns>Созданный Person.</returns>
    public PersonCreateResponse Create(PersonCreateRequest personCreateRequest)
    {
        var person = _mapper.Map<Person>(personCreateRequest);
         _personRepository.Create(person);

        return _mapper.Map<PersonCreateResponse>(person);
    }

    /// <summary>
    /// Обновление Person
    /// </summary>
    /// <param name="personUpdateRequest">Person на обновление.</param>
    /// <returns>Обновленный Person.</returns>
    public PersonUpdateResponse Update(PersonUpdateRequest personUpdateRequest)
    {
        var person = GetByIdOrThrow(personUpdateRequest.Id);

        person.Update(
            personUpdateRequest.FullName.FirstName,
            personUpdateRequest.FullName.LastName,
            personUpdateRequest.FullName.MiddleName,
            personUpdateRequest.PhoneNumber,
            personUpdateRequest.Gender,
            personUpdateRequest.BirthDay,
            personUpdateRequest.Telegram);
        
        _personRepository.Update(person);
        
        return _mapper.Map<PersonUpdateResponse>(person);
    }

    /// <summary>
    /// Удаление Person
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    public void Delete(Guid id)
    {
        var person = GetByIdOrThrow(id);
        _personRepository.Delete(person.Id);
    }

    /// <summary>
    /// Метод проверки на наличие объекта
    /// </summary>
    /// <param name="id">Идентификатор.</param>
    /// <returns>Person.</returns>
    private Person GetByIdOrThrow(Guid id)
    {
        var person = _personRepository.GetById(id);
        if (person == null)
        {
            throw new Exception();
        }

        return person;
    }
}