using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Interfaces;

public interface IAppDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    EntityEntry Attach([NotNullAttribute] object entity);
    EntityEntry Update([NotNullAttribute] object entity);
    DatabaseFacade Database { get; }

    DbSet<UserStakeholder> UserStakeholders { get; set; }
    DbSet<UserElementOwner> UserElementOwners { get; set; }
    DbSet<CodeRequest> CodeRequests { get; set; }
    DbSet<CodeRequestElementOwner> CodeRequestElementOwners { get; set; }
    DbSet<CodeRequestStakeHolder> CodeRequestStakeHolders { get; set; }
    DbSet<CodeRequestConsent> CodeRequestConsents { get; set; }
    DbSet<CodeRequestRemark> CodeRequestRemarks { get; set; }
}
