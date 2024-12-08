namespace Infrastructure.Identity;

using Application.Features.Identity.Roles;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class RoleService : IRoleService
{
    public Task<string> CreateAsync(CreateRoleRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<string> DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DoesItExistAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task<List<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<RoleDto> GetRoleByIdAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<string> UpdateAsync(UpdateRoleRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<string> UpdatePermissionsAsync(UpdateRolePermissionsRequest request)
    {
        throw new NotImplementedException();
    }
}
