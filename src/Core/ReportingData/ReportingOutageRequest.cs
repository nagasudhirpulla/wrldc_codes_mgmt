namespace Core.ReportingData;

public class ReportingOutageRequest
{
    public int ShutdownId { get; set; }
    public int ShutdownRequestId { get; set; }
    public int ElementTypeId { get; set; }
    public int ElementId { get; set; }
    public string? ElementName { get; set; }
    public string? ElementType { get; set; }
    public int ReasonId { get; set; }
    public string? Reason { get; set; }
    public string? OutageType { get; set; }
    public int OutageTypeId { get; set; }
    public string? OutageTag { get; set; }
    public int OutageTagId { get; set; }
    public string? OccName { get; set; }
    public string? Requester { get; set; }
    public string? OutageBasis { get; set; }
    public DateTime? ApprovedStartTime { get; set; }
    public DateTime? ApprovedEndTime { get; set; }
    public string? RequesterRemarks { get; set; }
    public string? AvailingStatus { get; set; }
    public string? ApprovalStatus { get; set; }
    public string? NldcApprovalStatus { get; set; }
    public string? RldcRemarks { get; set; }
    public string? RpcRemarks { get; set; }
    public string? NldcRemarks { get; set; }
}
