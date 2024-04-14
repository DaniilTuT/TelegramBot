namespace Application.Dtos;

public class PersonCreateResponse
{
    
    public Guid Id { get; set; }
    
    public DateTime BirthDay { get; set; }
    
    
    public int Age => (DateTime.Now.Month - BirthDay.Month >= 0 && DateTime.Now.Day - BirthDay.Day >= 0)
        ? DateTime.Now.Year - BirthDay.Year
        : DateTime.Now.Year - BirthDay.Year - 1;
   
    
    public string PhoneNumber { get; set; }
    
    
    public string Telegram { get; set; }
}