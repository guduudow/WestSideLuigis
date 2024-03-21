namespace WestSideLuigis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reception : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Receptions",
                c => new
                    {
                        ReceptionID = c.Int(nullable: false, identity: true),
                        ReceptionName = c.String(),
                        ReceptionLocation = c.String(),
                        ReceptionDate = c.DateTime(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        ReceptionPrice = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ReceptionDescription = c.String(),
                    })
                .PrimaryKey(t => t.ReceptionID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Receptions");
        }
    }
}
