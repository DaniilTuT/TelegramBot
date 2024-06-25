using Domain.Entities;
using Telegram.Bot.Types.ReplyMarkups;

namespace Infrastructure.Jobs.Keyboards;

public static class GetKeyboard
{
    public static InlineKeyboardMarkup InlineKeyboardForGetAll(List<Person> persons)
    {
        var buttons = new List<InlineKeyboardButton[]>();

        int numberOfRows = (int)Math.Ceiling((double)persons.Count / 2.0);

        for (int i = 0; i < numberOfRows; i++)
        {
            var row = new List<InlineKeyboardButton>();

            for (int j = 0; j < 2; j++)
            {
                int index = i * 2 + j;
                if (index < persons.Count)
                {
                    row.Add(InlineKeyboardButton.WithCallbackData(
                        persons[index].FullName.FirstName + " " + persons[index].FullName.LastName,
                        persons[index].Id.ToString()));
                }
            }

            buttons.Add(row.ToArray());
        }

        return new InlineKeyboardMarkup(buttons);
    }
}