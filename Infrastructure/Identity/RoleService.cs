namespace Infrastructure.Identity;

using Application.Exceptions;
using Application.Features.Identity.Roles;
using Finbuckle.MultiTenant;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class RoleService(
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager, 
    ApplicationDbContext applicationDbContext, 
    ITenantInfo tenant) :  IRoleService
{
    readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    readonly UserManager<ApplicationUser> _userManager = userManager;
    readonly ApplicationDbContext _applicationDbContext = applicationDbContext;
    readonly ITenantInfo _tenant = tenant;

    public async Task<string> CreateAsync(CreateRoleRequest request)
    {
        var newRole = new ApplicationRole()
        {
            Name = request.Name,
            Description = request.Description
        };

        var result = await _roleManager.CreateAsync(newRole);

        if (!result.Succeeded)
        {
            throw new IdentityException("Failed to create a role.", GetIdentityResultErrorDescriptions(result));
        }

        return newRole.Id;
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

    private List<string> GetIdentityResultErrorDescriptions(IdentityResult result)
    {
        var errorDescriptions = new List<string>();
        foreach (var error in result.Errors)
        {
            errorDescriptions.Add(error.Description);
        }
        return errorDescriptions;
    }
}
