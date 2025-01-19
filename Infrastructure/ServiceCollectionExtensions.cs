namespace Infrastructure;

using Infrastructure.Identity;
using Infrastructure.OpenApi;
using Infrastructure.Persistence;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddMultiTenancyServices(configuration)
            .AddPersistenceServices(configuration)
            .AddIdentityServices()
            .AddPermissions()
            .AddJwtAuthentication()
            .AddOpenApiDocumentation(configuration);
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        return app
            .UseCurrentUser()
            .UseOpenApiDocumentation();
    }
}
