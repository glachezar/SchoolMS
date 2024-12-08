namespace Infrastructure.Identity;

using Application.Exceptions;
using Application.Features.Identity.Roles;
using Azure.Core;
using Finbuckle.MultiTenant;
using Infrastructure.Identity.Constants;
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
    ITenantInfo tenant) : IRoleService
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
            throw new IdentityException("Failed to create a role.", GetIdentityResultErrorDescriptions(result));

        return newRole.Name;
    }

    public async Task<string> UpdateAsync(UpdateRoleRequest request)
    {
        var roleInDb = await _roleManager.FindByIdAsync(request.Id)
            ?? throw new NotFoundException("Role does not exists.");

        if (RoleConstants.IsDefault(roleInDb.Name))
            throw new ConflictException($"Changers are not allowed on {roleInDb.Name} role.");

        roleInDb.Name = request.Name;
        roleInDb.Description = request.Description;
        roleInDb.NormalizedName = request.Name.ToUpperInvariant();

        var result = await _roleManager.UpdateAsync(roleInDb);

        if (!result.Succeeded)
            throw new IdentityException("Failed to update role.", GetIdentityResultErrorDescriptions(result));

        return roleInDb.Name;
    }

    public async Task<string> DeleteAsync(string id)
    {
        var roleInDb = await _roleManager.FindByIdAsync(id)
            ?? throw new NotFoundException("Role does not exists.");

        if (RoleConstants.IsDefault(roleInDb.Name))
            throw new ConflictException($"Not allowed to delete '{roleInDb.Name}' role.");

        if ((await _userManager.GetUsersInRoleAsync(roleInDb.Name)).Count > 0)
            throw new ConflictException($"Role '{roleInDb.Name}' is in use, can not be deleted.");

        var result = await _roleManager.DeleteAsync(roleInDb);

        if (!result.Succeeded)
            throw new IdentityException($"Failed to delete {roleInDb.Name} role.", GetIdentityResultErrorDescriptions(result));

        return roleInDb.Name;
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