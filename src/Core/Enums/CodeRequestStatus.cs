using Ardalis.SmartEnum;

namespace Core.Enums;

public sealed class CodeRequestStatus : SmartEnum<CodeRequestStatus, string>
{
    public static readonly CodeRequestStatus Requested = new(nameof(Requested), "REQUESTED");
    public static readonly CodeRequestStatus ConsentPending = new(nameof(ConsentPending), "CONSENT_PENDING");
    public static readonly CodeRequestStatus RemarksPending = new(nameof(RemarksPending), "REMARKS_PENDING");
    public static readonly CodeRequestStatus ConsentApproved = new(nameof(ConsentApproved), "CONSENT_APPROVED");
    public static readonly CodeRequestStatus RemarksGiven = new(nameof(RemarksGiven), "REMARKS_GIVEN");
    public static readonly CodeRequestStatus Approved = new(nameof(Approved), "APPROVED");
    public static readonly CodeRequestStatus DisApproved = new(nameof(DisApproved), "DIS_APPROVED");

    private CodeRequestStatus(string name, string value) : base(name, value)
    {
    }
}
