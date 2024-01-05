using ExchangeBot.Abstraction;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Text.RegularExpressions;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Service.Implementation
{
    public class TelegramCommandHandler : BackgroundService, ICommandHandler
    {
        private readonly string _token;
        private readonly TelegramBotClient Bot;

        public TelegramCommandHandler(IConfiguration configuration)
        {
            _token = configuration["Token"];
            Bot = new TelegramBotClient(_token);
        }

        [Obsolete]
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Get();
                await Task.WhenAll();
                return;
            }
        }
        [Obsolete]
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
                    if (e.Message.Text == "/start")
                    {
                        // Create an inline keyboard with styled buttons.
                        InlineKeyboardMarkup inlineKeyboard = new(new[]
                        {
                new[]
                {
                    InlineKeyboardButton.WithCallbackData("🔴 Red Button", "redButton"),
                    InlineKeyboardButton.WithCallbackData("🌏 Blue Button", "blueButton")
                },
                new[]
                {
                    InlineKeyboardButton.WithUrl("Visit Google", "https://www.google.com")
                }
            });

                        // Send a message with the inline keyboard.
                        _ = await Bot.SendTextMessageAsync(
                            chatId: e.Message.Chat.Id,
                            text: "Choose an option:",
                            replyMarkup: inlineKeyboard
                        );
                    }
                    /*switch (e.Message.Text)
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
                        default:
                             var a = CommandSwitcher.CommandDictionary[e.Message.Text].Invoke("RazmikAnakhasyan");
                             _ = await Bot.SendTextMessageAsync(e.Message.Chat.Id, result);
                            break;
                    }*/

                }
                catch (Exception)
                {
                    _ = await Bot.SendTextMessageAsync(e.Message.Chat.Id, "Անհասկանալի հրաման, խնդրում ենք մուտքարգել հրամաններից որևիցե մեկը\n/all\n/allBest\n/available");
                }

            }
        }

        [Obsolete]
        public void Get()
        {
            Bot.OnMessage += Bot_OnMessage;
            Bot.StartReceiving();
            _ = Console.ReadLine();
            Bot.StopReceiving();
        }

    }
}
