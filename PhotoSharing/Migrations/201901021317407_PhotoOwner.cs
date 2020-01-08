namespace PhotoSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhotoOwner : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Photos", "Owner_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Photos", "Owner_Id");
            AddForeignKey("dbo.Photos", "Owner_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photos", "Owner_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Photos", new[] { "Owner_Id" });
            DropColumn("dbo.Photos", "Owner_Id");
        }
    }
}
