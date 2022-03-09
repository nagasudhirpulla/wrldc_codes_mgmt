using Core.ReportingData.GetElementsForDisplay;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData.ElementsDisplayQueries;

internal class GetAllTransmissionLineCktsQuery
{
    public static List<ReportingTransmissionLineCkt> Execute(string _reportingConnStr)
    {
        List<ReportingTransmissionLineCkt> allTransmissionLineCkts = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();
        con.Open();
        cmd.CommandText = @"SELECT
                            ckt.id,
                            ckt.line_circuit_name,
                            ckt.circuit_number,
                            ckt.length,
                            line.voltage,
                            owner_details.owners,
                            owner_details.owner_ids
                        FROM
                            reporting_web_ui_uat.ac_transmission_line_circuit ckt
                            LEFT JOIN (
                                SELECT
                                    lm.id,
                                    vol.trans_element_type AS voltage
                                FROM
                                    reporting_web_ui_uat.ac_trans_line_master      lm
                                    LEFT JOIN reporting_web_ui_uat.trans_element_type_master vol ON vol.trans_element_type_id = lm.voltage_level
                            )                                                 line ON line.id = ckt.line_id
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
                                    AND ent_reln.parent_entity = 'AC_TRANSMISSION_LINE'
                                    AND ent_reln.child_entity_attribute = 'OwnerId'
                                    AND ent_reln.parent_entity_attribute = 'Owner'
                                GROUP BY
                                    parent_entity_attribute_id
                            )owner_details ON owner_details.element_id = ckt.id";

        OracleDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ReportingTransmissionLineCkt? transLineCkt = new();
            transLineCkt.CrktId = DbUtils.SafeGetInt(reader, "ID");
            transLineCkt.LineCrktName = DbUtils.SafeGetString(reader, "LINE_CIRCUIT_NAME");
            transLineCkt.CrktNumber = DbUtils.SafeGetInt(reader, "CIRCUIT_NUMBER");
            transLineCkt.CrktLength = DbUtils.SafeGetInt(reader, "LENGTH");
            transLineCkt.LineVol = DbUtils.SafeGetString(reader, "VOLTAGE");
            transLineCkt.CrktOwners = DbUtils.SafeGetString(reader, "OWNERS");
            transLineCkt.OwnerIds = DbUtils.SafeGetString(reader, "OWNER_IDS");
            allTransmissionLineCkts.Add(transLineCkt);
        }
        reader.Dispose();

        return allTransmissionLineCkts;
    }
}
