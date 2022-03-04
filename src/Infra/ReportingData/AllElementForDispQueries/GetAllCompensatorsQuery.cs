using Core.ReportingData;
using Core.ReportingData.GetElementsForDisplay;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData.AllElementForDispQueries;

internal class GetAllCompensatorsQuery
{
    public static List<ReportingCompensator> Execute(string _reportingConnStr)
    {
        List<ReportingCompensator> allCompensators = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();
        con.Open();
        cmd.CommandText = @"SELECT
                            t.id,
                            t.tcsc_name,
                            t.perc_variable_compensation,
                            t.perc_fixed_compensation,
                            owner_details.owners,
                            owner_details.owner_ids
                        FROM
                            reporting_web_ui_uat.tcsc t
                            LEFT JOIN (
                                SELECT
                                    LISTAGG(own.owner_name, ',') WITHIN GROUP(
                                    ORDER BY
                                        owner_name
                                    )                          AS owners,
                                    LISTAGG(own.id, ',') WITHIN GROUP(
                                    ORDER BY
                                        owner_name
                                    )                          AS owner_ids,
                                    parent_entity_attribute_id AS element_id
                                FROM
                                    reporting_web_ui_uat.entity_entity_reln ent_reln
                                    LEFT JOIN reporting_web_ui_uat.owner              own ON own.id = ent_reln.child_entity_attribute_id
                                WHERE
                                        ent_reln.child_entity = 'OWNER'
                                    AND ent_reln.parent_entity IN ( 'STATCOM', 'TCSC', 'MSR', 'MSC' )
                                    AND ent_reln.child_entity_attribute = 'OwnerId'
                                    AND ent_reln.parent_entity_attribute = 'Owner'
                                GROUP BY
                                    parent_entity_attribute_id
                            )                         owner_details ON owner_details.element_id = t.id";
        
        OracleDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ReportingCompensator? obj = new();
            obj.CmpstrId = DbUtils.SafeGetInt(reader, "ID");
            obj.TcscName = DbUtils.SafeGetString(reader, "TCSC_NAME");
            obj.PercVarCmpstr = DbUtils.SafeGetInt(reader, "PERC_VARIABLE_COMPENSATION");
            obj.PercFxdCmpstr = DbUtils.SafeGetInt(reader, "PERC_FIXED_COMPENSATION");
            obj.Owners = DbUtils.SafeGetString(reader, "OWNERS");
            obj.OwnerIds = DbUtils.SafeGetString(reader, "OWNER_IDS");
            allCompensators.Add(obj);
        }
        reader.Dispose();

        return allCompensators;
    }
}
