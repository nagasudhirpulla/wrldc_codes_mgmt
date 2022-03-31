namespace Core.ReportingData.ElementsForDisplay;

public class ReportingTransformer
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int MvaCapacity { get; set; }
    public int TypeGtIct { get; set; }
    public string? Owners { get; set; }
    public string? OwnerIds { get; set; }
}
//t.id,
//    t.TRANSFORMER_NAME,
//    t.MVA_CAPACITY,
//    t.TYPE_GT_ICT,
//    owner_details.OWNERS,
//    owner_details.OWNER_IDS

