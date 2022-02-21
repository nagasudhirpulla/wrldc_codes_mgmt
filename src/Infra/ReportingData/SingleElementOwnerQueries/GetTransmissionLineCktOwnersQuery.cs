using Core.ReportingData;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData.SingleElementOwnerQueries;

internal class GetTransmissionLineCktOwnersQuery
{
    public static List<ReportingOwner> Execute(string _reportingConnStr, int id)
    {
        List<ReportingOwner> owners = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();
        con.Open();
        cmd.CommandText = @"select own.owner_name, own.id
                    from REPORTING_WEB_UI_UAT.entity_entity_reln ent_reln
                    left join REPORTING_WEB_UI_UAT.owner own on own.id = ent_reln.child_entity_attribute_id
                    where ent_reln.CHILD_ENTITY = 'OWNER'
                        and ent_reln.parent_entity = 'AC_TRANSMISSION_LINE'
                        and ent_reln.CHILD_ENTITY_ATTRIBUTE = 'OwnerId'
                        and ent_reln.PARENT_ENTITY_ATTRIBUTE = 'Owner'
                        and ent_reln.parent_entity_attribute_id = :lineId";

        cmd.Parameters.Add(new OracleParameter("lineId", id));
        OracleDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string uName = reader.GetString(0);
            int uId = reader.GetInt32(1);
            owners.Add(new ReportingOwner(uId, uName));
        }
        reader.Dispose();

        return owners;
    }
}
