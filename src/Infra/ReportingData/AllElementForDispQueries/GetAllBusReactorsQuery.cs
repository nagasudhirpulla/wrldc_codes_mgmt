using Core.ReportingData;
using Core.ReportingData.GetElementsForDisplay;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData.AllElementForDispQueries;

internal class GetAllBusReactorsQuery
{
    public static List<ReportingBusReactor> Execute(string _reportingConnStr)
    {
        List<ReportingBusReactor> allBusReactors = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();
        con.Open();
        cmd.CommandText = @"SELECT
                            br.id,
                            br.reactor_name,
                            br.mvar_capacity,
                            as2.substation_name,
                            owner_details.owners,
                            owner_details.owner_ids
                        FROM
                            reporting_web_ui_uat.bus_reactor          br
                            LEFT JOIN reporting_web_ui_uat.associate_substation as2 ON as2.id = br.fk_substation
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
                                    AND ent_reln.parent_entity = 'BUS_REACTOR'
                                    AND ent_reln.child_entity_attribute = 'OwnerId'
                                    AND ent_reln.parent_entity_attribute = 'Owner'
                                GROUP BY
                                    parent_entity_attribute_id
                            )                                         owner_details ON owner_details.element_id = br.id";
        OracleDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ReportingBusReactor? obj = new();
            obj.BrId = DbUtils.SafeGetInt(reader, "ID");
            obj.BReactorName = DbUtils.SafeGetString(reader, "REACTOR_NAME");
            obj.MvarCapacity = DbUtils.SafeGetString(reader, "MVAR_CAPACITY");
            obj.SubstationName = DbUtils.SafeGetString(reader, "SUBSTATION_NAME");
            obj.Owners = DbUtils.SafeGetString(reader, "OWNERS");
            obj.OwnerIds = DbUtils.SafeGetString(reader, "OWNER_IDS");
            allBusReactors.Add(obj);
        }
        reader.Dispose();

        return allBusReactors;
    }
}
