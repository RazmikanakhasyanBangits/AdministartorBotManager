using Service.Model.StaticModels;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Service.Helpers;

public static class BotCommandHelper
{

    public static async Task  SetBotCommandsAsync(TelegramBotClient bot)
    {
        var commands = new List<BotCommand>
        {
            new BotCommand() { Command=nameof(StaticCommandsCollection.get_users), Description= StaticCommandsCollection.get_users },
            new BotCommand() { Command=nameof(StaticCommandsCollection.update_rates), Description= StaticCommandsCollection.update_rates },
            new BotCommand() { Command=nameof(StaticCommandsCollection.update_locations), Description= StaticCommandsCollection.update_locations}
        };
        await bot.SetMyCommandsAsync(commands);
    }

}
