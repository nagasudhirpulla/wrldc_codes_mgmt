namespace Core.ReportingData.GetElementsForDisplay;

public class ReportingTransformer
{
    public int TId  { get; set; }
    public string? TransformerName  { get; set; }
    public int MvaCapacity  { get; set; }
    public int TypeGtIct  { get; set; }
    public string? Owners  { get; set; }
    public string? OwnerIds  { get; set; }
}
//t.id,
//    t.TRANSFORMER_NAME,
//    t.MVA_CAPACITY,
//    t.TYPE_GT_ICT,
//    owner_details.OWNERS,
//    owner_details.OWNER_IDS

