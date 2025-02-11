using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sales.Application.Services;
using Sales.Domain.Abstractions;
using Sales.Infrastructure.Persistence;
using Sales.Infrastructure.Persistence.Repositories;

namespace Sales.Infrastructure.DI;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
    {
        AddConfigurationsProviders(configuration);

        services.AddDatabase(configuration)
                .AddRepositories()
                .AddUnitOfWork();

        return services;
    }

    private static void AddConfigurationsProviders(ConfigurationManager configuration)
    {
        configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddEnvironmentVariables();
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfigurationRoot configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DeveloperStore")));

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}