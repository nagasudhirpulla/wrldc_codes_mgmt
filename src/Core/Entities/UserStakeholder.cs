using Core.Common;

namespace Core.Entities;

public class UserStakeholder : AuditableEntity
{
    public int StakeHolderId { get; set; }
    public string? StakeHolderName { get; set; }

    public ApplicationUser? Usr { get; set; }
    public string? UsrId { get; set; }
}
