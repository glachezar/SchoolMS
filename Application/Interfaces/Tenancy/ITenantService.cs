namespace Application.Interfaces.Tenancy;

using Application.Interfaces.Tenancy.Commands;
using Application.Interfaces.Tenancy.Models;

public interface ITenantService
{
    //Tenant creation
    Task<string> CreateTenantAsync(CreateTenantRequest createTenant, CancellationToken cancellationToken);

    Task<string> ActivateTenantAsync(string tenantId);

    Task<string> DeactivateTenantAsync(string tenantId);

    Task<string> UpdateTenantSubscriptionAsync(string tenantId, DateTime newExpiryDate);

    Task<List<TenantDto>> GetAllTenantsAsync();

    Task<TenantDto> GetTenantByIdAsync(string tenantId);
}
 