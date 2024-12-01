namespace Infrastructure.Persistence.DbInitializers;

using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Contexts;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

internal class ApplicationDbInitializer(
    SchoolTenantInfo tenant,
    RoleManager<ApplicationRole> roleManager,
    UserManager<ApplicationUser> userManager)
{
    readonly SchoolTenantInfo _tenant = tenant;
    readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    readonly UserManager<ApplicationUser> _userManager = userManager;

    public async Task InitializeDefaultRolesAsync(ApplicationDbContext applicationDbContext, CancellationToken cancellationToken)
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
                await AssignPermissionsToRole(applicationDbContext, SchoolPermissions.Basic, incomingRole, cancellationToken);
            }
            else if (roleName == RoleConstants.Admin)
            {
                await AssignPermissionsToRole(applicationDbContext, SchoolPermissions.Admin, incomingRole, cancellationToken);
            }
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
