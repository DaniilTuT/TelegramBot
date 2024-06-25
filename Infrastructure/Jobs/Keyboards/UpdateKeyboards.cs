using Domain.Entities;
using Domain.Primitives.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.Jobs.Keyboards;

public class UpdateKeyboards
{
    public static InlineKeyboardMarkup InlineKeyboardForUpdate(Person person)
    {
    return new InlineKeyboardMarkup(
        new List<InlineKeyboardButton[]>() 
        {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Изменить Ф.И.О.", 
                    string.Join(" ",new List<string> {((int)BotStates.UpdateFullName).ToString(), person.Id.ToString()})),
                InlineKeyboardButton.WithCallbackData("Изменить Пол", 
                    string.Join(" ",new List<string> {((int)BotStates.UpdateGender).ToString(), person.Id.ToString()}))
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Изменить Дату рождения", 
                    string.Join(" ",new List<string> {((int)BotStates.UpdateBirthDay).ToString(), person.Id.ToString()})),
                InlineKeyboardButton.WithCallbackData("Изменить Телефон", 
                    string.Join(" ",new List<string> {((int)BotStates.UpdatePhoneNumber).ToString(), person.Id.ToString()}))
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Изменить Тэг телеграм", 
                    string.Join(" ",new List<string> {((int)BotStates.UpdateTelegram).ToString(), person.Id.ToString()})),
            },
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Удалить запись", 
                    string.Join(" ",new List<string> {((int)BotStates.Delete).ToString(), person.Id.ToString()})),
            }
        });
    }

}