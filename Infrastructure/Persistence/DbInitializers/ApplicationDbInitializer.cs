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
        }
    }
}
