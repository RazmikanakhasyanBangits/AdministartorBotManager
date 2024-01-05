namespace Service.Abstraction;

public interface IUserActionService
{
    Task<bool> BlockUserAsync(string userName);
    Task<bool> UnblockUser(string userName);
}
