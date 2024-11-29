namespace Infrastructure.Persistence.DbConfigurations;

using Domain.Entities;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

internal class SchoolConfig : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        builder
            .ToTable("Schools", SchemaNames.Academics)
            .IsMultiTenant();

        builder
            .Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);
    }
}
