﻿using Core.Common;
using Core.Enums;

namespace Core.Entities;

public class CodeRequest : AuditableEntity
{
    public CodeRequestStatus RequestState { get; set; } = CodeRequestStatus.Requested;

    public ApplicationUser? Requester { get; set; }
    public string? RequesterId { get; set; }

    public string? Description { get; set; }

    public int? ElementId { get; set; }
    public string? ElementName { get; set; }

    public int? ElementTypeId { get; set; }
    public string? ElementType { get; set; }

    public int? OutageTypeId { get; set; }
    public string? OutageType { get; set; }

    public int? OutageTagId { get; set; }
    public string? OutageTag { get; set; }

    public int? OutageAprovalId { get; set; }
    
    public DateTime? DesiredExecutionTime { get; set; }

    public IList<CodeRequestElementOwner> ElementOwners { get; private set; } = new List<CodeRequestElementOwner>();

    public IList<CodeRequestStakeHolder> ConcernedStakeholders { get; private set; } = new List<CodeRequestStakeHolder>();

    public IList<CodeRequestConsent> ConsentRequests { get; private set; } = new List<CodeRequestConsent>();

    public IList<CodeRequestRemark> RemarksRequests { get; private set; } = new List<CodeRequestRemark>();
}
