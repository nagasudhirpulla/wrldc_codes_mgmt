namespace Core.ReportingData.ElementsForDisplay;

public class ReportingBay
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string BayNumber { get; set; }
    public string SubstationName { get; set; }
    public string Type { get; set; }
    public string Voltage { get; set; }
    public string Owners { get; set; }
    public string OwnerIds { get; set; }
}
//b.ID,
//    b.BAY_NAME,
//    b.BAY_NUMBER,
//    as2.SUBSTATION_NAME,
//    bt.TYPE,
//    vol.TRANS_ELEMENT_TYPE AS VOLTAGE,
//    owner_details.OWNERS,
//    owner_details.OWNER_IDS
