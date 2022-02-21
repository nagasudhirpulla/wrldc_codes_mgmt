using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ReportingData;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData.SingleElementOwnerQueries;

internal class GetCompensatorOwnersQuery
{
    public static List<ReportingOwner> Execute(string _reportingConnStr, int id)
    {
        List<ReportingOwner> owners = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();
        con.Open();
        cmd.CommandText = @"SELECT own.owner_name,
                                own.id
                            FROM REPORTING_WEB_UI_UAT.entity_entity_reln ent_reln
                                LEFT JOIN REPORTING_WEB_UI_UAT.owner own ON own.id = ent_reln.child_entity_attribute_id
                            WHERE ent_reln.child_entity = 'OWNER'
                                AND ent_reln.parent_entity IN (
                                    'STATCOM',
                                    'TCSC',
                                    'MSR',
                                    'MSC'
                                )
                                AND ent_reln.child_entity_attribute = 'OwnerId'
                                AND ent_reln.parent_entity_attribute = 'Owner'
                                AND ent_reln.parent_entity_attribute_id = :compId";
        cmd.Parameters.Add(new OracleParameter("compId", id));
        OracleDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            string uName = reader.GetString(0);
            int uId = reader.GetInt32(1);

            owners.Add(new ReportingOwner(uId, uName));
        }
        reader.Dispose();

        return owners;
    }
}
