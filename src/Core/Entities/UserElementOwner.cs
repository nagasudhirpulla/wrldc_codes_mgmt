using Core.Common;

namespace Core.Entities;

public class UserElementOwner : AuditableEntity
{
    public int OwnerId { get; set; }
    public string? OwnerName { get; set; }

    public ApplicationUser? Usr { get; set; }
    public string? UsrId { get; set; }
}
