using Grpc.Client.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Abstraction;
using Service.Helpers;
using Service.Model;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Service.Implementation
{
    public class TelegramCommandHandler : IHostedService
    {
        private readonly string _token;
        private readonly TelegramBotClient Bot;
        private readonly IServiceScopeFactory _scopeFactory;
        private static int currentPage = 1;
        private static string lastUserId;
        private readonly ReceiverOptions receiverOptions;

        public TelegramCommandHandler(IConfiguration configuration, IServiceScopeFactory scopeFactory)
        {
            _token = configuration["Token"];
            Bot = new TelegramBotClient(_token);
            _scopeFactory = scopeFactory;
        }

        private async Task Bot_OnCallbackQuery(Update e)
        {
            string callbackData = e.CallbackQuery.Data;
            long chatId = e.CallbackQuery.Message.Chat.Id;
            int messageId = e.CallbackQuery.Message.MessageId;
            using IServiceScope scope = _scopeFactory.CreateScope();
            IServiceProvider scopedServices = scope.ServiceProvider;
            IUserActionService userActionService = scopedServices.GetRequiredService<IUserActionService>();

            try
            {
                switch (callbackData.Split(".")[0])
                {
                    case "/BackToList":
                        await Bot.EditMessageReplyMarkupAsync(chatId, messageId, await userActionService.GetUsersAsync(currentPage.ToString()));
                        return;
                    case "/Block":
                        await userActionService.BlockUserAsync(lastUserId ?? callbackData.Split(".")[1]);
                        (InlineKeyboardMarkup, string) blockedUserData = await userActionService.GetUserDataAsync(lastUserId ?? callbackData.Split(".")[1]);
                        await Bot.EditMessageReplyMarkupAsync(chatId, messageId, blockedUserData.Item1);
                        lastUserId = blockedUserData.Item2;
                        return;
                    case "/Unblock":
                        _ = await userActionService.UnblockUser(lastUserId ?? callbackData.Split(".")[1]);
                        (InlineKeyboardMarkup, string) unBlockedUserData = await userActionService.GetUserDataAsync(lastUserId ?? callbackData.Split(".")[1]);
                        await Bot.EditMessageReplyMarkupAsync(chatId, messageId, unBlockedUserData.Item1);
                        lastUserId = unBlockedUserData.Item2;
                        return;
                    case "UserId":
                        (InlineKeyboardMarkup, string) userData = await userActionService.GetUserDataAsync(callbackData.Split(".")[1]);
                        await Bot.EditMessageReplyMarkupAsync(chatId, messageId, userData.Item1);
                        lastUserId = userData.Item2;
                        return;
                    default:
                        break;
                }

                if (int.TryParse(callbackData, out int _))
                {
                    _ = await Bot.EditMessageReplyMarkupAsync(chatId, messageId, await userActionService.GetUsersAsync(callbackData));
                    currentPage = int.Parse(callbackData);
                }
            }
            catch
            {
                //Ignore
            }
        }

        private async Task Bot_OnMessage(Update update)
        {
            if (update.Message.Type == MessageType.Text)
            {
                try
                {
                    switch (update.Message.Text.ToLower())
                    {
                        case "/start":
                            ReplyKeyboardMarkup buttons = ButtonSettings.ShowButtons();
                            buttons.ResizeKeyboard = true;
                            buttons.Selective = true;
                            _ = await Bot.SendTextMessageAsync(
                            update.Message.Chat.Id,
                            "Processing...",
                            replyMarkup: buttons);
                            _ = await Bot.SendTextMessageAsync(update.Message.Chat.Id, "Done!");
                            break;
                        case "/get_users":
                            using (IServiceScope scope = _scopeFactory.CreateScope())
                            {
                                IServiceProvider scopedServices = scope.ServiceProvider;
                                IUserActionService userActionService = scopedServices.GetRequiredService<IUserActionService>();

                                _ = await Bot.SendTextMessageAsync(
                                   chatId: update.Message.Chat.Id,
                                   text: "Users List:",
                                   replyMarkup: await userActionService.GetUsersAsync());
                            }
                            break;
                        case "/update_rates":
                            using (IServiceScope scope = _scopeFactory.CreateScope())
                            {
                                IServiceProvider scopedServices = scope.ServiceProvider;
                                IRateActionClient rateActionClient = scopedServices.GetRequiredService<IRateActionClient>();
                                await rateActionClient.UpdateRatesAsync();
                                await Bot.SendTextMessageAsync(update.Message.Chat.Id, "Done!");
                            }
                            break;
                        default:
                            using (IServiceScope scope = _scopeFactory.CreateScope())
                            {
                                IServiceProvider scopedServices = scope.ServiceProvider;
                                CommandSwitcher commandSwitcher = scopedServices.GetRequiredService<CommandSwitcher>();
                                commandSwitcher.Execute(update.Message.Text);
                                break;
                            }

                    }
                }
                catch
                {
                    //ignore
                }

            }
        }

        private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken token)
        {

            try
            {
                switch (update.Type)
                {
                    case UpdateType.CallbackQuery:
                        await Bot_OnCallbackQuery(update);
                        break;
                    case UpdateType.Message:
                        await Bot_OnMessage(update);
                        break;
                }
            }
            catch
            {
                Bot.StartReceiving(
                 updateHandler: HandleUpdateAsync,
                 pollingErrorHandler: HandlePollingErrorAsync,
                 receiverOptions: receiverOptions,
                 cancellationToken: CancellationToken.None);

                await Bot.GetMeAsync();
            }

        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Bot.StartReceiving(
                 updateHandler: HandleUpdateAsync,
                 pollingErrorHandler: HandlePollingErrorAsync,
                 receiverOptions: receiverOptions,
                 cancellationToken: CancellationToken.None);
            var commands = new List<BotCommand>
        {
            new BotCommand() { Command=nameof(StaticCommandsCollection.get_users), Description= StaticCommandsCollection.get_users },
            new BotCommand() { Command=nameof(StaticCommandsCollection.update_rates), Description= StaticCommandsCollection.update_rates },
            new BotCommand() { Command=nameof(StaticCommandsCollection.update_locations), Description= StaticCommandsCollection.update_locations }
        };
            await BotCommandHelper.SetBotCommandsAsync(Bot);
            await Bot.GetMeAsync();
            Console.WriteLine($"Start listening for @");
            Console.WriteLine();
        }

        private async Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            await Console.Out.WriteLineAsync(exception.Message);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
