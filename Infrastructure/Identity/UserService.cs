namespace Infrastructure.Identity;

using Application.Exceptions;
using Application.Features.Identity.Users;
using Finbuckle.MultiTenant;
using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class UserService(
    ITenantInfo tenant, ApplicationDbContext context,
    SignInManager<ApplicationUser> signInManager,
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager) : IUserService
{
    readonly UserManager<ApplicationUser> _userManager = userManager;
    readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    readonly ApplicationDbContext _context = context;
    readonly ITenantInfo _tenant = tenant;

    public async Task<string> ActivateOrDeactivateUserAsync(string userId, bool activation)
    {
        var userInDb = await GetUserAsync(userId);

        userInDb.IsActive = activation;
        await _userManager.UpdateAsync(userInDb);
        return userId;
    }

    public async Task<string> AssignRolesAsync(string userId, UserRoleRequest request)
    {
        var userInDb = await GetUserAsync(userId);

        if (await _userManager.IsInRoleAsync(userInDb, RoleConstants.Admin)
            && request.UserRoles.Any(ur => !ur.IsAssigned && ur.Name == RoleConstants.Admin))
        {
            var adminUsersCount = (await _userManager
                .GetUsersInRoleAsync(RoleConstants.Admin))
                .Count;

            if (userInDb.Email == TenancyConstants.Root.Email)
                if (_tenant.Id == TenancyConstants.Root.Id)
                    throw new ConflictException("Not allowed to remove Admin Role for a Root Tenant User.");
            else if (adminUsersCount <= 2)
                throw new ConflictException("Tenant should have at least 3 Admin Users.");
        }

        foreach (var userRole in request.UserRoles)
        {
            if (await _roleManager.FindByIdAsync(userRole.RoleId) is not null)
            {
                if (userRole.IsAssigned)
                    if (!await _userManager.IsInRoleAsync(userInDb, userRole.Name))
                        await _userManager.AddToRoleAsync(userInDb, userRole.Name);               
                else
                    await _userManager.RemoveFromRoleAsync(userInDb, userRole.Name);
                
            }
        }
        return userInDb.Id;
    }

    public async Task<string> ChangePasswordAsync(ChangePasswordRequest request)
    {
        var userInDb = await GetUserAsync(request.UserId);

        if (request.NewPassword != request.ConfirmPassword)
            throw new ConflictException("Passwords do not match.");

        var result = await _userManager.ChangePasswordAsync(userInDb, request.CurrentPassword, request.NewPassword);

        if (!result.Succeeded)
            throw new IdentityException("Failed to change password.", GetIdentityResultErrorDescriptions(result));

        return userInDb.Id;
    }

    public Task<string> CreateUserAsync(CreateUserRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<string> DeleteUserAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<UserDto> GetUserByIdAsync(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserRoleDto>> GetUserRolesAsync(string userId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<List<UserDto>> GetUsersAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsEmailTakenAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<string> UpdateUserAsync(UpdateUserRequest request)
    {
        throw new NotImplementedException();
    }

    private async Task<ApplicationUser> GetUserAsync(string userId)
    {
        return await _userManager
            .Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync() ??
            throw new NotFoundException("User do not exists.");
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
