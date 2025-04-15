using FluentMigrator;

namespace DBAudit.Infrastructure.Data.Migrations;

[Migration(202504151419)]
public class DatabaseMigration : Migration
{
    public override void Up()
    {
        Create.Table("Database")
            .WithColumn("Id").AsGuid().PrimaryKey()
            .WithColumn("Name").AsString().NotNullable()
            .WithColumn("EnvironmentId").AsGuid().NotNullable()
            .ForeignKey("FK_Database_Environment", "Environments", "Id");
    }

    public override void Down()
    {
        Delete.Table("Database");
    }
}