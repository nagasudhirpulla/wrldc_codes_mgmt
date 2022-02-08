using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infra.Persistence.Configurations;

class UserStakeholderConfiguration : IEntityTypeConfiguration<UserStakeholder>
{
    public void Configure(EntityTypeBuilder<UserStakeholder> builder)
    {

        builder.Property(b => b.StakeHolderName)
           .IsRequired();

        builder.Property(b => b.UsrId)
           .IsRequired();

        // combination of user and stakeholder Id will be unique
        builder
               .HasIndex(b => new { b.StakeHolderId, b.UsrId })
               .IsUnique();

        // cascade delete is the default behaviour, hence not required
        //builder.HasOne(e => e.Usr).WithMany(u => u.Stakeholders).OnDelete(DeleteBehavior.Cascade);
    }
}
