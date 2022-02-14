using Core.Common;

namespace Core.Entities;

public class CodeRequestElementOwner : AuditableEntity
{
    public int OwnerId { get; set; }
    public string? OwnerName { get; set; }

    public CodeRequest? CodeRequest { get; set; }
    public int CodeRequestId { get; set; }
}