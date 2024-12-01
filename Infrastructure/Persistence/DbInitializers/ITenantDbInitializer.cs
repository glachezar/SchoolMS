namespace Infrastructure.Persistence.DbInitializers;

using Infrastructure.Tenancy;

internal interface ITenantDbInitializer
{
    Task InitializeDatabaseAsync(CancellationToken cancellationToken);
    //Task InitializeDatabaseWithTenantAsync(CancellationToken cancellationToken);
    //Task InitializeApplicationDbForTenantAsync(SchoolTenantInfo tenant, CancellationToken cancellationToken);
}
