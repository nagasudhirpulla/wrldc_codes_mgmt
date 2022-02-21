using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.ReportingData;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData.SingleElementOwnerQueries;

internal class GetGeneratingUnitOwnersQuery
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
                                LEFT JOIN REPORTING_WEB_UI_UAT.generating_unit gen_unit on ent_reln.parent_entity_attribute_id = gen_unit.fk_generating_station
                            WHERE ent_reln.child_entity = 'Owner'
                                AND ent_reln.parent_entity = 'GENERATING_STATION'
                                AND ent_reln.child_entity_attribute = 'OwnerId'
                                AND ent_reln.parent_entity_attribute = 'Owner'
                                AND gen_unit.id = :genId";
        cmd.Parameters.Add(new OracleParameter("genId", id));
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
