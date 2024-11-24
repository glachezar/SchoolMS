namespace Infrastructure.Identity.Contexts;

using Domain.Entities;
using Finbuckle.MultiTenant;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext(ITenantInfo tenantInfo, DbContextOptions<ApplicationDbContext> options) 
    : BaseDbContext(tenantInfo, options)
{
    public DbSet<School> Schools => Set<School>();
}
