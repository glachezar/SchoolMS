namespace Infrastructure.Persistence.DbInitializers;

using Finbuckle.MultiTenant;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;

internal class TenantDbInitializer(TenantDbContext tenantDbContext, ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider) : ITenantDbInitializer
{
    readonly TenantDbContext _tenantDbContext = tenantDbContext;
    readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
    readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task InitializeDatabaseAsync(CancellationToken cancellationToken)
    {
        await InitializeDatabaseWithTenantAsync(cancellationToken);

        foreach (var tenant in await _tenantDbContext.TenantInfo.ToListAsync(cancellationToken))
        {
            await InitializeApplicationDbForTenantAsync(tenant, cancellationToken);
        }
    }

    private async Task InitializeDatabaseWithTenantAsync(CancellationToken cancellationToken)
    {
        if (await _tenantDbContext.TenantInfo.FindAsync([TenancyConstants.Root.Id], cancellationToken: cancellationToken) is null)
        {
            var rootTenant = new SchoolTenantInfo
            {
                Id = TenancyConstants.Root.Id,
                Identifier = TenancyConstants.Root.Name,
                Name = TenancyConstants.Root.Name,
                AdminEmail = TenancyConstants.Root.Email,
                IsActive = true,
                ValidUpTo = DateTime.UtcNow.AddYears(5),
            };

            await _tenantDbContext.TenantInfo.AddAsync(rootTenant, cancellationToken);
            await _tenantDbContext.SaveChangesAsync(cancellationToken);
        }
    }

    private async Task InitializeApplicationDbForTenantAsync(SchoolTenantInfo tenant, CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        _serviceProvider.GetRequiredService<IMultiTenantContextAccessor>()
            .MultiTenantContext = new MultiTenantContext<SchoolTenantInfo>()
            {
                TenantInfo = tenant
            };
        
        await _serviceProvider.GetRequiredService<ApplicationDbInitializer>()
            .InitializeDatabaseWithTenantAsync(_applicationDbContext, cancellationToken);
    }
}