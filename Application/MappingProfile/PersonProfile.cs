using Application.Dtos.Person;
using AutoMapper;
using Domain.Entities;
using Domain.ValueObjects;

namespace Application.MappingProfile;

/// <summary>
/// Маппинг для Person
/// </summary>
public class PersonMappingProfile : Profile
{
    public PersonMappingProfile()
    {
        CreateMap<Person, PersonGetByIdResponse>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => src.FullName));
        
        CreateMap<Person, PersonGetResponse>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => src.FullName));
            
        CreateMap<Person, PersonCreateResponse>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => src.FullName));
        
        CreateMap<Person, PersonUpdateResponse>()
            .ForMember(dest => dest.FullName,
                opt => opt.MapFrom(src => src.FullName));
        
        CreateMap<PersonCreateRequest, Person>()
            .ConstructUsing(dto => new Person(
                new FullName(dto.FullName.FirstName, dto.FullName.LastName, dto.FullName.MiddleName),
                dto.BirthDay,
                dto.PhoneNumber,
                dto.Telegram,
                dto.Gender));
        
        CreateMap<PersonUpdateRequest, Person>()
            .ConstructUsing(dto => new Person(
                dto.Id,
                new FullName(dto.FullName.FirstName, dto.FullName.LastName, dto.FullName.MiddleName),
                dto.BirthDay,
                dto.PhoneNumber,
                dto.Telegram,
                dto.Gender));
    }
}