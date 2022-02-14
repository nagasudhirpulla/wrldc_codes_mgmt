using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infra.Persistence.Configurations;

internal class CodeRequestStakeHolderConfiguration : IEntityTypeConfiguration<CodeRequestStakeHolder>
{
    public void Configure(EntityTypeBuilder<CodeRequestStakeHolder> builder)
    {

        // many to many relationship in ef-core - https://www.entityframeworktutorial.net/efcore/configure-many-to-many-relationship-in-ef-core.aspx
        // https://www.learnentityframeworkcore.com/configuration/many-to-many-relationship-configuration
        builder.HasKey(b => new { b.StakeholderId, b.CodeRequestId });

        builder
            .HasOne(rs => rs.CodeRequest)
            .WithMany(r => r.ConcernedStakeholders)
            .HasForeignKey(rs => rs.CodeRequestId);


        builder
            .HasOne(rs => rs.Stakeholder)
            .WithMany()
            .HasForeignKey(rcc => rcc.StakeholderId);
    }
}
