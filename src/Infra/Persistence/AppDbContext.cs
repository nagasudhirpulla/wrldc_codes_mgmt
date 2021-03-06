using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Application.Common.Interfaces;
using Core.Common;
using Core.Entities;
using Infra.SmartEnum.EFCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infra.Persistence;

public class AppDbContext : IdentityDbContext<ApplicationUser>, IAppDbContext
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDomainEventService _domainEventService;

    public AppDbContext(DbContextOptions<AppDbContext> options, ICurrentUserService currentUserService, IDomainEventService domainEventService)
        : base(options)
    {
        _currentUserService = currentUserService;
        _domainEventService = domainEventService;
    }

    public DbSet<UserStakeholder> UserStakeholders { get; set; }
    public DbSet<UserElementOwner> UserElementOwners { get; set; }
    public DbSet<CodeRequest> CodeRequests { get; set; }
    public DbSet<CodeRequestElementOwner> CodeRequestElementOwners { get; set; }
    public DbSet<CodeRequestStakeHolder> CodeRequestStakeHolders { get; set; }
    public DbSet<CodeRequestConsent> CodeRequestConsents { get; set; }
    public DbSet<CodeRequestRemark> CodeRequestRemarks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        foreach (var property in builder.Model.GetEntityTypes()
                 .SelectMany(t => t.GetProperties())
                 .Where(p => p.ClrType == typeof(DateTime) || p.ClrType == typeof(DateTime?)))
            property.SetColumnType("timestamp without time zone");
        builder.ConfigureSmartEnum();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = _currentUserService.UserName;
                    entry.Entity.Created = DateTime.Now;
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedBy = _currentUserService.UserName;
                    entry.Entity.LastModified = DateTime.Now;
                    break;
            }
        }

        var events = ChangeTracker.Entries<IHasDomainEvent>()
            .Select(x => x.Entity.DomainEvents)
            .SelectMany(x => x)
            .Where(domainEvent => !domainEvent.IsPublished)
            .ToArray();

        if (this.Database.CurrentTransaction == null)
        {
            using IDbContextTransaction transaction = this.Database.BeginTransaction();
            try
            {
                var result = await base.SaveChangesAsync(cancellationToken);
                await DispatchEvents(events);
                transaction.Commit();
                return result;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"rolling back changes occured while saving DB changes, {ex.Message}");
                throw;
            }
        }
        else
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            await DispatchEvents(events);
            return result;
        }
    }

    private async Task DispatchEvents(DomainEvent[] events)
    {
        foreach (var @event in events)
        {
            @event.IsPublished = true;
            await _domainEventService.Publish(@event);
        }
    }
}
