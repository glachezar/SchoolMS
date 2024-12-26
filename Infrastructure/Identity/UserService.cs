namespace Infrastructure.Identity;

using Application.Exceptions;
using Application.Features.Identity.Users;
using Finbuckle.MultiTenant;
using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Tenancy;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<string> CreateUserAsync(CreateUserRequest request)
    {
        if (request.Password != request.ConfirmPassword)
            throw new ConflictException("Passwords do not match.");

        var newUser = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email,
            PhoneNumber = request.PhoneNumber,
            IsActive = request.IsActive
        };

        var result = await _userManager.CreateAsync(newUser, request.Password);

        if (!result.Succeeded)
            throw new IdentityException("Failed to create user.", GetIdentityResultErrorDescriptions(result));

        return newUser.Id;
    }

    public async Task<string> DeleteUserAsync(string id)
    {
        var userInDb = await GetUserAsync(id);

        var result = await _userManager.DeleteAsync(userInDb);

        if (!result.Succeeded)
            throw new IdentityException("Failed to delete user.", GetIdentityResultErrorDescriptions(result));

        return userInDb.Id;
    }

    public async Task<UserDto> GetUserByIdAsync(string id, CancellationToken cancellationToken)
    {
        var userInDb = await GetUserAsync(id, cancellationToken);

        return userInDb.Adapt<UserDto>();
    }

    public async Task<List<UserRoleDto>> GetUserRolesAsync(string userId, CancellationToken cancellationToken)
    {
        var userRoles = new List<UserRoleDto>();

        var userInDb = await GetUserAsync(userId, cancellationToken);

        var roles = await _roleManager
            .Roles
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        foreach (var rple in roles)
        {
            userRoles.Add(new UserRoleDto
            {
                RoleId = rple.Id,
                Name = rple.Name,
                Description = rple.Description,
                IsAssigned = await _userManager.IsInRoleAsync(userInDb, rple.Name)
            });
        }

        return userRoles;
    }

    public async Task<List<UserDto>> GetUsersAsync(CancellationToken cancellationToken)
    {
        var usersInDb =await _userManager
            .Users
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        return usersInDb.Adapt<List<UserDto>>();
    }

    public Task<bool> IsEmailTakenAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<string> UpdateUserAsync(UpdateUserRequest request)
    {
        throw new NotImplementedException();
    }

    private async Task<ApplicationUser> GetUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _userManager
            .Users
            .Where(u => u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken) ??
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
