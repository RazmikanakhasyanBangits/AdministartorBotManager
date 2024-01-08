using Telegram.Bot.Types.ReplyMarkups;

namespace Service.Abstraction;

public interface IUserActionService
{
    Task<bool> BlockUserAsync(string userName);
    Task<(InlineKeyboardMarkup, string)> GetUserDataAsync(string username);
    Task<InlineKeyboardMarkup> GetUsersAsync(string page = "1");
    Task ReStartBotAsync();
    Task StopBotAsync();
    Task<bool> UnblockUser(string userName);
}
