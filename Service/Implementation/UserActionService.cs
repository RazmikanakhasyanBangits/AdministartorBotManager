using Grpc.Client.Abstraction;
using Service.Abstraction;
using UserActionsProto;

namespace Service.Implementation;

public class UserActionService : IUserActionService
{
    private readonly IUserActionsClient _userActions;

    public UserActionService(IUserActionsClient userActions)
    {
        _userActions = userActions;
    }

    public async Task<bool> BlockUserAsync(string userName)
    {
        return (await _userActions.BlockUserAsync(new BlockUserGrpcRequestModel { UserName = userName })).Status;
    }

    public async Task<bool> UnblockUser(string userName)
    {
        return (await _userActions.UnblockUserAsync(new UnblockUserGrpcRequest { UserName = userName })).Status;
    }


}
