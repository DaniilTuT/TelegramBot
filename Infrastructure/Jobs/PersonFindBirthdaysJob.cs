using Application.Interfaces.Repositories;
using Domain.Primitives.Enums;
using Microsoft.Extensions.Options;
using Quartz;
using Telegram.Bot;

namespace Infrastructure.Jobs;

public class PersonFindBirthdaysJob : IJob
{
    private readonly IPersonRepository _personRepository;
    private readonly TelegramSettings _telegramSettings;
    private readonly TelegramBotClient _telegramBotClient;
    

    public PersonFindBirthdaysJob(IPersonRepository personRepository, IOptions<TelegramSettings> telegramSettings)
    {
        _personRepository = personRepository;
        _telegramSettings = telegramSettings.Value;
        _telegramBotClient = new TelegramBotClient(_telegramSettings.BotToken);
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        var persons = _personRepository.GetAllByBirthday();
        Console.WriteLine(persons);
        foreach (var person in  persons)
        {
            await _telegramBotClient.SendTextMessageAsync(
                person.ChatId, 
                @$"
                Доброе утро!! 
                {"\n"}Сегодня ваш контакт {person.FullName.FirstName} {person.FullName.LastName} празднует день рождения!!
                {"\n"}Сегодня {(person.Gender == Gender.Male ? "он" : "она")} празднует свой {person.Age}й день рождения!! 
                {"\n"}Вы можете позвонить ему по номеру: {person.PhoneNumber}
                {"\n"}Или написать ему в телеграм: {person.Telegram}");
        }
    }
}