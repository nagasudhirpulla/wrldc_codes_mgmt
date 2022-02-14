using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infra.Persistence.Configurations;

internal class CodeRequestRemarkConfiguration : IEntityTypeConfiguration<CodeRequestRemark>
{
    public void Configure(EntityTypeBuilder<CodeRequestRemark> builder)
    {

        builder.Property(b => b.StakeholderId)
           .IsRequired();

        builder.Property(b => b.CodeRequestId)
           .IsRequired();
    }
}
