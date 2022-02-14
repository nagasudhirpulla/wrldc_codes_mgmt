using Core.Common;

namespace Core.Entities;

public class CodeRequestStakeHolder : AuditableEntity
{
    public string? StakeholderId { get; set; }
    public ApplicationUser? Stakeholder { get; set; }
    public int CodeRequestId { get; set; }
    public CodeRequest? CodeRequest { get; set; }
}