namespace PhotoSharing.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoriesAndPhotos : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Photos",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Category_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categories", t => t.Category_Id)
                .Index(t => t.Category_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Photos", "Category_Id", "dbo.Categories");
            DropIndex("dbo.Photos", new[] { "Category_Id" });
            DropTable("dbo.Photos");
            DropTable("dbo.Categories");
        }
    }
}
