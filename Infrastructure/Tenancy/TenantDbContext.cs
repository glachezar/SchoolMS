namespace Infrastructure.Tenancy;

using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;

public class TenantDbContext(DbContextOptions<TenantDbContext> options) 
    : EFCoreStoreDbContext<SchoolTenantInfo>(options){}