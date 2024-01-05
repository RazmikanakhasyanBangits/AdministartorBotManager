using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Service.Helpers
{
    public static class ButtonSettings
    {
        public static ReplyKeyboardMarkup ShowButtons(long chatId, TelegramBotClient sender)
        {
            return new(new[]
                   {
                     new KeyboardButton("/all"),
                     new KeyboardButton("/allBest"),
                     new KeyboardButton("/available"),
                     new KeyboardButton("/location")
                   })
            { ResizeKeyboard = true };
        }
    }
}
