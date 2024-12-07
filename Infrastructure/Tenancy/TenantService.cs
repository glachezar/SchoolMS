namespace Infrastructure.Tenancy;

using Application.Interfaces.Tenancy;
using Application.Interfaces.Tenancy.Commands;
using Application.Interfaces.Tenancy.Models;
using Finbuckle.MultiTenant;
using Infrastructure.Persistence.DbInitializers;
using System.Collections.Generic;
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

    public async Task<string> ActivateTenantAsync(string tenantId)
    {
        var tenantInDb = await _tenantStore.TryGetAsync(tenantId);
        tenantInDb.IsActive = true;
        await _tenantStore.TryUpdateAsync(tenantInDb);
        return tenantInDb.Id;
    }

    public async Task<string> DeactivateTenantAsync(string tenantId)
    {
        var tenantInDb = await _tenantStore.TryGetAsync(tenantId);
        tenantInDb.IsActive = false;
        await _tenantStore.TryUpdateAsync(tenantInDb);
        return tenantInDb.Id;
    }

    public async Task<TenantDto> GetTenantByIdAsync(string tenantId)
    {
        var tenantInDb = await _tenantStore.TryGetAsync(tenantId);

        return new TenantDto()
        {
            Id = tenantInDb.Id,
            Identifier = tenantInDb.Identifier,
            Name = tenantInDb.Name,
            ConnectionString = tenantInDb.ConnectionString,
            AdminEmail = tenantInDb.AdminEmail,
            ValidUpTo = tenantInDb.ValidUpTo,
            IsActive = tenantInDb.IsActive
        };
    }

    public Task<List<TenantDto>> GetAllTenantsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<string> UpdateTenantSubscriptionAsync(string tenantId, DateTime newExpiryDate)
    {
        throw new NotImplementedException();
    }
}