namespace WestSideLuigis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BeerDTO : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Beers", "BreweryID", c => c.Int(nullable: false));
            CreateIndex("dbo.Beers", "BreweryID");
            AddForeignKey("dbo.Beers", "BreweryID", "dbo.Breweries", "BreweryID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Beers", "BreweryID", "dbo.Breweries");
            DropIndex("dbo.Beers", new[] { "BreweryID" });
            DropColumn("dbo.Beers", "BreweryID");
        }
    }
}
