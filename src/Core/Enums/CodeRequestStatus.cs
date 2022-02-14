using Ardalis.SmartEnum;

namespace Core.Enums;

public sealed class CodeRequestStatus : SmartEnum<CodeRequestStatus>
{
    public static readonly CodeRequestStatus Requested = new(nameof(Requested), 1);
    public static readonly CodeRequestStatus ConsentPending = new(nameof(ConsentPending), 2);
    public static readonly CodeRequestStatus RemarksPending = new(nameof(RemarksPending), 3);
    public static readonly CodeRequestStatus ConsentApproved = new(nameof(ConsentApproved), 4);
    public static readonly CodeRequestStatus RemarksGiven = new(nameof(RemarksGiven), 5);
    public static readonly CodeRequestStatus Approved = new(nameof(Approved), 6);
    public static readonly CodeRequestStatus DisApproved = new(nameof(DisApproved), 7);

    private CodeRequestStatus(string name, int value) : base(name, value)
    {
    }
}
