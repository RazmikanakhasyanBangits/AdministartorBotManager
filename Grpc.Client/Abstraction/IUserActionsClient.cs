using UserActionsProto;

namespace Grpc.Client.Abstraction;
public interface IUserActionsClient
{
    Task<BlockUserGrpcResponseModel> BlockUserAsync(BlockUserGrpcRequestModel model);
    Task ReStartBotAsync();
    Task<StopBotGrpcResponse> StopBotAsync();
    Task<UnblockUserGrpcResponse> UnblockUserAsync(UnblockUserGrpcRequest model);
}
