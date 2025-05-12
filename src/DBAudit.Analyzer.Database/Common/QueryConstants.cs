namespace DBAudit.Analyzer.Database.Common;

public static class QueryConstants
{
    public const string TablesWithoutPk = """
                                          SELECT DB_NAME() , t.name 
                                          FROM sys.tables t
                                          WHERE NOT EXISTS (SELECT 1
                                                            FROM sys.indexes i
                                                            WHERE i.object_id = t.object_id
                                                              AND i.is_primary_key = 1);
                                          """;
}