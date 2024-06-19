using Application.Interfaces.Repositories;
using Domain.Entities;
using Domain.Entities.ValueObjects;
using Domain.Primitives.Enums;
using Domain.Validations;
using Domain.Validations.Validators;
using Infrastructure.Jobs.Keyboards;
using Microsoft.Extensions.Options;
using Quartz;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Infrastructure.Jobs;

public class TelegramCreatePersonJob : IJob
{
    private readonly IPersonRepository _personRepository;
    private readonly TelegramSettings _telegramSettings;
    private readonly TelegramBotClient _telegramBotClient;

    private Dictionary<long,CreateStates> States { get; set; } = [];


    private FullName TmpFullName { get; set; }
    private Gender TmpGender { get; set; }
    private DateTime TmpBirthDay { get; set; }
    private string TmpPhoneNumber { get; set; }
    private string TmpTelegram { get; set; }


    public TelegramCreatePersonJob(IPersonRepository personRepository, IOptions<TelegramSettings> telegramSettings)
    {
        _personRepository = personRepository;
        _telegramSettings = telegramSettings.Value;
        _telegramBotClient = new TelegramBotClient(_telegramSettings.BotToken);
    }


    public async Task Execute(IJobExecutionContext context)
    {
        var _receiverOptions = new ReceiverOptions
        {
            AllowedUpdates =
                new[]
                {
                    UpdateType.Message, // Сообщения (текст, фото/видео, голосовые/видео сообщения и т.д.)
                    UpdateType.CallbackQuery
                },
            ThrowPendingUpdates = true,
        };

        using var cts = new CancellationTokenSource();

        _telegramBotClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token); // Запускаем бота

        var me = await _telegramBotClient
            .GetMeAsync(); // Создаем переменную, в которую помещаем информацию о нашем боте.
        Console.WriteLine($"{me.FirstName} запущен!");

        await Task.Delay(-1); // Устанавливаем бесконечную задержку, чтобы наш бот работал постоянно
    }


    private async Task UpdateHandler(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        try
        {
            switch (update.Type)
            {
                case UpdateType.Message:
                {
                    var message = update.Message;

                    var user = message.From;

                    Console.WriteLine($"{user.FirstName} ({user.Id}) написал сообщение: {message.Text}");

                    var chat = message.Chat;

                    switch (message.Type)
                    {
                        case MessageType.Text:
                        {
                            if (message.Text == "/stop")
                            {
                                States[chat.Id] = CreateStates.None;
                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Создание записи прекращено");
                                return;
                            }
                            
                            if (message.Text == "/start")
                            {
                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Приветствую");
                                States[chat.Id] = CreateStates.None;
                                return;
                            }

                            if (message.Text == "/add")
                            {
                                States[chat.Id] = CreateStates.None;
                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Заполните анкету");
                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Введите Имя, Фамилию и отчество через пробел");
                                States[chat.Id] = CreateStates.FullName;
                                return;
                            }

                            if (States[chat.Id] == CreateStates.FullName)
                            {
                                try
                                {
                                    string[] m = message.Text.Split(' ');
                                    TmpFullName = m.Length == 2
                                        ? new FullName(m[0], m[1], null)    
                                        : new FullName(m[0], m[1], m[2]);
                                    States[chat.Id] = CreateStates.Gender;
                                    await _telegramBotClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Выберите гендер",
                                        replyMarkup: CreateKeyboards.inlineKeyboardForGender);
                                    return;
                                }
                                catch 
                                {
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Данные введены неправильно");
                                    return;
                                }
                                
                            }

                            if (States[chat.Id] == CreateStates.Birthday)
                            {
                                try
                                {
                                    TmpBirthDay =  new BirthDayValidator(nameof(TmpBirthDay)).ValidateWithErrors(DateTime.Parse(message.Text));
                                    States[chat.Id] = CreateStates.PhoneNumber;
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Введите номер в формате \"+37377731226\"");
                                    return;
                                }
                                catch
                                {
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Дата введена некоректно, попробуйте ещё раз");
                                    return;
                                }
                            }

                            if (States[chat.Id] == CreateStates.PhoneNumber)
                            {
                                try
                                {
                                    TmpPhoneNumber = new PhoneNumberValidator(nameof(TmpPhoneNumber)).ValidateWithErrors(message.Text);
                                    States[chat.Id] = CreateStates.Telegram;
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Введите телеграм тэг. Например \"@SomeSimpleTag\"");
                                    return;
                                }
                                catch 
                                {
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Номер телефона введен некоректно, попробуйте ещё раз");
                                    return;
                                }
                            }

                            if (States[chat.Id] == CreateStates.Telegram)
                            {
                                try
                                {
                                    TmpTelegram = new TelegramValidator(nameof(TmpTelegram)).ValidateWithErrors(message.Text);
                                    try
                                    {
                                        _personRepository.Create(new Person(Guid.NewGuid(),TmpFullName,TmpGender,TmpBirthDay,TmpPhoneNumber,TmpTelegram));
                                        States[chat.Id] = CreateStates.None;
                                        new Person(Guid.NewGuid(),TmpFullName,TmpGender,TmpBirthDay,TmpPhoneNumber,TmpTelegram).ConsoleWriteLine();
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Все готово!");
                                        return;
                                    }
                                    catch
                                    {
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Не получается создать, попробуйте заново(");
                                        return;
                                    }
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Телегграм тэг введен некоректно, попробуйте ещё раз");
                                    return;
                                }

                                
                            }
                            
                            
                            
                            
                            else
                            {
                                await botClient.SendTextMessageAsync(chat.Id,
                                    "Команда не распознана, попробуй еще раз.");
                            }


                            return;
                        }

                        default:
                        {
                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                "Используй только текст!");
                            return;
                        }
                    }

                    return;
                }
                case UpdateType.CallbackQuery:
                {
                    var callbackQuery = update.CallbackQuery;

                    var user = callbackQuery.From;

                    Console.WriteLine($"{user.FirstName} ({user.Id}) нажал на кнопку: {callbackQuery.Data}");
                    
                    var chat = callbackQuery.Message.Chat;

                    if (States[chat.Id] == CreateStates.Gender)
                    {
                        TmpGender = callbackQuery.Data == "Male" ? Gender.Male : Gender.Female;
                        States[chat.Id] = CreateStates.Birthday;
                        await botClient.SendTextMessageAsync(
                            chat.Id,
                            "Введите дату рождения в формате дд.мм.гггг");
                        return;
                    }
                    
                    
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
    {
        var ErrorMessage = error switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => error.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }
}