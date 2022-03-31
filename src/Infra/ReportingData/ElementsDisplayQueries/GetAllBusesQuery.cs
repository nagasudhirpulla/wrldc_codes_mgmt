using Core.ReportingData.ElementsForDisplay;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData.ElementsDisplayQueries;

internal class GetAllBusesQuery
{
    public static List<ReportingBus> Execute(string _reportingConnStr)
    {
        List<ReportingBus> allBuses = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();
        con.Open();
        cmd.CommandText = @"SELECT
                            b.id,
                            b.bus_name,
                            b.bus_number,
                            vol.trans_element_type AS voltage,
                            as2.substation_name,
                            owner_details.owners,
                            owner_details.owner_ids
                        FROM
                            reporting_web_ui_uat.bus                       b
                            LEFT JOIN reporting_web_ui_uat.associate_substation      as2 ON as2.id = b.fk_substation_id
                            LEFT JOIN reporting_web_ui_uat.trans_element_type_master vol ON vol.trans_element_type_id = b.voltage
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
                                    AND ent_reln.parent_entity = 'ASSOCIATE_SUBSTATION'
                                    AND ent_reln.child_entity_attribute = 'OwnerId'
                                    AND ent_reln.parent_entity_attribute = 'Owner'
                                GROUP BY
                                    parent_entity_attribute_id
                            )                                              owner_details ON owner_details.element_id = as2.id";
        OracleDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ReportingBus? bus = new();
            bus.Id = DbUtils.SafeGetInt(reader, "ID");
            bus.Name = DbUtils.SafeGetString(reader, "BUS_NAME");
            bus.BusNumber = DbUtils.SafeGetInt(reader, "BUS_NUMBER");
            bus.SubstationName = DbUtils.SafeGetString(reader, "SUBSTATION_NAME");
            bus.Voltage = DbUtils.SafeGetString(reader, "VOLTAGE");
            bus.Owners = DbUtils.SafeGetString(reader, "OWNERS");
            bus.OwnerIds = DbUtils.SafeGetString(reader, "OWNER_IDS");
            allBuses.Add(bus);
        }
        reader.Dispose();
        return allBuses;
    }
}
