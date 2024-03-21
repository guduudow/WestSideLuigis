namespace WestSideLuigis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Beer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Beers",
                c => new
                    {
                        BeerID = c.Int(nullable: false, identity: true),
                        BeerName = c.String(),
                        BeerType = c.String(),
                        BeerDescription = c.String(),
                        BeerAlcoholContent = c.String(),
                        BeerHasPic = c.Boolean(nullable: false),
                        PicExtension = c.String(),
                        BreweryName = c.String(),
                    })
                .PrimaryKey(t => t.BeerID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Beers");
        }
    }
}
