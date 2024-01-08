using Service.Abstraction;

namespace Service.Helpers;
public class CommandSwitcher
{
    public static IDictionary<string, Func<Task>> CommandDictionary { get; set; }
    public CommandSwitcher(IUserActionService userActionService)
    {
        CommandDictionary = new Dictionary<string, Func<Task>>
        {
            {"/ReStartBot",  userActionService.ReStartBotAsync },
            {"/StopBot",  userActionService.StopBotAsync },
        };
    }

    public void Execute(string command)
    {
        CommandDictionary[command].Invoke();
    }
}