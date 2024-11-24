namespace Infrastructure.Tenancy;

using Application.Interfaces.Tenancy;
using Application.Interfaces.Tenancy.Commands;
using System.Threading.Tasks;

internal class TenantService : ITenantService
{
    public Task<string> CreateTenantAsync(CreateTenantRequest createTenant)
    {
        throw new NotImplementedException();
    }
}
