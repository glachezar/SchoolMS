namespace Infrastructure.Identity.Contexts;

using Finbuckle.MultiTenant;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public abstract class BaseDbContext 
    : MultiTenantIdentityDbContext<ApplicationUser, ApplicationRole, string, 
        IdentityUserClaim<string>, IdentityUserRole<string>, 
        IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
{
    protected BaseDbContext(ITenantInfo tenantInfo, DbContextOptions options)
        : base(tenantInfo, options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}
