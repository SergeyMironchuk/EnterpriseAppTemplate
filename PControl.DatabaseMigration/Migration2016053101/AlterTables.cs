using FluentMigrator;

namespace BIZ.PControl.DbSchema.Migration2016053101
{
    [Migration(2016053101)]
    public class AlterTables: Migration
    {
        public override void Up()
        {
            Delete.ForeignKey("DocumentsDetails_Documents").OnTable("DocumentsDetails");
            Delete.PrimaryKey("PK_Documents").FromTable("Documents");
            Delete.PrimaryKey("PK_DocumentsDetails").FromTable("DocumentsDetails");
            Alter.Column("Id").OnTable("Documents").AsInt64();
            Alter.Column("Document_Id").OnTable("DocumentsDetails").AsInt64();
            Alter.Column("Id").OnTable("DocumentsDetails").AsInt64();
            Create.PrimaryKey().OnTable("Documents").Column("Id");
            Create.PrimaryKey().OnTable("DocumentsDetails").Column("Id");
            Create.ForeignKey("DocumentsDetails_Documents").FromTable("DocumentsDetails").ForeignColumn("Document_Id").ToTable("Documents").PrimaryColumn("Id");
        }

        public override void Down()
        {
            Delete.ForeignKey("DocumentsDetails_Documents").OnTable("DocumentsDetails");
            Delete.PrimaryKey("PK_Documents").FromTable("Documents");
            Delete.PrimaryKey("PK_DocumentsDetails").FromTable("DocumentsDetails");
            Alter.Column("Id").OnTable("Documents").AsInt32();
            Alter.Column("Document_Id").OnTable("DocumentsDetails").AsInt32();
            Alter.Column("Id").OnTable("DocumentsDetails").AsInt32();
            Create.PrimaryKey().OnTable("Documents").Column("Id");
            Create.PrimaryKey().OnTable("DocumentsDetails").Column("Id");
            Create.ForeignKey("DocumentsDetails_Documents").FromTable("DocumentsDetails").ForeignColumn("Document_Id").ToTable("Documents").PrimaryColumn("Id");
        }
    }
}