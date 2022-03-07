using Core.ReportingData.GetElementsForDisplay;

namespace Core.ReportingData;

public interface IReportingDataService
{
    List<ReportingStakeholder> GetReportingStakeHolders();
    List<ReportingOwner> GetReportingOwners();
    List<ReportingOutageRequest> GetApprovedOutageRequestsForDate(DateTime inpDate);
    ReportingOutageRequest? GetApprovedOutageRequestById(int outReqId);
    List<ReportingOwner> GetElementOwners(string elType, int id);
    List<ReportingOwner> GetTransmissionLineCktOwners(int id);
    List<ReportingOwner> GetBayOwners(int id);
    List<ReportingOwner> GetBusOwners(int id);
    List<ReportingOwner> GetBusReactorOwners(int id);
    List<ReportingOwner> GetCompensatorOwners(int id);
    List<ReportingOwner> GetFSCOwners(int id);
    List<ReportingOwner> GetGeneratingUnitOwners(int id);
    List<ReportingOwner> GetHVDCLineCktOwners(int id);
    List<ReportingOwner> GetHVDCPoleOwners(int id);
    List<ReportingOwner> GetLineReactorOwners(int id);
    List<ReportingOwner> GetTransformerOwners(int id);
    List<ReportingOutage> GetLatestUnrevivedOutages();
    ReportingOutage? GetLatestOutageById(int outageId);



    List<ReportingBay> GetAllBays();
    List<ReportingBus> GetAllBuses();
    List<ReportingBusReactor> GetAllBusReactors();
    List<ReportingCompensator> GetAllCompensators();
    List<ReportingFsc> GetAllFscs();
    List<ReportingGeneratingUnit> GetAllGeneratingUnits();
    List<ReportingHvdcLineCkt> GetAllHvdcLineCkts();
    List<ReportingHvdcPole> GetAllHvdcPoles();
    List<ReportingLineReactor> GetAllLineReactors();
    List<ReportingTransformer> GetAllTransformers();
    List<ReportingTransmissionLineCkt> GetAllTransmissionLineCkts();

}
