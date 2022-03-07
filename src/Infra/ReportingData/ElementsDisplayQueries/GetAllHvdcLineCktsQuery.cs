using Core.ReportingData;
using Core.ReportingData.GetElementsForDisplay;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData.ElementsDisplayQueries;

internal class GetAllHvdcLineCktsQuery
{
    public static List<ReportingHvdcLineCkt> Execute(string _reportingConnStr)
    {
        List<ReportingHvdcLineCkt> allHvdcLineCkts = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();
        con.Open();
        cmd.CommandText = @"SELECT
                            hlc.id,
                            hlc.line_circuit_name,
                            hlc.circuit_no,
                            line.voltage,
                            owner_details.owners,
                            owner_details.owner_ids
                        FROM
                            reporting_web_ui_uat.hvdc_line_circuit hlc
                            LEFT JOIN (
                                SELECT
                                    hlm.*,
                                    vol.trans_element_type AS voltage
                                FROM
                                    reporting_web_ui_uat.hvdc_line_master          hlm
                                    LEFT JOIN reporting_web_ui_uat.trans_element_type_master vol ON vol.trans_element_type_id = hlm.from_voltage
                            )                                      line ON line.id = hlc.hvdc_line_id
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
                                    AND ent_reln.parent_entity = 'HVDC_LINE'
                                    AND ent_reln.child_entity_attribute = 'OwnerId'
                                    AND ent_reln.parent_entity_attribute = 'Owner'
                                GROUP BY
                                    parent_entity_attribute_id
                            )                                      owner_details ON owner_details.element_id = hlc.id";
        OracleDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ReportingHvdcLineCkt? hvdcLineCkt = new();
            hvdcLineCkt.HlcId = DbUtils.SafeGetInt(reader, "ID");
            hvdcLineCkt.HlcName = DbUtils.SafeGetString(reader, "LINE_CIRCUIT_NAME");
            hvdcLineCkt.HlcNumber = DbUtils.SafeGetInt(reader, "CIRCUIT_NO");
            hvdcLineCkt.LineVoltage = DbUtils.SafeGetString(reader, "VOLTAGE");
            hvdcLineCkt.Owners = DbUtils.SafeGetString(reader, "OWNERS");
            hvdcLineCkt.OwnerIds = DbUtils.SafeGetString(reader, "OWNER_IDS");
            allHvdcLineCkts.Add(hvdcLineCkt);
        }
        reader.Dispose();

        return allHvdcLineCkts;
    }
}
