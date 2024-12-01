namespace Infrastructure.Tenancy;

using Application.Interfaces.Tenancy;
using Application.Interfaces.Tenancy.Commands;
using Finbuckle.MultiTenant;
using Infrastructure.Persistence.DbInitializers;
using System.Threading.Tasks;

public class TenantService(IMultiTenantStore<SchoolTenantInfo> tenantStore,
    ApplicationDbInitializer applicationDbInitializer) : ITenantService
{
    readonly IMultiTenantStore<SchoolTenantInfo> _tenantStore = tenantStore;
    readonly ApplicationDbInitializer _applicationDbInitializer = applicationDbInitializer;

    public async Task<string> CreateTenantAsync(CreateTenantRequest createTenant, CancellationToken cancellationToken)
    {
        var newTenant = new SchoolTenantInfo
        {
            Id = createTenant.Identifier,
            Name = createTenant.Name,
            ConnectionString = createTenant.ConnectionString,
            AdminEmail = createTenant.AdminEmail,
            ValidUpTo = createTenant.ValidUpTo,
            IsActive = createTenant.IsActive
        };

        var isSuccsess = await _tenantStore.TryAddAsync(newTenant);

        try
        {
            await _applicationDbInitializer.InitializeDatabaseWithTenantAsync(cancellationToken);
        }
        catch (Exception)
        {
            await _tenantStore.TryRemoveAsync(createTenant.Identifier);
            throw;
        }
        return newTenant.Id;
    }
}