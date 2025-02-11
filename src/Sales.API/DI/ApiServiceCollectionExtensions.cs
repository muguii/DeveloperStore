using Microsoft.OpenApi.Models;

namespace Sales.API.DI;

public static class ApiServiceCollectionExtensions
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "DeveloperStore.API", Version = "v1" });
        });

        return services;
    }
}