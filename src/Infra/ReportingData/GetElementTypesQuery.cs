using Core.ReportingData;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData;

internal class GetElementTypesQuery
{
    public static List<ElementType> Execute(string _reportingConnStr)
    {
        List<ElementType> elTypes = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();
        con.Open();
        cmd.CommandText = @"SELECT
                    em.ID,
                    em.ENTITY_NAME AS name
                FROM
                    REPORTING_WEB_UI_UAT.ENTITY_MASTER em
                WHERE
                    em.IS_OUTAGE_ENTITY = 1
                ORDER BY
                    em.ENTITY_NAME";

        OracleDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ElementType? elType = new();
            elType.Id = DbUtils.SafeGetInt(reader, "ID");
            elType.Name = DbUtils.SafeGetString(reader, "NAME");
            elTypes.Add(elType);
        }
        reader.Dispose();

        return elTypes;
    }
}
