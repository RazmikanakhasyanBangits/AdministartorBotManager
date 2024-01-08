using ExchangeBot.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Abstraction;
using Service.Helpers;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Service.Implementation
{
    public class TelegramCommandHandler : IHostedService, ICommandHandler
    {
        private readonly string _token;
        private readonly TelegramBotClient Bot;
        private readonly IServiceScopeFactory _scopeFactory;
        private static int currentPage=1;
        private static string lastUserName;
        public TelegramCommandHandler(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _token = configuration["Token"];
            Bot = new TelegramBotClient(_token);
            _scopeFactory=scopeFactory;
        }


        private async void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            var callbackData = e.CallbackQuery.Data;
            var chatId = e.CallbackQuery.Message.Chat.Id;
            var messageId = e.CallbackQuery.Message.MessageId;
            using (var scope = _scopeFactory.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var userActionService = scopedServices.GetRequiredService<IUserActionService>();

                switch (callbackData.Split(".")[0])
                {
                    case "/BackToList":
                        await Bot.EditMessageReplyMarkupAsync(chatId, messageId, await userActionService.GetUsersAsync(currentPage.ToString()));
                        return;
                    case "/Block":
                        await userActionService.BlockUserAsync(lastUserName??callbackData.Split(".")[1]);
                        var blockedUserData = await userActionService.GetUserDataAsync(lastUserName??callbackData.Split(".")[1]);
                        await Bot.EditMessageReplyMarkupAsync(chatId, messageId, blockedUserData.Item1);
                        lastUserName= blockedUserData.Item2;
                        return;
                    case "/Unblock":
                        await userActionService.UnblockUser(lastUserName??callbackData.Split(".")[1]);
                        var unBlockedUserData = await userActionService.GetUserDataAsync(lastUserName??callbackData.Split(".")[1]);
                        await Bot.EditMessageReplyMarkupAsync(chatId, messageId, unBlockedUserData.Item1);
                        lastUserName= unBlockedUserData.Item2;
                        return;
                    default:
                        break;
                }
                if (!int.TryParse(callbackData, out int _))
                {
                    var userData = await userActionService.GetUserDataAsync(callbackData);
                    await Bot.EditMessageReplyMarkupAsync(chatId, messageId, userData.Item1);
                    lastUserName = userData.Item2;
                    return;
                }

                currentPage=int.Parse(callbackData);
                await Bot.EditMessageReplyMarkupAsync(chatId, messageId, await userActionService.GetUsersAsync(callbackData));
            }
        }
        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Type == MessageType.Text)
            {
                try
                {
                    string result = string.Empty;
                    string pattern = @"(\d+)([1-9]{3})([:]([1-9]{3})){0,1}";
                    Regex regex = new(pattern);
                    if (regex.IsMatch(e.Message.Text))
                    {

                        GroupCollection match = regex.Match(e.Message.Text).Groups;
                        string page = match[2].Value;
                        double amount = double.Parse(match[1].Value);
                        string pageSize = match[4].Value;
                        _ = await Bot.SendTextMessageAsync(e.Message.Chat.Id, "GetUsers(MethodNotImplemented)");
                        return;
                    }

                    switch (e.Message.Text)
                    {
                        case "/start":
                            ReplyKeyboardMarkup buttons = ButtonSettings.ShowButtons(e.Message.Chat.Id, (TelegramBotClient)sender);
                            buttons.ResizeKeyboard = true;
                            buttons.Selective = true;
                            _ = await ((TelegramBotClient)sender).SendTextMessageAsync(
                            e.Message.Chat.Id,
                            "Processing...",
                            replyMarkup: buttons);
                            _ = await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Done!");
                            break;
                        case "/GetUsers":
                            using (var scope = _scopeFactory.CreateScope())
                            {
                                var scopedServices = scope.ServiceProvider;
                                var userActionService = scopedServices.GetRequiredService<IUserActionService>();

                                _ = await Bot.SendTextMessageAsync(
                                   chatId: e.Message.Chat.Id,
                                   text: "Users List:",
                                   replyMarkup: await userActionService.GetUsersAsync());
                            }
                            break;
                        default:
                            using (var scope = _scopeFactory.CreateScope())
                            {
                                var scopedServices = scope.ServiceProvider;
                                var commandSwitcher = scopedServices.GetRequiredService<CommandSwitcher>();
                                commandSwitcher.Execute(e.Message.Text);
                                break;
                            }

                    }
                }
                catch (Exception ex)
                {
                    _ = await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Անհասկանալի հրաման, խնդրում ենք մուտքարգել հրամաններից որևիցե մեկը\n/all\n/allBest\n/available");
                }

            }
        }

        public void StartListen()
        {
            Bot.OnMessage += Bot_OnMessage;
            Bot.OnCallbackQuery +=Bot_OnCallbackQuery;
            Bot.StartReceiving();
            _ = Console.ReadLine();
            Bot.StopReceiving();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            Bot.OnMessage += Bot_OnMessage;
            Bot.OnCallbackQuery +=Bot_OnCallbackQuery;
            Bot.StartReceiving();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Bot.StopReceiving();
            return Task.CompletedTask;
        }
    }
}
