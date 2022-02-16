using Ardalis.SmartEnum;

namespace Core.Enums;

public sealed class CodeType : SmartEnum<CodeType, string>
{
    public static readonly CodeType Generic = new(nameof(Generic), "GENERIC");
    public static readonly CodeType Element = new(nameof(Element), "ELEMENT");
    public static readonly CodeType Outage = new(nameof(Outage), "OUTAGE");
    public static readonly CodeType ApprovedOutage = new(nameof(ApprovedOutage), "APPROVED_OUTAGE");
    public static readonly CodeType Revival = new(nameof(Revival), "REVIVAL");

    private CodeType(string name, string value) : base(name, value)
    {
    }
}