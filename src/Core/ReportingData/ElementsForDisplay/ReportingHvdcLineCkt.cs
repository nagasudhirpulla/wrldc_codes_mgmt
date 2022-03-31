namespace Core.ReportingData.ElementsForDisplay;

public class ReportingHvdcLineCkt
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int HlcNumber { get; set; }
    public string LineVoltage { get; set; }
    public string Owners { get; set; }
    public string OwnerIds { get; set; }
}
//hlc.id,
//    hlc.LINE_CIRCUIT_NAME,
//    hlc.CIRCUIT_NO,
//    line.voltage,
//    owner_details.OWNERS,
//    owner_details.OWNER_IDS

