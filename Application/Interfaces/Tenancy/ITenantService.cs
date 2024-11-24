namespace Application.Interfaces.Tenancy;

using Application.Interfaces.Tenancy.Commands;

public interface ITenantService
{
    //Tenant creation
    Task<string> CreateTenantAsync(CreateTenantRequest createTenant);
}
 