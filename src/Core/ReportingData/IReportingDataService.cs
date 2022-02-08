﻿namespace Core.ReportingData;

public interface IReportingDataService
{
    List<ReportingStakeholder> GetReportingStakeHolders();
    List<ReportingOwner> GetReportingOwners();
    List<ReportingOutageRequest> GetRequesterApprovedOutageRequestsForDate(int requesterId, DateTime inpDate);
}
