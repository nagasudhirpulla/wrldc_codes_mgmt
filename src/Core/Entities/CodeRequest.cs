using Core.Common;
using Core.Enums;

namespace Core.Entities;

public class CodeRequest : AuditableEntity
{
    public CodeType? CodeType { get; set; }

    private CodeRequestStatus _requestState = CodeRequestStatus.Requested;
    public DateTime RequestStatusChangedAt { get; set; } = DateTime.Now;

    public CodeRequestStatus RequestState
    {
        get => _requestState;
        set
        {
            if (_requestState != value)
            {
                _requestState = value;
                RequestStatusChangedAt = DateTime.Now;
            }
        }
    }

    public ApplicationUser? Requester { get; set; }
    public string? RequesterId { get; set; }

    public string? Description { get; set; }
    public string? Remarks { get; set; }

    public int? ElementId { get; set; }
    public string? ElementName { get; set; }

    public int? ElementTypeId { get; set; }
    public string? ElementType { get; set; }

    public int? OutageTypeId { get; set; }
    public string? OutageType { get; set; }

    public int? OutageTagId { get; set; }
    public string? OutageTag { get; set; }
    //for approved outage
    public int? OutageRequestId { get; set; }
    //for already created outage like revival outage
    public int? OutageId { get; set; }

    // code string if a code is created for this request
    public string? Code { get; set; }

    public DateTime? DesiredExecutionStartTime { get; set; }
    public DateTime? DesiredExecutionEndTime { get; set; }

    public IList<CodeRequestElementOwner> ElementOwners { get; private set; } = new List<CodeRequestElementOwner>();

    public IList<CodeRequestStakeHolder> ConcernedStakeholders { get; private set; } = new List<CodeRequestStakeHolder>();

    public IList<CodeRequestConsent> ConsentRequests { get; private set; } = new List<CodeRequestConsent>();

    public IList<CodeRequestRemark> RemarksRequests { get; private set; } = new List<CodeRequestRemark>();
}
