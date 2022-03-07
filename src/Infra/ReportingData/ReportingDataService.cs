﻿using Core.ReportingData;
using Core.ReportingData.GetElementsForDisplay;
using Infra.ReportingData.SingleElementOwnerQueries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using Infra.ReportingData.ElementsDisplayQueries;

namespace Infra.ReportingData;

public class ReportingDataService : IReportingDataService
{
    private readonly string _reportingConnStr;
    private readonly ILogger<ReportingDataService> _logger;
    public ReportingDataService(IConfiguration configuration, ILogger<ReportingDataService> logger)
    {
        _reportingConnStr = configuration["ConnectionStrings:ReportingConnection"];
        _logger = logger;
    }
    public List<ReportingStakeholder> GetReportingStakeHolders()
    {
        List<ReportingStakeholder> stakeholders = new();
        // https://www.oracle.com/tools/technologies/quickstart-dotnet-for-oracle-database.html
        // https://stackoverflow.com/questions/11048910/oraclecommand-sql-parameters-binding
        using (OracleConnection con = new(_reportingConnStr))
        {
            using (OracleCommand cmd = con.CreateCommand())
            {
                try
                {
                    con.Open();
                    cmd.CommandText = @"SELECT USERID, USER_NAME
                                        FROM REPORTING_WEB_UI_UAT.USER_DETAILS ud
                                        WHERE ud.IS_SHUTDOWN_USER = 1";
                    OracleDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        int uId = reader.GetInt32(0);
                        string uName = reader.GetString(1);
                        stakeholders.Add(new ReportingStakeholder(uId, uName));
                    }

                    reader.Dispose();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error while fetching reporting data stakeholders, {msg}", ex.Message);
                    stakeholders = new();
                }
            }
        }
        return stakeholders;
    }
    public List<ReportingOwner> GetReportingOwners()
    {
        List<ReportingOwner> owners;
        try
        {
            owners = GetReportingOwnersQuery.Execute(_reportingConnStr);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting data owners, {msg}", ex.Message);
            owners = new();
        }
        return owners;
    }
    public List<ReportingOutageRequest> GetApprovedOutageRequestsForDate(DateTime inpDate)
    {
        List<ReportingOutageRequest> outageRequests;
        try
        {
            outageRequests = GetApprovedOutageRequestsQuery.Execute(_reportingConnStr, inpDate);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for date, {msg}", ex.Message);
            outageRequests = new();
        }
        return outageRequests;
    }
    public ReportingOutageRequest? GetApprovedOutageRequestById(int outReqId)
    {
        ReportingOutageRequest? outageRequest = null;
        try
        {
            var outageRequests = GetApprovedOutageRequestsQuery.Execute(_reportingConnStr, null, outReqId);
            if (outageRequests != null && outageRequests.Count == 1)
            {
                // set the result if a single outage request is found
                outageRequest = outageRequests[0];
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage request by id, {msg}", ex.Message);
            outageRequest = null;
        }
        return outageRequest;
    }
    public List<ReportingOwner> GetElementOwners(string elType, int id)
    {
        // TODO create tests
        /*
        GENERATING STATION
        AC TRANSMISSION LINE
        AC_TRANSMISSION_LINE_CIRCUIT
        AUTO RECLOSURE
        BUS
        BUS REACTOR
        Bay
        FSC
        Filter Bank
        HVDC LINE
        HVDC POLE
        HVDC_LINE_CIRCUIT
        LINE_REACTOR
        SVC
        Sub Filter Bank
        TCSC
        TRANSFORMER     
         */
        List<ReportingOwner> elementOwners = new();
        try
        {
            if (elType == "TRANSFORMER")
            {
                elementOwners = GetTransformerOwnersQuery.Execute(_reportingConnStr, id);
            }
            else if (elType == "Bay")
            {
                elementOwners = GetBayOwnersQuery.Execute(_reportingConnStr, id);
            }
            else if (elType == "BUS")
            {
                elementOwners = GetBusOwnersQuery.Execute(_reportingConnStr, id);
            }
            else if (elType == "BUS REACTOR")
            {
                elementOwners = GetBusReactorOwnersQuery.Execute(_reportingConnStr, id);
            }
            else if (elType == "FSC")
            {
                elementOwners = GetFSCOwnersQuery.Execute(_reportingConnStr, id);
            }
            else if (elType == "GENERATING_STATION")
            {
                elementOwners = GetGeneratingUnitOwnersQuery.Execute(_reportingConnStr, id);
            }
            else if (elType == "HVDC_LINE_CIRCUIT")
            {
                elementOwners = GetHVDCLineCktOwnersQuery.Execute(_reportingConnStr, id);
            }
            else if (elType == "HVDC POLE")
            {
                elementOwners = GetHVDCPoleOwnersQuery.Execute(_reportingConnStr, id);
            }
            else if (elType == "LINE_REACTOR")
            {
                elementOwners = GetLineReactorOwnersQuery.Execute(_reportingConnStr, id);
            }
            else if (elType == "AC_TRANSMISSION_LINE_CIRCUIT")
            {
                elementOwners = GetTransmissionLineCktOwnersQuery.Execute(_reportingConnStr, id);
            }
            else if (new List<string>() { "TCSC", "MSR", "MSC" }.Any(x => x == elType))
            {
                elementOwners = GetCompensatorOwnersQuery.Execute(_reportingConnStr, id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            elementOwners = new();
        }
        return elementOwners;
    }
    public List<ReportingOwner> GetTransmissionLineCktOwners(int id)
    {
        List<ReportingOwner> elementowners;
        try
        {
            elementowners = GetTransmissionLineCktOwnersQuery.Execute(_reportingConnStr, id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            elementowners = new();
        }
        return elementowners;
    }
    public List<ReportingOwner> GetBayOwners(int id)
    {
        List<ReportingOwner> elementowners;
        try
        {
            elementowners = GetBayOwnersQuery.Execute(_reportingConnStr, id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            elementowners = new();
        }
        return elementowners;
    }
    public List<ReportingOwner> GetBusOwners(int id)
    {
        List<ReportingOwner> elementowners;
        try
        {
            elementowners = GetBusOwnersQuery.Execute(_reportingConnStr, id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            elementowners = new();
        }
        return elementowners;
    }
    public List<ReportingOwner> GetBusReactorOwners(int id)
    {
        List<ReportingOwner> elementowners;
        try
        {
            elementowners = GetBusReactorOwnersQuery.Execute(_reportingConnStr, id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            elementowners = new();
        }
        return elementowners;
    }
    public List<ReportingOwner> GetCompensatorOwners(int id)
    {
        List<ReportingOwner> elementowners;
        try
        {
            elementowners = GetCompensatorOwnersQuery.Execute(_reportingConnStr, id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            elementowners = new();
        }
        return elementowners;
    }
    public List<ReportingOwner> GetFSCOwners(int id)
    {
        List<ReportingOwner> elementowners;
        try
        {
            elementowners = GetFSCOwnersQuery.Execute(_reportingConnStr, id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            elementowners = new();
        }
        return elementowners;
    }
    public List<ReportingOwner> GetGeneratingUnitOwners(int id)
    {
        List<ReportingOwner> elementowners;
        try
        {
            elementowners = GetGeneratingUnitOwnersQuery.Execute(_reportingConnStr, id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            elementowners = new();
        }
        return elementowners;
    }
    public List<ReportingOwner> GetHVDCLineCktOwners(int id)
    {
        List<ReportingOwner> elementowners;
        try
        {
            elementowners = GetHVDCLineCktOwnersQuery.Execute(_reportingConnStr, id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            elementowners = new();
        }
        return elementowners;
    }
    public List<ReportingOwner> GetHVDCPoleOwners(int id)
    {
        List<ReportingOwner> elementowners;
        try
        {
            elementowners = GetHVDCPoleOwnersQuery.Execute(_reportingConnStr, id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            elementowners = new();
        }
        return elementowners;
    }
    public List<ReportingOwner> GetLineReactorOwners(int id)
    {
        List<ReportingOwner> elementowners;
        try
        {
            elementowners = GetLineReactorOwnersQuery.Execute(_reportingConnStr, id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            elementowners = new();
        }
        return elementowners;
    }
    public List<ReportingOwner> GetTransformerOwners(int id)
    {
        List<ReportingOwner> elementowners;
        try
        {
            elementowners = GetTransformerOwnersQuery.Execute(_reportingConnStr, id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            elementowners = new();
        }
        return elementowners;
    }

    public List<ReportingOutage> GetLatestUnrevivedOutages()
    {
        List<ReportingOutage> unrevivedOutages;
        try
        {
            unrevivedOutages = GetLatestOutagesQuery.Execute(_reportingConnStr, true, null);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            unrevivedOutages = new();
        }
        return unrevivedOutages;
    }

    public ReportingOutage? GetLatestOutageById(int outageId)
    {
        ReportingOutage? outage = null;
        try
        {
            List<ReportingOutage>? outages = GetLatestOutagesQuery.Execute(_reportingConnStr, false, outageId);
            if (outages != null && outages.Count == 1)
            {
                outage = outages[0];
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting outage by Id, {msg}", ex.Message);
            outage = null;
        }
        return outage;
    }

    public List<ReportingBay> GetAllBays()
    {
        List<ReportingBay> allBays;
        try
        {
            allBays = GetAllBaysQuery.Execute(_reportingConnStr);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            allBays = new();
        }
        return allBays;
    }

    public List<ReportingBus> GetAllBuses()
    {
        List<ReportingBus> allBuses;
        try
        {
            allBuses = GetAllBusesQuery.Execute(_reportingConnStr);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            allBuses = new();
        }
        return allBuses;
    }

    public List<ReportingBusReactor> GetAllBusReactors()
    {
        List<ReportingBusReactor> allBusReactors;
        try
        {
            allBusReactors = GetAllBusReactorsQuery.Execute(_reportingConnStr);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            allBusReactors = new();
        }
        return allBusReactors;
    }

    public List<ReportingFsc> GetAllFscs()
    {
        List<ReportingFsc> allFSCs;
        try
        {
            allFSCs = GetAllFscsQuery.Execute(_reportingConnStr);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            allFSCs = new();
        }
        return allFSCs;
    }

    public List<ReportingGeneratingUnit> GetAllGeneratingUnits()
    {
        List<ReportingGeneratingUnit> allGeneratingUnits;
        try
        {
            allGeneratingUnits = GetAllGeneratingUnitsQuery.Execute(_reportingConnStr);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            allGeneratingUnits = new();
        }
        return allGeneratingUnits;
    }

    public List<ReportingHvdcLineCkt> GetAllHvdcLineCkts()
    {
        List<ReportingHvdcLineCkt> allHVDCLineCrkts;
        try
        {
            allHVDCLineCrkts = GetAllHvdcLineCktsQuery.Execute(_reportingConnStr);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            allHVDCLineCrkts = new();
        }
        return allHVDCLineCrkts;
    }

    public List<ReportingHvdcPole> GetAllHvdcPoles()
    {
        List<ReportingHvdcPole> allHVDCPoles;
        try
        {
            allHVDCPoles = GetAllHvdcPolesQuery.Execute(_reportingConnStr);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            allHVDCPoles = new();
        }
        return allHVDCPoles;
    }

    public List<ReportingLineReactor> GetAllLineReactors()
    {
        List<ReportingLineReactor> allLineReactors;
        try
        {
            allLineReactors = GetAllLineReactorsQuery.Execute(_reportingConnStr);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            allLineReactors = new();
        }
        return allLineReactors;
    }

    public List<ReportingTransformer> GetAllTransformers()
    {
        List<ReportingTransformer> allTransformers;
        try
        {
            allTransformers = GetAllTransformersQuery.Execute(_reportingConnStr);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            allTransformers = new();
        }
        return allTransformers;
    }

    public List<ReportingTransmissionLineCkt> GetAllTransmissionLineCkts()
    {
        List<ReportingTransmissionLineCkt> allTransmissionLineCkts;
        try
        {
            allTransmissionLineCkts = GetAllTransmissionLineCktsQuery.Execute(_reportingConnStr);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            allTransmissionLineCkts = new();
        }
        return allTransmissionLineCkts;
    }

    public List<ReportingCompensator> GetAllCompensators()
    {
        List<ReportingCompensator> allCompensators;
        try
        {
            allCompensators = GetAllCompensatorsQuery.Execute(_reportingConnStr);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            allCompensators = new();
        }
        return allCompensators;
    }
}
