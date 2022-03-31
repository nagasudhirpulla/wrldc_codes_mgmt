namespace Core.ReportingData.ElementsForDisplay;

public class ReportingTransmissionLineCkt
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CrktNumber { get; set; }
    public int CrktLength { get; set; }
    public string LineVol { get; set; }
    public string CrktOwners { get; set; }
    public string OwnerIds { get; set; }
}
//ckt.ID,
//    ckt.LINE_CIRCUIT_NAME,
//    ckt.CIRCUIT_NUMBER,
//    ckt.LENGTH,
//    line.VOLTAGE,
//    owner_details.OWNERS,
//    owner_details.OWNER_IDS