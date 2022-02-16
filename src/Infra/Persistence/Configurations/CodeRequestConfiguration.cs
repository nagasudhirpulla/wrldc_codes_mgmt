using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infra.Persistence.Configurations;

internal class CodeRequestConfiguration : IEntityTypeConfiguration<CodeRequest>
{
    public void Configure(EntityTypeBuilder<CodeRequest> builder)
    {

        builder.Property(b => b.CodeType)
           .IsRequired();

        builder.Property(b => b.RequestState)
           .IsRequired();

        builder.Property(b => b.RequesterId)
           .IsRequired();

        builder.Property(b => b.Description)
           .IsRequired();
    }
}