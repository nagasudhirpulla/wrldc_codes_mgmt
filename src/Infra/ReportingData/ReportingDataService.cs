using Core.ReportingData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;

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
            outageRequests = GetRequesterApprovedOutageRequestsForDateQuery.Execute(_reportingConnStr, inpDate);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error while fetching reporting approved outage requests for requester, {msg}", ex.Message);
            outageRequests = new();
        }
        return outageRequests;
    }
}
