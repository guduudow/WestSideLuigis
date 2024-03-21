namespace WestSideLuigis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Attendee : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Attendees",
                c => new
                    {
                        AttendeeID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Email = c.String(),
                        PhoneNumber = c.String(),
                    })
                .PrimaryKey(t => t.AttendeeID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Attendees");
        }
    }
}
