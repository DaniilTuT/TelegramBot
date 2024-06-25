using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.Jobs.Keyboards;

public class DeleteKeyboard
{
    public static InlineKeyboardMarkup inlineKeyboardForDelete = new InlineKeyboardMarkup(
        new List<InlineKeyboardButton[]>()
        {
            new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData("Да", "true"),
                InlineKeyboardButton.WithCallbackData("Нет", "false"),
            }
        });
}