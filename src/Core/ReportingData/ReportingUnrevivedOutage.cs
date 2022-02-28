namespace Core.ReportingData;

public class ReportingUnrevivedOutage
{
    public int RTOutageId { get; set; }
   
    public int ElementTypeId { get; set; }
    public int ElementId { get; set; }
    public string? ElementName { get; set; }
    public string? ElementType { get; set; }
    public int OutageTypeId { get; set; }
    public string? OutageType { get; set; }
    
    public string? Reason { get; set; }
    public DateTime? OutageDateTime { get; set; }
    
    public string? OutageRemarks { get; set; }
    public int OutageTagId { get; set; }
    public string? OutageTag { get; set; }

}
