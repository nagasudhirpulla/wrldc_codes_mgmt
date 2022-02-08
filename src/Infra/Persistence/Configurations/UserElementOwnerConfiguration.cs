using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infra.Persistence.Configurations;

class UserElementOwnerConfiguration : IEntityTypeConfiguration<UserElementOwner>
{
    public void Configure(EntityTypeBuilder<UserElementOwner> builder)
    {

        builder.Property(b => b.OwnerName)
           .IsRequired();

        builder.Property(b => b.UsrId)
           .IsRequired();

        // combination of user and stakeholder Id will be unique
        builder
               .HasIndex(b => new { b.OwnerId, b.UsrId })
               .IsUnique();

    }
}