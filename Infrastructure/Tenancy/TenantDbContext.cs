namespace Infrastructure.Tenancy;

using Finbuckle.MultiTenant.Stores;
using Infrastructure.Persistence.DbConfigurations;
using Microsoft.EntityFrameworkCore;

public class TenantDbContext(DbContextOptions<TenantDbContext> options) 
    : EFCoreStoreDbContext<SchoolTenantInfo>(options)
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder
            .Entity<SchoolTenantInfo>()
            .ToTable("Tenants", SchemaNames.Multitenancy);
    }
}