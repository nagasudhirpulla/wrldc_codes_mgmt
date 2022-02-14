using Ardalis.SmartEnum;

namespace Core.Enums;

public sealed class ApprovalStatus : SmartEnum<ApprovalStatus>
{
    public static readonly ApprovalStatus Pending = new(nameof(Pending), 1);
    public static readonly ApprovalStatus Approved = new(nameof(Approved), 2);
    public static readonly ApprovalStatus DisApproved = new(nameof(DisApproved), 3);

    private ApprovalStatus(string name, int value) : base(name, value)
    {
    }
}