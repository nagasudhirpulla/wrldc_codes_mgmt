using Core.ReportingData;
using Core.ReportingData.GetElementsForDisplay;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData.ElementsDisplayQueries;

internal class GetAllGeneratingUnitsQuery
{
    public static List<ReportingGeneratingUnit> Execute(string _reportingConnStr)
    {
        List<ReportingGeneratingUnit> allGeneratingUnits = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();
        con.Open();
        cmd.CommandText = @"SELECT
                            gu.id,
                            gu.unit_name,
                            gu.unit_number,
                            gu.installed_capacity,
                            gu.mva_capacity,
                            vol.trans_element_type AS generating_voltage,
                            owner_details.owners,
                            owner_details.owner_ids
                        FROM
                            reporting_web_ui_uat.generating_unit           gu
                            LEFT JOIN reporting_web_ui_uat.trans_element_type_master vol ON vol.trans_element_type_id = gu.generating_voltage_kv
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
                                        ent_reln.child_entity = 'Owner'
                                    AND ent_reln.parent_entity = 'GENERATING_STATION'
                                    AND ent_reln.child_entity_attribute = 'OwnerId'
                                    AND ent_reln.parent_entity_attribute = 'Owner'
                                GROUP BY
                                    parent_entity_attribute_id
                            )                                              owner_details ON owner_details.element_id = gu.fk_generating_station";
        OracleDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ReportingGeneratingUnit? gu = new();
            gu.GuId = DbUtils.SafeGetInt(reader, "ID");
            gu.UnitName = DbUtils.SafeGetString(reader, "UNIT_NAME");
            gu.UnitNumber = DbUtils.SafeGetInt(reader, "UNIT_NUMBER");
            gu.InstalledCapacity = DbUtils.SafeGetInt(reader, "INSTALLED_CAPACITY");
            gu.MVACapacity = DbUtils.SafeGetInt(reader, "MVA_CAPACITY");
            gu.GeneratingVol = DbUtils.SafeGetString(reader, "GENERATING_VOLTAGE");
            gu.Owners = DbUtils.SafeGetString(reader, "OWNERS");
            gu.OwnerIds = DbUtils.SafeGetString(reader, "OWNER_IDS");
            allGeneratingUnits.Add(gu);
        }
        reader.Dispose();

        return allGeneratingUnits;
    }
}
