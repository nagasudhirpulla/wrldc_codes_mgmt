using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infra.Persistence.Configurations;

internal class CodeRequestElementOwnerConfiguration : IEntityTypeConfiguration<CodeRequestElementOwner>
{
    public void Configure(EntityTypeBuilder<CodeRequestElementOwner> builder)
    {

        builder.Property(b => b.OwnerName)
           .IsRequired();

        builder.Property(b => b.CodeRequestId)
           .IsRequired();

        // combination of code request and owner Id will be unique
        builder
               .HasIndex(b => new { b.OwnerId, b.CodeRequestId })
               .IsUnique();

    }
}
