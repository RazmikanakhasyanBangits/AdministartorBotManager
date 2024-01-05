using Grpc.Client.Abstraction;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using UserActionsProto;

namespace Grpc.Client.Implementation;
public class UserActionsClient : IUserActionsClient
{
    private readonly IConfiguration _configuration;

    public UserActionsClient(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<BlockUserGrpcResponseModel> BlockUserAsync(BlockUserGrpcRequestModel model)
    {
        Channel channel = new(_configuration["Services:ExchangeBotManager"], ChannelCredentials.Insecure);
        UserActions.UserActionsClient client = new(channel);
        return await client.BlockUserAsync(model);
    }
    public async Task<UnblockUserGrpcResponse> UnblockUserAsync(UnblockUserGrpcRequest model)
    {
        Channel channel = new(_configuration["Services:ExchangeBotManager"], ChannelCredentials.Insecure);
        UserActions.UserActionsClient client = new(channel);
        return await client.UnblockUserAsync(model);
    }
}
