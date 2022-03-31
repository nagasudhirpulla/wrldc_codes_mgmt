using Core.ReportingData.ElementsForDisplay;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData.ElementsDisplayQueries;

internal class GetAllLineReactorsQuery
{
    public static List<ReportingLineReactor> Execute(string _reportingConnStr)
    {
        List<ReportingLineReactor> allLineReactors = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();
        con.Open();
        cmd.CommandText = @"SELECT
                            lr.id,
                            lr.reactor_name,
                            lr.mvar_capacity,
                            as2.substation_name,
                            atlc.line_circuit_name,
                            owner_details.owners,
                            owner_details.owner_ids
                        FROM
                            reporting_web_ui_uat.line_reactor                 lr
                            LEFT JOIN reporting_web_ui_uat.associate_substation         as2 ON as2.id = lr.fk_substation
                            LEFT JOIN reporting_web_ui_uat.ac_transmission_line_circuit atlc ON atlc.id = lr.fk_line_id
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
                                    AND ent_reln.parent_entity = 'LINE_REACTOR'
                                    AND ent_reln.child_entity_attribute = 'OwnerId'
                                    AND ent_reln.parent_entity_attribute = 'Owner'
                                GROUP BY
                                    parent_entity_attribute_id
                            )                                                 owner_details ON owner_details.element_id = lr.id";
        OracleDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ReportingLineReactor? lineReactor = new();
            lineReactor.Id = DbUtils.SafeGetInt(reader, "ID");
            lineReactor.Name = DbUtils.SafeGetString(reader, "REACTOR_NAME");
            lineReactor.MvarCapacity = DbUtils.SafeGetInt(reader, "MVAR_CAPACITY");
            lineReactor.SubstationName = DbUtils.SafeGetString(reader, "SUBSTATION_NAME");
            lineReactor.LineCrktName = DbUtils.SafeGetString(reader, "LINE_CIRCUIT_NAME");
            lineReactor.Owners = DbUtils.SafeGetString(reader, "OWNERS");
            lineReactor.OwnerIds = DbUtils.SafeGetString(reader, "OWNER_IDS");
            allLineReactors.Add(lineReactor);
        }
        reader.Dispose();

        return allLineReactors;
    }
}
