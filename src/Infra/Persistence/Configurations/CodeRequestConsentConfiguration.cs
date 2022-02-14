using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infra.Persistence.Configurations;

internal class CodeRequestConsentConfiguration : IEntityTypeConfiguration<CodeRequestConsent>
{
    public void Configure(EntityTypeBuilder<CodeRequestConsent> builder)
    {

        builder.Property(b => b.StakeholderId)
           .IsRequired();

        builder.Property(b => b.CodeRequestId)
           .IsRequired();

        builder.Property(b => b.ApprovalStatus)
           .IsRequired();
    }
}
