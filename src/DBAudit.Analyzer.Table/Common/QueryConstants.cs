namespace DBAudit.Analyzer.Table.Common;

public static class QueryConstants
{
    public const string TablesWithoutPk = """
                                          SELECT Count(1)
                                          FROM sys.tables t
                                          WHERE NOT EXISTS (SELECT 1
                                                            FROM sys.indexes i
                                                            WHERE i.object_id = t.object_id
                                                              AND i.is_primary_key = 1) and name = '@table';
                                          """;

    public const string DetectTablesWithoutIndexes = """
        SELECT
            count(1)
        FROM
            sys.tables t
                JOIN sys.schemas s ON t.schema_id = s.schema_id
        WHERE
            NOT EXISTS (
                SELECT 1
                FROM sys.indexes i
                WHERE i.object_id = t.object_id
                  AND i.type > 0 
            )
        and t.name ='@table'
        """;

}