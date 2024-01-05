using Service.Abstraction;

namespace Service.Helpers;
public class CommandSwitcher
{
    public static IDictionary<string, Func<string, Task<bool>>> CommandDictionary { get; set; }
    public CommandSwitcher(IUserActionService userActionService)
    {
        CommandDictionary = new Dictionary<string, Func<string, Task<bool>>>
        {
            {"/blockUser",  userActionService.BlockUserAsync },
            {"/unblockUser",  userActionService.UnblockUser },
        };
    }
}