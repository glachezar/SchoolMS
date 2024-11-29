namespace Infrastructure.Tenancy;

using Infrastructure.Persistence.DbConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class TenancyDbConfiguration : IEntityTypeConfiguration<SchoolTenantInfo>
{
    public void Configure(EntityTypeBuilder<SchoolTenantInfo> builder)
    {
        builder
            .ToTable("SchoolTenantInfo", SchemaNames.Multitenancy);
    }
}
