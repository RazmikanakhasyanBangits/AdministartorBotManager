using Grpc.Client.Abstraction;
using Repository.Abstraction;
using Service.Abstraction;
using Shared.Enum;
using Telegram.Bot.Types.ReplyMarkups;
using UserActionsProto;

namespace Service.Implementation;

public class UserActionService : IUserActionService
{
    private readonly IUserActionsClient _userActions;
    private readonly IUserRepository _userRepository;
    public UserActionService(IUserActionsClient userActions, IUserRepository userRepository)
    {
        _userActions = userActions;
        this._userRepository=userRepository;
    }

    public async Task<(InlineKeyboardMarkup,string)> GetUserDataAsync(string username)
    {
        var user = await _userRepository.GetDetailsAsync(x => x.UserExternalId.ToString() == username);
        List<List<InlineKeyboardButton>> buttonRows = new List<List<InlineKeyboardButton>>();
        List<InlineKeyboardButton> currentRow = new List<InlineKeyboardButton>();
        var status = user.StatusId==(short)UserStatusEnum.Active ? "✅"+UserStatusEnum.Active.ToString() : "🚫"+UserStatusEnum.Blocked.ToString();
        var userName = InlineKeyboardButton.WithCallbackData($"UserName: {user.UserName ?? "Unknown"}", user.UserName ?? "Unknown");
        var firstName = InlineKeyboardButton.WithCallbackData($"FistName: {user.FirstName??"Unknown"}", user.FirstName ?? "Unknown");
        var lastName = InlineKeyboardButton.WithCallbackData($"LastName: {user.LastName ?? "Unknown"}", user.LastName ?? "Unknown");
        var lastUpdateDate = InlineKeyboardButton.WithCallbackData($"LastUpdateDate: {user.LastUpdateDate}", user.LastUpdateDate.ToString());
        var creationDate = InlineKeyboardButton.WithCallbackData($"CreationDate: {user.CreationDate}", user.CreationDate.ToString());
        var userStatus = InlineKeyboardButton.WithCallbackData($"Status: {status}", status);
        currentRow.Add(userName);
        currentRow.Add(firstName);
        buttonRows.Add(currentRow);
        currentRow = new List<InlineKeyboardButton>
        {
            lastName,
            userStatus
        };
        buttonRows.Add(currentRow);
        currentRow = new List<InlineKeyboardButton>
        {
            lastUpdateDate,
            creationDate
        };
        buttonRows.Add(currentRow);

        currentRow = new List<InlineKeyboardButton>
        {
            InlineKeyboardButton.WithCallbackData($"Back To List", "/BackToList"),
            InlineKeyboardButton.WithCallbackData(user.StatusId is (short)UserStatusEnum.Active ? "🚫Block" : "✅Unblock",
            user.StatusId is (short)UserStatusEnum.Active ? $"/Block.{user.UserExternalId}" : $"/Unblock.{user.UserExternalId}")
        };
        buttonRows.Add(currentRow);
        return (new InlineKeyboardMarkup(buttonRows),user.UserExternalId.ToString());
    }
    public async Task<InlineKeyboardMarkup> GetUsersAsync(string page = "1")
    {

        var users = await _userRepository.GetAllAsync(x => true, null, null, int.Parse(page));


        List<List<InlineKeyboardButton>> buttonRows = new List<List<InlineKeyboardButton>>();
        List<InlineKeyboardButton> currentRow = new List<InlineKeyboardButton>();

        foreach (var user in users.Results)
        {
            var userName = user.UserName ?? user.FirstName ?? user.LastName ?? "Unknown";
            var button = InlineKeyboardButton.WithCallbackData("@" + userName,$"UserId.{user.UserExternalId}");

            currentRow.Add(button);

            if (currentRow.Count == 3)
            {
                buttonRows.Add(currentRow);
                currentRow = new List<InlineKeyboardButton>();
            }
        }

        if (currentRow.Count > 0)
        {
            buttonRows.Add(currentRow);
        }

        if (page!="1")
            buttonRows.Add(new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData("Previouse", (int.Parse(page)-1).ToString()) });

        if (int.Parse(page) < users.PageCount)
            buttonRows.Add(new List<InlineKeyboardButton> { InlineKeyboardButton.WithCallbackData("Next", (int.Parse(page)+1).ToString()) });
        return new InlineKeyboardMarkup(buttonRows);
    }
    public async Task<bool> BlockUserAsync(string userName)
    {
        return (await _userActions.BlockUserAsync(new BlockUserGrpcRequestModel { UserName = userName })).Status;
    }

    public async Task<bool> UnblockUser(string userName)
    {
        return (await _userActions.UnblockUserAsync(new UnblockUserGrpcRequest { UserName = userName })).Status;
    }

    public async Task ReStartBotAsync()
    {
        await _userActions.ReStartBotAsync();
    }

    public async Task StopBotAsync()
    {
        await _userActions.StopBotAsync();
    }
}
