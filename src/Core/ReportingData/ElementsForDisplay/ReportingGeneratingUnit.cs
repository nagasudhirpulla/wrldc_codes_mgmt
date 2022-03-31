namespace Core.ReportingData.ElementsForDisplay;

public class ReportingGeneratingUnit
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int UnitNumber { get; set; }
    public int InstalledCapacity { get; set; }
    public int MVACapacity { get; set; }
    public string GeneratingVol { get; set; }
    public string Owners { get; set; }
    public string OwnerIds { get; set; }
}
//gu.ID,
//    gu.UNIT_NAME,
//    gu.UNIT_NUMBER,
//    gu.INSTALLED_CAPACITY,
//    gu.MVA_CAPACITY,
//    vol.TRANS_ELEMENT_TYPE AS GENERATING_VOLTAGE,
//    owner_details.OWNERS,
//    owner_details.OWNER_IDS

