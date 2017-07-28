using FluentMigrator;

namespace BIZ.PControl.DbSchema.Migration2016053001
{
    [Migration(2016053001)]
    public class CreateTablesAndProcedures: Migration
    {
        public override void Up()
        {
            Create.Table("Products")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Name").AsString(256);

            Create.Table("Documents")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Description").AsString(256)
            .WithColumn("Date").AsDateTime().NotNullable();

            Create.Table("DocumentsDetails")
            .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("Document_Id").AsInt32().NotNullable().ForeignKey("DocumentsDetails_Documents", "Documents", "Id")
            .WithColumn("Product_Id").AsInt32().NotNullable().ForeignKey("DocumentsDetails_Products", "Products", "Id")
            .WithColumn("Quantity").AsDouble().NotNullable();

            Create.Table("Balances")
            .WithColumn("Date").AsDateTime().NotNullable()
            .WithColumn("Product_Id").AsInt32().NotNullable().ForeignKey("Balances_product", "Products", "Id")
            .WithColumn("Quantity").AsDouble().NotNullable();

            Create.PrimaryKey("PK").OnTable("Balances").Columns("Date", "Product_Id");

            Execute.EmbeddedScript("UpdateBalances_2016050301.sql");
        }

        public override void Down()
        {
            Delete.Table("Balances");
            Delete.Table("DocumentsDetails");
            Delete.Table("Documents");
            Delete.Table("Products");

            Execute.Sql("DROP PROCEDURE [UpdateBalances]");
        }
    }
}