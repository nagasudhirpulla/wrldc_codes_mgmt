using Core.ReportingData;
using Core.ReportingData.GetElementsForDisplay;
using Oracle.ManagedDataAccess.Client;

namespace Infra.ReportingData.ElementsDisplayQueries;

internal class GetAllTransformersQuery
{
    public static List<ReportingTransformer> Execute(string _reportingConnStr)
    {
        List<ReportingTransformer> allTransformers = new();

        using OracleConnection con = new(_reportingConnStr);

        using OracleCommand cmd = con.CreateCommand();
        con.Open();
        cmd.CommandText = @"SELECT
                            t.id,
                            t.transformer_name,
                            t.mva_capacity,
                            t.type_gt_ict,
                            owner_details.owners,
                            owner_details.owner_ids
                        FROM
                            reporting_web_ui_uat.transformer t
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
                                    AND ent_reln.parent_entity = 'TRANSFORMER'
                                    AND ent_reln.child_entity_attribute = 'OwnerId'
                                    AND ent_reln.parent_entity_attribute = 'Owner'
                                GROUP BY
                                    parent_entity_attribute_id
                            )                                owner_details ON owner_details.element_id = t.id";
        OracleDataReader reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            ReportingTransformer? transformer = new();
            transformer.TId = DbUtils.SafeGetInt(reader, "ID");
            transformer.TransformerName = DbUtils.SafeGetString(reader, "TRANSFORMER_NAME");
            transformer.MvaCapacity = DbUtils.SafeGetInt(reader, "MVA_CAPACITY");
            transformer.TypeGtIct = DbUtils.SafeGetInt(reader, "TYPE_GT_ICT");
            transformer.Owners = DbUtils.SafeGetString(reader, "OWNERS");
            transformer.OwnerIds = DbUtils.SafeGetString(reader, "OWNER_IDS");
            allTransformers.Add(transformer);
        }
        reader.Dispose();

        return allTransformers;
    }
}
