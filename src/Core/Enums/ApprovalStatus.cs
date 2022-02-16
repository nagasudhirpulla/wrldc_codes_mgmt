using Ardalis.SmartEnum;

namespace Core.Enums;

public sealed class ApprovalStatus : SmartEnum<ApprovalStatus, string>
{
    public static readonly ApprovalStatus Pending = new(nameof(Pending), "PENDING");
    public static readonly ApprovalStatus Approved = new(nameof(Approved), "APPROVED");
    public static readonly ApprovalStatus DisApproved = new(nameof(DisApproved), "DIS_APPROVED");

    private ApprovalStatus(string name, string value) : base(name, value)
    {
    }
}
