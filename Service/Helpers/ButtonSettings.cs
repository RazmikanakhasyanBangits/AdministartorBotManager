using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Service.Helpers
{
    public static class ButtonSettings
    {
        public static ReplyKeyboardMarkup ShowButtons()
        {
            return new(new[]
                   {
                     new KeyboardButton("/GetUsers"),
                     new KeyboardButton("/ReStartBot"),
                     new KeyboardButton("/StopBot"),
                     new KeyboardButton("/Help"),
                   })
            { ResizeKeyboard = true };
        }
    }
}
