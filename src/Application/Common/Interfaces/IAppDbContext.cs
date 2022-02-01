using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;

namespace Application.Common.Interfaces;

public interface IAppDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    EntityEntry Attach([NotNullAttribute] object entity);
    EntityEntry Update([NotNullAttribute] object entity);
}
