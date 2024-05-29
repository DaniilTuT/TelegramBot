using Application.Dtos.Person;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Api;
[Route("api/[controller]")]
[ApiController]
public class PersonController:ControllerBase
{
    [HttpGet]
    public IActionResult Get([FromServices] PersonServices personService)
    {
        var persons = personService.Get();
        return Ok(persons);
    }
    
    [HttpGet("GetById")]
    public IActionResult GetById([FromBody] PersonDeleteRequest request ,[FromServices] PersonServices personService)
    {
        var person = personService.GetById(request.Id);
        if (person == null)
            return NotFound();

        return Ok(person);
    }
    
    [HttpPost("Add")]
    public IActionResult Create([FromBody] PersonCreateRequest personDto, [FromServices] PersonServices personService)
    {
        personService.Create(personDto);
        return Ok();
    }
    
    [HttpPut("Update")]
    public IActionResult Update([FromBody] PersonUpdateRequest personUpdateRequest, [FromServices] PersonServices personService)
    {
        var updatedPerson = personService.Update(personUpdateRequest);
        return Ok(updatedPerson);
    }

    [HttpDelete("Delete")]
    public IActionResult Delete([FromRoute] Guid id, [FromServices] PersonServices personService)
    {
        personService.Delete(id);
        return Ok();
    }
}