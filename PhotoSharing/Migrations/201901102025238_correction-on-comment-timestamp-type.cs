namespace PhotoSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class correctiononcommenttimestamptype : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Comments", "Timestamp", c => c.DateTime(nullable: false, defaultValueSql: "GETDATE()"));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Comments", "Timestamp", c => c.String());
        }
    }
}
