using Core.ReportingData;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData;

internal static class GetRequesterApprovedOutageRequestsForDateQuery
{
    public static List<ReportingOutageRequest> Execute(string _reportingConnStr, DateTime inpDate)
    {
        List<ReportingOutageRequest> outageRequests = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();
        con.Open();
        cmd.CommandText = @"SELECT
            SD.ID,
            SD.SHUTDOWN_REQUEST_ID,
            sr.ENTITY_ID,
            sr.ELEMENT_ID,
            sr.ELEMENT_NAME,
            sr.elementType,
            sr.REASON_ID,
            sr.REASON,
            sr.shutdownType,
            sr.SHUT_DOWN_TYPE_ID,
            sr.SHUTDOWN_TAG,
            sr.SHUTDOWN_TAG_ID,
            sr.occ_name,
            sr.requester_name,
            sr.requester_id,
            CASE
                WHEN SR.IS_CONTINUOUS = 1 THEN 'Continuous'
                WHEN sr.IS_CONTINUOUS = 0 THEN 'Daily'
                ELSE NULL
            END DailyCont,
            SD.APPROVED_START_DATE,
            SD.APPROVED_END_DATE,
            sr.REQUESTER_REMARKS,
            CASE
                WHEN sr.is_availed = 1 THEN 'Yes'
                WHEN sr.is_availed = 2 THEN 'NO'
                ELSE NULL
            END AvailingStatus,
            ss.STATUS,
            sr.nldc_approval_status,
            SD.RLDC_REMARKS,
            sr.rpc_remarks,
            sr.NLDC_REMARKS
        FROM
            REPORTING_WEB_UI_UAT.SHUTDOWN sd
        LEFT JOIN REPORTING_WEB_UI_UAT.SHUTDOWN_STATUS ss ON
            ss.ID = sd.STATUS_ID
        LEFT JOIN (
            SELECT
                req.*, sot.NAME AS shutdownType, em.ENTITY_NAME AS elementType, or2.REASON, om.OCC_NAME, ud.USER_NAME AS requester_name, ud.USERID AS requester_id, ss2.STATUS AS nldc_approval_status, sdTag.NAME AS SHUTDOWN_TAG
            FROM
                REPORTING_WEB_UI_UAT.SHUTDOWN_REQUEST req
            LEFT JOIN REPORTING_WEB_UI_UAT.SHUTDOWN_OUTAGE_TYPE sot ON
                sot.ID = req.SHUT_DOWN_TYPE_ID
            LEFT JOIN REPORTING_WEB_UI_UAT.ENTITY_MASTER em ON
                em.ID = req.ENTITY_ID
            LEFT JOIN REPORTING_WEB_UI_UAT.OUTAGE_REASON or2 ON
                or2.ID = req.REASON_ID
            LEFT JOIN REPORTING_WEB_UI_UAT.OCC_MASTER om ON
                om.OCC_ID = req.OCC_ID
            LEFT JOIN REPORTING_WEB_UI_UAT.USER_DETAILS ud ON
                req.INTENDED_BY = ud.USERID
            LEFT JOIN REPORTING_WEB_UI_UAT.SHUTDOWN_OUTAGE_TAG sdTag ON
                sdTag.ID = req.SHUTDOWN_TAG_ID 
            LEFT JOIN REPORTING_WEB_UI_UAT.SHUTDOWN_STATUS ss2 ON
                req.NLDC_STATUS_ID = ss2.ID ) sr ON
            sr.ID = sd.SHUTDOWN_REQUEST_ID
        WHERE
            (TRUNC(:inpDate) BETWEEN TRUNC(sd.APPROVED_START_DATE) AND TRUNC(sd.APPROVED_END_DATE))
            AND ss.STATUS = 'Approved'";

        cmd.Parameters.Add(new OracleParameter("inpDate", inpDate));

        OracleDataReader reader = cmd.ExecuteReader();

        // TODO move to utils or create extension method
        static string SafeGetString(OracleDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            return string.Empty;
        }

        static int SafeGetInt(OracleDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetInt32(colIndex);
            return -1;
        }

        static DateTime? SafeGetDt(OracleDataReader reader, int colIndex)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetDateTime(colIndex);
            return null;
        }

        while (reader.Read())
        {
            //TODO get values by column name instead of position
            // https://stackoverflow.com/questions/28325813/sqldatareader-get-value-by-column-name-not-ordinal-number/42182943
            ReportingOutageRequest? req = new();
            req.ShutdownId = SafeGetInt(reader, 0);
            req.ShutdownRequestId = SafeGetInt(reader, 1);
            req.ElementTypeId = SafeGetInt(reader, 2);
            req.ElementId = SafeGetInt(reader, 3);
            req.ElementName = SafeGetString(reader, 4);
            req.ElementType = SafeGetString(reader, 5);
            req.ReasonId = SafeGetInt(reader, 6);
            req.Reason = SafeGetString(reader, 7);
            req.OutageType = SafeGetString(reader, 8);
            req.OutageTypeId = SafeGetInt(reader, 9);
            req.OutageTag = SafeGetString(reader, 10);
            req.OutageTagId = SafeGetInt(reader, 11);
            req.OccName = SafeGetString(reader, 12);
            req.Requester = SafeGetString(reader, 13);
            req.RequesterId = SafeGetInt(reader, 14);
            req.OutageBasis = SafeGetString(reader, 15);
            req.ApprovedStartTime = SafeGetDt(reader, 16);
            req.ApprovedEndTime = SafeGetDt(reader, 17);
            req.RequesterRemarks = SafeGetString(reader, 18);
            req.AvailingStatus = SafeGetString(reader, 19);
            req.ApprovalStatus = SafeGetString(reader, 20);
            req.NldcApprovalStatus = SafeGetString(reader, 21);
            req.RldcRemarks = SafeGetString(reader, 22);
            req.RpcRemarks = SafeGetString(reader, 23);
            req.NldcRemarks = SafeGetString(reader, 24);
            outageRequests.Add(req);
        }
        reader.Dispose();

        return outageRequests;
    }
}
