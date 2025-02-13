using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sales.Application.Services;
using Sales.Domain.Abstractions;
using Sales.Domain.Carts;
using Sales.Domain.Sales;
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

    public static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<ApplicationDbContext>>();
        var context = services.GetRequiredService<ApplicationDbContext>();

        try
        {
            context.Database.Migrate();

            logger.LogInformation("Migrations aplicadas com sucesso!");
        }
        catch (Exception ex)
        {
            logger.LogError($"Erro ao aplicar migrations: {ex}");
        }
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
        services.AddScoped<ICartRepository, CartRepository>();
        services.AddScoped<ISaleRepository, SaleRepository>();
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));

        return services;
    }

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}