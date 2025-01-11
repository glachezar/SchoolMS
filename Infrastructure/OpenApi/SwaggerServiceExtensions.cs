namespace Infrastructure.OpenApi;

using Microsoft.Extensions.DependencyInjection;

internal static class SwaggerServiceExtensions
{
    internal static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services, SwagerSettings settings)
    {
        services.AddSwaggerDocument(config =>
        {

        });
        return services;
    }
}
