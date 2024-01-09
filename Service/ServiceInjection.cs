using ExchangeBot.Abstraction;
using Grpc.Client.Abstraction;
using Grpc.Client.Implementation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service.Abstraction;
using Service.Helpers;
using Service.Implementation;

namespace Service;

public static class ServiceInjection
{
    public static IServiceCollection InjectServices(this IServiceCollection services)
    {
        services.AddScoped<CommandSwitcher>();
        services.AddHostedService<TelegramCommandHandler>();
        services.AddHostedService<RatesUpdateScheduleService>();
        services.AddScoped<IUserActionsClient, UserActionsClient>();
        services.AddScoped<IUserActionService, UserActionService>();
        services.AddScoped<IRateActionClient, RateActionClient>();
        return services;
    }
}
