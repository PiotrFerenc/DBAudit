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
            .WithColumn("DatabaseId").AsGuid().ForeignKey("FK_Table_Database", "Database", "Id");
        
    }

    public override void Down()
    {
        Delete.Table("Table");
    }
}