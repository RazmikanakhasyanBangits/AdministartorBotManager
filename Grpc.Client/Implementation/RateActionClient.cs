using Google.Protobuf.WellKnownTypes;
using Grpc.Client.Abstraction;
using Grpc.Core;
using Microsoft.Extensions.Configuration;
using UserActionsProto;

namespace Grpc.Client.Implementation;

public class RateActionClient : IRateActionClient
{
    private readonly IConfiguration _configuration;

    public RateActionClient(IConfiguration configuration)
    {
        _configuration=configuration;
    }

    public async Task UpdateRatesAsync()
    {
        try
        {
            Channel channel = new(_configuration["Services:ExchangeBotManager"], ChannelCredentials.Insecure);
            RateActions.RateActionsClient client = new(channel);
            await client.UpdateRatesAsync(new Empty());
        }
        catch
        {
            //ignore
        }
       
    }
}
