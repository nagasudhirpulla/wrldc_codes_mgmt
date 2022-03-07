using Core.ReportingData.GetElementsForDisplay;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData.ElementsDisplayQueries;

internal class GetAllBaysQuery
{
    public static List<ReportingBay> Execute(string _reportingConnStr)
    {
        List<ReportingBay> allBays = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();
        con.Open();
        cmd.CommandText = @"SELECT
                            b.id,
                            b.bay_name,
                            b.bay_number,
                            as2.substation_name,
                            bt.type,
                            vol.trans_element_type AS voltage,
                            owner_details.owners,
                            owner_details.owner_ids
                        FROM
                            reporting_web_ui_uat.bay                       b
                            LEFT JOIN reporting_web_ui_uat.bay_type                  bt ON bt.id = b.bay_type_id
                            LEFT JOIN reporting_web_ui_uat.associate_substation      as2 ON as2.id = b.station_id
                            LEFT JOIN reporting_web_ui_uat.trans_element_type_master vol ON vol.trans_element_type_id = b.voltage_id
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
                                    AND ent_reln.parent_entity = 'BAY'
                                    AND ent_reln.child_entity_attribute = 'OwnerId'
                                    AND ent_reln.parent_entity_attribute = 'Owner'
                                GROUP BY
                                    parent_entity_attribute_id
                            )                                              owner_details ON owner_details.element_id = b.id";

        OracleDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ReportingBay? bay = new();
            bay.BayId = DbUtils.SafeGetInt(reader, "ID");
            bay.BayName = DbUtils.SafeGetString(reader, "BAY_NAME");
            //Bay number is string
            bay.BayNumber = DbUtils.SafeGetString(reader, "BAY_NUMBER");
            bay.SubstationName = DbUtils.SafeGetString(reader, "SUBSTATION_NAME");
            bay.Type = DbUtils.SafeGetString(reader, "TYPE");
            bay.Voltage = DbUtils.SafeGetString(reader, "VOLTAGE");
            bay.Owners = DbUtils.SafeGetString(reader, "OWNERS");
            bay.OwnerIds = DbUtils.SafeGetString(reader, "OWNER_IDS");
            allBays.Add(bay);
        }
        reader.Dispose();

        return allBays;
    }
}
