using Core.ReportingData.GetElementsForDisplay;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData.ElementsDisplayQueries;

internal class GetAllHvdcPolesQuery
{
    public static List<ReportingHvdcPole> Execute(string _reportingConnStr)
    {
        List<ReportingHvdcPole> allHvdcPoles = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();
        con.Open();
        cmd.CommandText = @"SELECT
                            hp.id,
                            hp.pole_name,
                            stn.substation_name,
                            stn.voltage,
                            owner_details.owners,
                            owner_details.owner_ids
                        FROM
                            reporting_web_ui_uat.hvdc_pole hp
                            LEFT JOIN (
                                SELECT
                                    as2.*,
                                    vol.trans_element_type AS voltage
                                FROM
                                    reporting_web_ui_uat.associate_substation      as2
                                    LEFT JOIN reporting_web_ui_uat.trans_element_type_master vol ON vol.trans_element_type_id = as2.voltage_level
                            )                              stn ON stn.id = hp.fk_substation
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
                                    AND ent_reln.parent_entity = 'HVDC_POLE'
                                    AND ent_reln.child_entity_attribute = 'OwnerId'
                                    AND ent_reln.parent_entity_attribute = 'Owner'
                                GROUP BY
                                    parent_entity_attribute_id
                            )                              owner_details ON owner_details.element_id = hp.id";
        OracleDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ReportingHvdcPole? hvdcPole = new();
            hvdcPole.HVDCPId = DbUtils.SafeGetInt(reader, "ID");
            hvdcPole.PoleName = DbUtils.SafeGetString(reader, "POLE_NAME");
            hvdcPole.SubstationName = DbUtils.SafeGetString(reader, "SUBSTATION_NAME");
            hvdcPole.Voltage = DbUtils.SafeGetString(reader, "VOLTAGE");
            hvdcPole.Owners = DbUtils.SafeGetString(reader, "OWNERS");
            hvdcPole.OwnerIds = DbUtils.SafeGetString(reader, "OWNER_IDS");
            allHvdcPoles.Add(hvdcPole);
        }
        reader.Dispose();

        return allHvdcPoles;
    }
}
