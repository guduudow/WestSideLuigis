namespace WestSideLuigis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Brewery : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Breweries",
                c => new
                    {
                        BreweryID = c.Int(nullable: false, identity: true),
                        BreweryName = c.String(),
                        BreweryLocation = c.String(),
                        BreweryHasPic = c.Boolean(nullable: false),
                        PicExtension = c.String(),
                    })
                .PrimaryKey(t => t.BreweryID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Breweries");
        }
    }
}
