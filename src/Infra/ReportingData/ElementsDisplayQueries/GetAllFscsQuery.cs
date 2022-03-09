using Core.ReportingData.GetElementsForDisplay;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData.ElementsDisplayQueries;

internal class GetAllFscsQuery
{
    public static List<ReportingFsc> Execute(string _reportingConnStr)
    {
        List<ReportingFsc> allFSCs = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();
        con.Open();
        cmd.CommandText = @"SELECT
                            f.id,
                            f.fsc_name,
                            as2.substation_name,
                            owner_details.owners,
                            owner_details.owner_ids
                        FROM
                            reporting_web_ui_uat.fsc                  f
                            LEFT JOIN reporting_web_ui_uat.associate_substation as2 ON as2.id = f.fk_substation
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
                                    AND ent_reln.parent_entity = 'FSC'
                                    AND ent_reln.child_entity_attribute = 'OwnerId'
                                    AND ent_reln.parent_entity_attribute = 'Owner'
                                GROUP BY
                                    parent_entity_attribute_id
                            )                                         owner_details ON owner_details.element_id = f.id";
        OracleDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ReportingFsc? fsc = new();
            fsc.FSCId = DbUtils.SafeGetInt(reader, "ID");
            fsc.FSCName = DbUtils.SafeGetString(reader, "FSC_NAME");
            fsc.SubstationName = DbUtils.SafeGetString(reader, "SUBSTATION_NAME");
            fsc.Owners = DbUtils.SafeGetString(reader, "OWNERS");
            fsc.OwnerIds = DbUtils.SafeGetString(reader, "OWNER_IDS");
            allFSCs.Add(fsc);
        }
        reader.Dispose();

        return allFSCs;
    }
}
