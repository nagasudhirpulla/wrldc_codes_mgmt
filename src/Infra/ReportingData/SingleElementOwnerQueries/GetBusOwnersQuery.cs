using Core.ReportingData;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData.SingleElementOwnerQueries;

internal class GetBusOwnersQuery
{
    public static List<ReportingOwner> Execute(string _reportingConnStr, int id)
    {
        List<ReportingOwner> owners = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();
        con.Open();
        cmd.CommandText = @"SELECT own.id,own.owner_name
                            FROM reporting_web_ui_uat.entity_entity_reln ent_reln
                                LEFT JOIN reporting_web_ui_uat.owner own ON own.id = ent_reln.child_entity_attribute_id
                                LEFT JOIN reporting_web_ui_uat.bus bus ON bus.fk_substation_id = ent_reln.parent_entity_attribute_id
                            WHERE ent_reln.child_entity = 'OWNER'
                                AND ent_reln.parent_entity = 'ASSOCIATE_SUBSTATION'
                                AND ent_reln.child_entity_attribute = 'OwnerId'
                                AND ent_reln.parent_entity_attribute = 'Owner'
                                AND bus.id = :bus_id";
        cmd.Parameters.Add(new OracleParameter("bus_id", id));
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
