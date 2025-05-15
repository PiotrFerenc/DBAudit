namespace DBAudit.Analyzer.Table.Common;

public static class QueryConstants
{
    public const string TablesWithoutPk = """
                                          SELECT Count(1)
                                          FROM sys.tables t
                                          WHERE NOT EXISTS (SELECT 1
                                                            FROM sys.indexes i
                                                            WHERE i.object_id = t.object_id
                                                              AND i.is_primary_key = 1) and DB_NAME() = '@table';
                                          """;
}