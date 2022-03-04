namespace Core.ReportingData.GetElementsForDisplay;

public class ReportingLineReactor
{
    public int LrId  { get; set; }
    public string ReactorName  { get; set; }
    public int MvarCapacity  { get; set; }
    public string SubstationName  { get; set; }
    public string LineCrktName  { get; set; }
    public string Owners  { get; set; }
    public string OwnerIds  { get; set; }
}
//lr.ID,
//    lr.REACTOR_NAME,
//    lr.MVAR_CAPACITY,
//    as2.SUBSTATION_NAME,
//    ATLC.LINE_CIRCUIT_NAME,
//    owner_details.OWNERS,
//    owner_details.OWNER_IDS

