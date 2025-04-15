using FluentMigrator;

namespace DBAudit.Infrastructure.Data.Migrations;

[Migration(202504151421)]
public class TableMigration : Migration
{
    public override void Up()
    {
        Create.Table("Table")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("DatabaseId").AsGuid();
    }

    public override void Down()
    {
        Delete.Table("Table");
    }
}