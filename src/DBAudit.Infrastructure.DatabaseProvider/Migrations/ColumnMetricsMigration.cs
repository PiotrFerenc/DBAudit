using FluentMigrator;

namespace DBAudit.Infrastructure.DatabaseProvider.Migrations;

[Migration(202505261022)]
public class ColumnMetricsMigration : Migration
{
    public override void Up()
    {
        Create.Table("ColumnMetrics")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Title").AsString().NotNullable()
            .WithColumn("Value").AsInt32().NotNullable()
            .WithColumn("EnvironmentId").AsGuid().NotNullable()
            .WithColumn("DatabaseId").AsGuid().NotNullable()
            .WithColumn("TableId").AsGuid().NotNullable()
            .WithColumn("ColumnId").AsGuid().NotNullable()
            .WithColumn("Type").AsString().NotNullable()
            .WithColumn("CreatedAt").AsDateTime().NotNullable();
    }
    
    public override void Down()
    {
        Delete.Table("ColumnMetrics");
    }
}