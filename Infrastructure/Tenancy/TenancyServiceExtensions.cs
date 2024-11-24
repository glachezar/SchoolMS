namespace Infrastructure.Tenancy;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

internal static class TenancyServiceExtensions
{
    internal static IServiceCollection AddMultiTenancyServices(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddDbContext<TenantDbContext>(options =>

                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
            .AddMultiTenant<SchoolTenantInfo>()
            .WithHeaderStrategy(TenancyConstants.TenantIdName)
            .WithEFCoreStore<TenantDbContext, SchoolTenantInfo>()
            .Services;
    }
}
