using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository.Abstraction;
using Repository.Implementation;

namespace Repository;

public static class RepositoryInjection
{
    public static IServiceCollection InjectRepositories(this IServiceCollection services, string connectionString)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ILocationRepository, LocationRepository>();
        _ = services.AddDbContext<ExchangeBotDbContext>(_ => _.UseSqlServer(connectionString),ServiceLifetime.Transient);

        return services;
    }
}
