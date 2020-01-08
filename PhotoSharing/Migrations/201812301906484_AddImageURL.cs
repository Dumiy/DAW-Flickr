namespace PhotoSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageURL : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "URL", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Photos", "URL");
        }
    }
}
