namespace Infrastructure.Persistence.DbInitializers;

using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

internal class ApplicationDbInitializer(
    SchoolTenantInfo tenant,
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager,
    ApplicationDbContext applicationDbContext)
{
    readonly SchoolTenantInfo _tenant = tenant;
    readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    readonly UserManager<ApplicationUser> _userManager = userManager;
    readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

    public async Task InitializeDatabaseWithTenantAsync(CancellationToken cancellationToken)
    {
        await InitializeDefaultRolesAsync(cancellationToken);
        await InitializeAdminUserAsync(cancellationToken);
    }

    private async Task InitializeDefaultRolesAsync(CancellationToken cancellationToken)
    {
        foreach (string roleName in RoleConstants.DefaultRoles)
        {
            if (await _roleManager.Roles.SingleOrDefaultAsync(role => role.Name == roleName, cancellationToken) is not ApplicationRole incomingRole)
            {
                incomingRole = new ApplicationRole() { Name = roleName, Description = $"{roleName} Role"};
                await _roleManager.CreateAsync(incomingRole);
            }

            if (roleName == RoleConstants.Basic)
            {
                await AssignPermissionsToRole(_applicationDbContext, SchoolPermissions.Basic, incomingRole, cancellationToken);
            }
            else if (roleName == RoleConstants.Admin)
            {
                await AssignPermissionsToRole(_applicationDbContext, SchoolPermissions.Admin, incomingRole, cancellationToken);
            }
        }
    }

    private async Task InitializeAdminUserAsync(CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_tenant.AdminEmail))
        {
            return;
        }
        if (await _userManager.Users.FirstOrDefaultAsync(u => u.Email == _tenant.AdminEmail) is not ApplicationUser adminUser)
        {
            adminUser = new ApplicationUser()
            {
                FirstName = TenancyConstants.FirstName,
                LastName = TenancyConstants.LastName,
                Email = _tenant.AdminEmail,
                UserName = _tenant.AdminEmail,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                NormalizedEmail = _tenant.AdminEmail.ToUpperInvariant(),
                NormalizedUserName = _tenant.AdminEmail.ToUpperInvariant(),
                IsActive = true
            };

            var password = new PasswordHasher<ApplicationUser>();
            adminUser.PasswordHash = password.HashPassword(adminUser, TenancyConstants.DefaultPassword);
            await _userManager.CreateAsync(adminUser);
        }
        if (!await _userManager.IsInRoleAsync(adminUser, RoleConstants.Admin))
        {
            await _userManager.AddToRoleAsync(adminUser, RoleConstants.Admin);
        }
    }

    private async Task AssignPermissionsToRole(ApplicationDbContext applicationDbContext, 
        IReadOnlyList<SchoolPermission> rolePermissions,
        ApplicationRole currentRole,
        CancellationToken cancellationToken)
    {
        var currentClaims = await _roleManager.GetClaimsAsync(currentRole);
        foreach (SchoolPermission rolePermission in rolePermissions)
        {
            if (!currentClaims.Any(c => c.Type == ClaimConstants.Permission && c.Value == rolePermission.Name))
            {
                await applicationDbContext.RoleClaims.AddAsync(new IdentityRoleClaim<string>
                {
                    RoleId = currentRole.Id,
                    ClaimType = ClaimConstants.Permission,
                    ClaimValue = rolePermission.Name
                }, 
                cancellationToken);

                await applicationDbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
