namespace Core.ReportingData;

public interface IReportingDataService
{
    List<ReportingStakeholder> GetReportingStakeHolders();
    List<ReportingOwner> GetReportingOwners();
    List<ReportingOutageRequest> GetApprovedOutageRequestsForDate(DateTime inpDate);
    ReportingOutageRequest? GetApprovedOutageRequestById(int outReqId);
}
