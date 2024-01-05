using UserActionsProto;

namespace Grpc.Client.Abstraction;
public interface IUserActionsClient
{
    Task<BlockUserGrpcResponseModel> BlockUserAsync(BlockUserGrpcRequestModel model);
    Task<UnblockUserGrpcResponse> UnblockUserAsync(UnblockUserGrpcRequest model);
}
