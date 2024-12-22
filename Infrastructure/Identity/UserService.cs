namespace Infrastructure.Identity;

using Application.Features.Identity.Users;
using Finbuckle.MultiTenant;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
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

    public Task<string> ActivateOrDeactivateUserAsync(string userId, bool activation)
    {
        throw new NotImplementedException();
    }

    public Task<string> AssignRolesAsync(string userId, UserRoleRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<string> ChangePasswordAsync(ChangePasswordRequest request)
    {
        throw new NotImplementedException();
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
}
