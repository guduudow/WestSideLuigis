namespace WestSideLuigis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ListBeersForMenuItems : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuItems", "BeerID", c => c.Int(nullable: false));
            CreateIndex("dbo.MenuItems", "BeerID");
            AddForeignKey("dbo.MenuItems", "BeerID", "dbo.Beers", "BeerID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MenuItems", "BeerID", "dbo.Beers");
            DropIndex("dbo.MenuItems", new[] { "BeerID" });
            DropColumn("dbo.MenuItems", "BeerID");
        }
    }
}
