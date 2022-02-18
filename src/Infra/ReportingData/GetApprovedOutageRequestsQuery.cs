using Core.ReportingData;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData;

internal static class GetApprovedOutageRequestsQuery
{
    public static List<ReportingOutageRequest> Execute(string _reportingConnStr, DateTime? inpDate, int? apprOutageReqId = null, bool isApproved = true)
    {
        List<ReportingOutageRequest> outageRequests = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();

        // derive the where clause
        List<string> whereClauses = new();
        if (inpDate.HasValue)
        {
            whereClauses.Add("(TRUNC(:inpDate) BETWEEN TRUNC(sd.APPROVED_START_DATE) AND TRUNC(sd.APPROVED_END_DATE))");
            cmd.Parameters.Add(new OracleParameter("inpDate", inpDate ?? null));
        }
        if (apprOutageReqId.HasValue)
        {
            whereClauses.Add("SD.SHUTDOWN_REQUEST_ID=:reqId AND ss.STATUS = 'Approved'");
            cmd.Parameters.Add(new OracleParameter("reqId", apprOutageReqId.Value));
        }
        if (isApproved)
        {
            whereClauses.Add("ss.STATUS = 'Approved'");
        }

        // deny the query if there are no where clauses
        if (whereClauses.Count == 0)
        {
            return outageRequests;
        }

        string whereClause = string.Join(" AND ", whereClauses);

        con.Open();
        cmd.CommandText = @$"SELECT
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
            {whereClause}";

        OracleDataReader reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            ReportingOutageRequest? req = new();
            req.ShutdownId = DbUtils.SafeGetInt(reader, "ID");
            req.ShutdownRequestId = DbUtils.SafeGetInt(reader, "SHUTDOWN_REQUEST_ID");
            req.ElementTypeId = DbUtils.SafeGetInt(reader, "ENTITY_ID");
            req.ElementId = DbUtils.SafeGetInt(reader, "ELEMENT_ID");
            req.ElementName = DbUtils.SafeGetString(reader, "ELEMENT_NAME");
            req.ElementType = DbUtils.SafeGetString(reader, "ELEMENTTYPE");
            req.ReasonId = DbUtils.SafeGetInt(reader, "REASON_ID");
            req.Reason = DbUtils.SafeGetString(reader, "REASON");
            req.OutageType = DbUtils.SafeGetString(reader, "SHUTDOWNTYPE");
            req.OutageTypeId = DbUtils.SafeGetInt(reader, "SHUT_DOWN_TYPE_ID");
            req.OutageTag = DbUtils.SafeGetString(reader, "SHUTDOWN_TAG");
            req.OutageTagId = DbUtils.SafeGetInt(reader, "SHUTDOWN_TAG_ID");
            req.OccName = DbUtils.SafeGetString(reader, "OCC_NAME");
            req.Requester = DbUtils.SafeGetString(reader, "REQUESTER_NAME");
            req.RequesterId = DbUtils.SafeGetInt(reader, "REQUESTER_ID");
            req.OutageBasis = DbUtils.SafeGetString(reader, "DAILYCONT");
            req.ApprovedStartTime = DbUtils.SafeGetDt(reader, "APPROVED_START_DATE");
            req.ApprovedEndTime = DbUtils.SafeGetDt(reader, "APPROVED_END_DATE");
            req.RequesterRemarks = DbUtils.SafeGetString(reader, "REQUESTER_REMARKS");
            req.AvailingStatus = DbUtils.SafeGetString(reader, "AVAILINGSTATUS");
            req.ApprovalStatus = DbUtils.SafeGetString(reader, "STATUS");
            req.NldcApprovalStatus = DbUtils.SafeGetString(reader, "NLDC_APPROVAL_STATUS");
            req.RldcRemarks = DbUtils.SafeGetString(reader, "RLDC_REMARKS");
            req.RpcRemarks = DbUtils.SafeGetString(reader, "RPC_REMARKS");
            req.NldcRemarks = DbUtils.SafeGetString(reader, "NLDC_REMARKS");
            outageRequests.Add(req);
        }
        reader.Dispose();

        return outageRequests;
    }
}
