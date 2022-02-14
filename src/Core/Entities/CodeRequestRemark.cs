using Core.Common;

namespace Core.Entities;

public class CodeRequestRemark : AuditableEntity
{
    public CodeRequest? CodeRequest { get; set; }
    public int CodeRequestId { get; set; }

    public ApplicationUser? Stakeholder { get; set; }
    public string? StakeholderId { get; set; }

    public string? Remarks { get; set; }
    public string? RldcRemarks { get; set; }
}
