using Core.ReportingData;

using Oracle.ManagedDataAccess.Client;
using System.Globalization;

namespace Infra.ReportingData;

internal class GetLatestUnrevivedOutagesQuery
{
    public static List<ReportingUnrevivedOutage> Execute(string _reportingConnStr)
    {
        List<ReportingUnrevivedOutage> unrevOutages = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();
        con.Open();
        cmd.CommandText = @"SELECT
    rto.id,
    rto.entity_id,
    rto.element_id,
    rto.entity_name        AS elementtype,
    rto.shut_down_type_id,
    rto.shut_down_type_name,
    rto.elementname,
    trunc(rto.outage_date) AS outage_date,
    rto.outage_time,
    rto.reason,
    rto.shutdown_tag,
    rto.shutdown_tag_id,
    rto.outage_remarks
FROM
         (
        SELECT
            outages.id,
            outages.entity_id,
            outages.element_id,
            outages.elementname,
            outages.shutdown_tag_id,
            outages.outage_remarks,
            outages.revived_time,
            outages.outage_time,
            outages.outage_date,
            ent_master.entity_name,
            reas.reason,
            sd_type.id             AS shut_down_type_id,
            sd_type.name           AS shut_down_type_name,
            sd_tag.name            AS shutdown_tag,
            to_char(outages.outage_date, 'YYYY-MM-DD')
            || ' '
            || outages.outage_time AS out_date_time
        FROM
            reporting_web_ui_uat.real_time_outage     outages
            LEFT JOIN reporting_web_ui_uat.outage_reason        reas ON reas.id = outages.reason_id
            LEFT JOIN reporting_web_ui_uat.entity_master        ent_master ON ent_master.id = outages.entity_id
            LEFT JOIN reporting_web_ui_uat.shutdown_outage_tag  sd_tag ON sd_tag.id = outages.shutdown_tag_id
            LEFT JOIN reporting_web_ui_uat.shutdown_outage_type sd_type ON sd_type.id = outages.shut_down_type
    ) rto
    INNER JOIN (
        SELECT
            element_id,
            entity_id,
            MAX(to_char(outage_date, 'YYYY-MM-DD')
                || ' '
                || outage_time) AS out_date_time
        FROM
            reporting_web_ui_uat.real_time_outage
        GROUP BY
            entity_id,
            element_id
    ) latest_out_info ON ( ( latest_out_info.entity_id = rto.entity_id )
                           AND ( latest_out_info.element_id = rto.element_id )
                           AND ( latest_out_info.out_date_time = rto.out_date_time ) )
WHERE
    rto.revived_time IS NULL
ORDER BY
    rto.out_date_time DESC
";

        OracleDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ReportingUnrevivedOutage? req = new();
            req.RTOutageId = DbUtils.SafeGetInt(reader, "ID");

            req.ElementId = DbUtils.SafeGetInt(reader, "ELEMENT_ID");
            req.ElementName = DbUtils.SafeGetString(reader, "ELEMENTNAME");

            req.ElementTypeId = DbUtils.SafeGetInt(reader, "ENTITY_ID");
            req.ElementType = DbUtils.SafeGetString(reader, "ELEMENTTYPE");
            
            req.OutageTypeId = DbUtils.SafeGetInt(reader, "SHUT_DOWN_TYPE_ID");
            req.OutageType = DbUtils.SafeGetString(reader, "SHUT_DOWN_TYPE_NAME");

            DateTime? outageDate = DbUtils.SafeGetDt(reader, "OUTAGE_DATE");
            string outageTimeStr = DbUtils.SafeGetString(reader, "OUTAGE_TIME");
            bool isOutageTimeStrValid = (outageDate != null) && (!string.IsNullOrWhiteSpace(outageTimeStr)) && (outageTimeStr.Length >= 5);
            if (!isOutageTimeStrValid)
            {
                continue;
            }
            DateTime outageDt = DateTime.ParseExact($"{outageDate?.ToString("yyyy-MM-dd")} {outageTimeStr[..5]}", "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
            req.OutageDateTime = outageDt;
            
            req.Reason = DbUtils.SafeGetString(reader, "REASON");
            
            req.OutageTagId = DbUtils.SafeGetInt(reader, "SHUTDOWN_TAG_ID");
            req.OutageTag = DbUtils.SafeGetString(reader, "SHUTDOWN_TAG");
            
            req.OutageRemarks = DbUtils.SafeGetString(reader, "OUTAGE_REMARKS");
            unrevOutages.Add(req);
        }
        reader.Dispose();

        return unrevOutages;
    }
}
