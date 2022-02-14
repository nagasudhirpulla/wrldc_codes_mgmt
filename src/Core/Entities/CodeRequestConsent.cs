using Core.Common;
using Core.Enums;

namespace Core.Entities;

public class CodeRequestConsent : AuditableEntity
{
    public CodeRequest? CodeRequest { get; set; }
    public int CodeRequestId { get; set; }

    public ApplicationUser? Stakeholder { get; set; }
    public string? StakeholderId { get; set; }

    public string? Remarks { get; set; }
    public string? RldcRemarks { get; set; }

    private ApprovalStatus? _approvalStatus;

    public ApprovalStatus? ApprovalStatus
    {
        get => _approvalStatus;
        set
        {
            if (_approvalStatus != value)
            {
                _approvalStatus = value;
                ApprovalStatusChangedAt = DateTime.Now;
            }
        }
    }

    public DateTime? ApprovalStatusChangedAt { get; set; }
}
