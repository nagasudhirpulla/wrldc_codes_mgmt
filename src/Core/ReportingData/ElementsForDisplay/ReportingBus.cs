namespace Core.ReportingData.ElementsForDisplay;

public class ReportingBus
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int BusNumber { get; set; }
    public string SubstationName { get; set; }
    public string Voltage { get; set; }
    public string Owners { get; set; }
    public string OwnerIds { get; set; }
}
//b.ID,
//    b.BUS_NAME,
//    b.BUS_NUMBER,
//    vol.TRANS_ELEMENT_TYPE AS VOLTAGE,
//    as2.SUBSTATION_NAME,
//    owner_details.OWNERS,
//    owner_details.OWNER_IDS

