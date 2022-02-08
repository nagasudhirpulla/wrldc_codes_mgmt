using Core.ReportingData;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData;

internal static class GetReportingOwnersQuery
{
    public static List<ReportingOwner> Execute(string _reportingConnStr)
    {
        List<ReportingOwner> owners = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();
        con.Open();
        cmd.CommandText = @"SELECT ID, OWNER_NAME
                                        FROM REPORTING_WEB_UI_UAT.owner";

        OracleDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            int uId = reader.GetInt32(0);
            string uName = reader.GetString(1);
            owners.Add(new ReportingOwner(uId, uName));
        }
        reader.Dispose();

        return owners;
    }
}
