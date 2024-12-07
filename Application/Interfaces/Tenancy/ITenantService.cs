namespace Application.Interfaces.Tenancy;

using Application.Interfaces.Tenancy.Commands;

public interface ITenantService
{
    //Tenant creation
    Task<string> CreateTenantAsync(CreateTenantRequest createTenant, CancellationToken cancellationToken);

    Task<string> ActivateTenantAsync(string tenantId);

    Task<string> DeactivateTenantAsync(string tenantId);

    Task<string> UpdateTenantSubscriptionAsync(string tenantId, DateTime newExpiryDate);
}
 