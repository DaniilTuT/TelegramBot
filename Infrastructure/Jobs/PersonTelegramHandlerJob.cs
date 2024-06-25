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

public class PersonTelegramHandlerJob : IJob
{
    private readonly IPersonRepository _personRepository;
    private readonly TelegramSettings _telegramSettings;
    private readonly TelegramBotClient _telegramBotClient;

    private Dictionary<long, BotStates> States { get; set; } = [];
    private Dictionary<long, Guid> Id { get; set; } = [];
    private Dictionary<long, int> LastBotMessage { get; set; } = [];


    private FullName TmpFullName { get; set; }
    private Gender TmpGender { get; set; }
    private DateTime TmpBirthDay { get; set; }
    private string TmpPhoneNumber { get; set; }
    private string TmpTelegram { get; set; }


    public PersonTelegramHandlerJob(IPersonRepository personRepository, IOptions<TelegramSettings> telegramSettings)
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
                    UpdateType.Message, 
                    UpdateType.CallbackQuery
                },
            ThrowPendingUpdates = true,
        };

        using var cts = new CancellationTokenSource();

        _telegramBotClient.StartReceiving(UpdateHandler, ErrorHandler, _receiverOptions, cts.Token);

        var me = await _telegramBotClient
            .GetMeAsync(); 
        Console.WriteLine($"{me.FirstName} запущен!");

        await Task.Delay(-1); 
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
                                if (States[chat.Id] !=  BotStates.None) {
                                    States[chat.Id] = BotStates.None;
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Создание записи прекращено.");
                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Вы не создаете запись.");
                                }
                                
                                return;
                            }

                            if (message.Text == "/start")
                            {
                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Приветствую");
                                States[chat.Id] = BotStates.None;
                                return;
                            }

                            if (message.Text == "/add")
                            {
                                States[chat.Id] = BotStates.None;
                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Заполните анкету");
                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Введите Имя, Фамилию и отчество через пробел");
                                States[chat.Id] = BotStates.CreateFullName;
                                return;
                            }

                            if (States[chat.Id] == BotStates.CreateFullName ||
                                States[chat.Id] == BotStates.UpdateFullName)
                            {
                                try
                                {
                                    string[] m = message.Text.Split(' ');
                                    TmpFullName = m.Length == 2
                                        ? new FullName(m[0], m[1], null)
                                        : new FullName(m[0], m[1], m[2]);
                                    if (States[chat.Id] == BotStates.CreateFullName)
                                    {
                                        States[chat.Id] = BotStates.CreateGender;
                                        await _telegramBotClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Выберите гендер",
                                            replyMarkup: CreateKeyboards.inlineKeyboardForGender);
                                    }
                                    else
                                    {
                                        var person = _personRepository.GetById(Id[chat.Id]);
                                        _personRepository.Update(person.Update(TmpFullName.FirstName,
                                            TmpFullName.LastName, TmpFullName.MiddleName, person.PhoneNumber,
                                            person.Gender, person.BirthDay, person.Telegram));
                                        await botClient.EditMessageTextAsync(chat.Id, LastBotMessage[chat.Id],
                                            person.ToString(),
                                            replyMarkup: UpdateKeyboards.InlineKeyboardForUpdate(person));
                                        States[chat.Id] = BotStates.None;
                                    }

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

                            if (States[chat.Id] == BotStates.CreateBirthday ||
                                States[chat.Id] == BotStates.UpdateBirthDay)
                            {
                                try
                                {
                                    TmpBirthDay =
                                        new BirthDayValidator(nameof(TmpBirthDay)).ValidateWithErrors(
                                            DateTime.Parse(message.Text));
                                    if (States[chat.Id] == BotStates.CreateBirthday)
                                    {
                                        States[chat.Id] = BotStates.CreatePhoneNumber;
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Введите номер в формате \"+37377731226\"");
                                    }
                                    else
                                    {
                                        var person = _personRepository.GetById(Id[chat.Id]);
                                        _personRepository.Update(person.Update(person.FullName.FirstName,
                                            person.FullName.LastName, person.FullName.MiddleName, person.PhoneNumber,
                                            person.Gender, TmpBirthDay, person.Telegram));
                                        await botClient.EditMessageTextAsync(chat.Id, LastBotMessage[chat.Id],
                                            person.ToString(),
                                            replyMarkup: UpdateKeyboards.InlineKeyboardForUpdate(person));
                                        States[chat.Id] = BotStates.None;
                                    }

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

                            if (States[chat.Id] == BotStates.CreatePhoneNumber ||
                                States[chat.Id] == BotStates.UpdatePhoneNumber)
                            {
                                try
                                {
                                    TmpPhoneNumber =
                                        new PhoneNumberValidator(nameof(TmpPhoneNumber)).ValidateWithErrors(
                                            message.Text);

                                    if (States[chat.Id] == BotStates.CreatePhoneNumber)
                                    {
                                        States[chat.Id] = BotStates.CreateTelegram;
                                        await botClient.SendTextMessageAsync(
                                            chat.Id,
                                            "Введите телеграм тэг. Например \"@SomeSimpleTag\"");
                                    }
                                    else
                                    {
                                        var person = _personRepository.GetById(Id[chat.Id]);
                                        _personRepository.Update(person.Update(person.FullName.FirstName,
                                            person.FullName.LastName, person.FullName.MiddleName, TmpPhoneNumber,
                                            person.Gender, person.BirthDay, person.Telegram));
                                        await botClient.EditMessageTextAsync(chat.Id, LastBotMessage[chat.Id],
                                            person.ToString(),
                                            replyMarkup: UpdateKeyboards.InlineKeyboardForUpdate(person));
                                        States[chat.Id] = BotStates.None;
                                    }

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

                            if (States[chat.Id] == BotStates.CreateTelegram ||
                                States[chat.Id] == BotStates.UpdateTelegram)
                            {
                                try
                                {
                                    TmpTelegram =
                                        new TelegramValidator(nameof(TmpTelegram)).ValidateWithErrors(message.Text);
                                    if (States[chat.Id] == BotStates.CreateTelegram)
                                    {
                                        try
                                        {
                                            _personRepository.Create(new Person(Guid.NewGuid(), TmpFullName, TmpGender,
                                                TmpBirthDay, TmpPhoneNumber, TmpTelegram, chat.Id));
                                            States[chat.Id] = BotStates.None;
                                            new Person(Guid.NewGuid(), TmpFullName, TmpGender, TmpBirthDay,
                                                TmpPhoneNumber,
                                                TmpTelegram, chat.Id).ConsoleWriteLine();
                                            await botClient.SendTextMessageAsync(
                                                chat.Id,
                                                "Все готово! \nСписок ваших записей вы можете посмотреть с помощью команды /all");
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
                                    else
                                    {
                                        var person = _personRepository.GetById(Id[chat.Id]);
                                        _personRepository.Update(person.Update(person.FullName.FirstName,
                                            person.FullName.LastName, person.FullName.MiddleName, person.PhoneNumber,
                                            person.Gender, person.BirthDay, TmpTelegram));
                                        await botClient.EditMessageTextAsync(chat.Id, LastBotMessage[chat.Id],
                                            person.ToString(),
                                            replyMarkup: UpdateKeyboards.InlineKeyboardForUpdate(person));
                                        States[chat.Id] = BotStates.None;
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


                            if (message.Text == "/all")
                            {
                                var persons = _personRepository.GetAll().Where(p => p.ChatId == chat.Id).ToList();
                                if (persons.Count != 0)
                                {
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "Вот список ваших контактов",
                                        replyMarkup: GetKeyboard.InlineKeyboardForGetAll(persons)
                                    );
                                }
                                else
                                {
                                    await botClient.SendTextMessageAsync(
                                        chat.Id,
                                        "У вас еще нет записей, вы можете создать их, используя команду /add");
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

                    if (States[chat.Id] == BotStates.CreateGender || States[chat.Id] == BotStates.UpdateGender)
                    {
                        TmpGender = callbackQuery.Data == "Male" ? Gender.Male : Gender.Female;
                        if (States[chat.Id] == BotStates.CreateGender)
                        {
                            States[chat.Id] = BotStates.CreateBirthday;
                            await botClient.SendTextMessageAsync(
                                chat.Id,
                                "Введите дату рождения в формате дд.мм.гггг");
                        }
                        else
                        {
                            var person = _personRepository.GetById(Id[chat.Id]);
                            _personRepository.Update(person.Update(person.FullName.FirstName,
                                person.FullName.LastName, person.FullName.MiddleName, person.PhoneNumber,
                                TmpGender, person.BirthDay, person.Telegram));
                            await botClient.EditMessageTextAsync(chat.Id, LastBotMessage[chat.Id],
                                person.ToString(),
                                replyMarkup: UpdateKeyboards.InlineKeyboardForUpdate(person));
                            States[chat.Id] = BotStates.None;
                        }

                        return;
                    }

                    if (callbackQuery.Data.Length == 36)
                    {
                        try
                        {
                            var id = new Guid(callbackQuery.Data);
                            var person = _personRepository.GetById(id);
                            var mess = await botClient.SendTextMessageAsync(
                                chat.Id,
                                person.ToString(),
                                replyMarkup: UpdateKeyboards.InlineKeyboardForUpdate(person));
                            LastBotMessage[chat.Id] = mess.MessageId;
                            return;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
                        }
                    }


                    if (callbackQuery.Data.Split(" ").Length == 2)
                    {
                        States[chat.Id] = (BotStates)(int.Parse(callbackQuery.Data.Split(" ")[0]));
                        Id[chat.Id] = new Guid(callbackQuery.Data.Split(" ")[1]);
                        switch (callbackQuery.Data.Split(" ")[0])
                        {
                            case "7":
                            {
                                await botClient.SendTextMessageAsync(chat.Id,
                                    "Введите Имя, Фамилию и отчество через пробел");
                                return;
                            }
                            case "8":
                            {
                                await botClient.SendTextMessageAsync(chat.Id,
                                    "Выберите гендер",
                                    replyMarkup: CreateKeyboards.inlineKeyboardForGender);
                                return;
                            }
                            case "9":
                            {
                                await botClient.SendTextMessageAsync(chat.Id,
                                    "Введите дату рождения в формате дд.мм.гггг");
                                return;
                            }
                            case "10":
                            {
                                await botClient.SendTextMessageAsync(chat.Id,
                                    "Введите номер в формате \"+37377731226\"");
                                return;
                            }
                            case "11":
                            {
                                await botClient.SendTextMessageAsync(chat.Id,
                                    "Введите телеграм тэг. Например \"@SomeSimpleTag\"");
                                return;
                            }
                            case "12":
                            {
                                Console.WriteLine("rberbebefbefb");
                                await botClient.SendTextMessageAsync(
                                    chat.Id,
                                    "Вы уверены что хотите удалить запись.",
                                    replyMarkup: DeleteKeyboard.inlineKeyboardForDelete);
                                return;
                            }
                        }
                    }

                    if (Boolean.Parse(callbackQuery.Data))
                    {
                        var person = _personRepository.GetById(Id[chat.Id]);
                        _personRepository.Delete(person);
                        await botClient.DeleteMessageAsync(chat.Id,LastBotMessage[chat.Id]);
                        await botClient.SendTextMessageAsync(
                            chat.Id,
                            "Запись успешно удалена");
                        States[chat.Id] = BotStates.None;
                    }
                    else
                    {
                        States[chat.Id] = BotStates.None;
                    }

                    if (callbackQuery.Data.Split(" ")[0] == ((int)BotStates.UpdateFullName).ToString())
                    {
                        States[chat.Id] = BotStates.UpdateFullName;
                        Id[chat.Id] = new Guid(callbackQuery.Data.Split(" ")[1]);
                        await botClient.SendTextMessageAsync(chat.Id,
                            "Введите Имя, Фамилию и отчество через пробел");
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