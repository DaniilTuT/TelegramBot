using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.Jobs.Keyboards;

public class CreateKeyboards
{
    public static InlineKeyboardMarkup inlineKeyboardForGender = new InlineKeyboardMarkup(
        new List<InlineKeyboardButton[]>()
        {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Male", "Male"),
                InlineKeyboardButton.WithCallbackData("Female", "Female"),
            }
        });
}