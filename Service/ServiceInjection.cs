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
        _ = services.AddScoped<ICommandHandler, TelegramCommandHandler>();
        _ = services.AddScoped<CommandSwitcher>();
        _ = services.AddHostedService<TelegramCommandHandler>();
        _ = services.AddScoped<IUserActionsClient, UserActionsClient>();
        _ = services.AddScoped<IUserActionService, UserActionService>();
        return services;
    }
}
