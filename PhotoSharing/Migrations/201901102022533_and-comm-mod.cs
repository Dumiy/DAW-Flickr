namespace PhotoSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class andcommmod : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Comments", "Timestamp", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Comments", "Timestamp");
        }
    }
}
