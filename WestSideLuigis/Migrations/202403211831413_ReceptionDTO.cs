namespace WestSideLuigis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReceptionDTO : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Receptions", "Attendee_AttendeeID", "dbo.Attendees");
            DropIndex("dbo.Receptions", new[] { "Attendee_AttendeeID" });
            CreateTable(
                "dbo.ReceptionAttendees",
                c => new
                    {
                        Reception_ReceptionID = c.Int(nullable: false),
                        Attendee_AttendeeID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Reception_ReceptionID, t.Attendee_AttendeeID })
                .ForeignKey("dbo.Receptions", t => t.Reception_ReceptionID, cascadeDelete: true)
                .ForeignKey("dbo.Attendees", t => t.Attendee_AttendeeID, cascadeDelete: true)
                .Index(t => t.Reception_ReceptionID)
                .Index(t => t.Attendee_AttendeeID);
            
            DropColumn("dbo.Receptions", "Attendee_AttendeeID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Receptions", "Attendee_AttendeeID", c => c.Int());
            DropForeignKey("dbo.ReceptionAttendees", "Attendee_AttendeeID", "dbo.Attendees");
            DropForeignKey("dbo.ReceptionAttendees", "Reception_ReceptionID", "dbo.Receptions");
            DropIndex("dbo.ReceptionAttendees", new[] { "Attendee_AttendeeID" });
            DropIndex("dbo.ReceptionAttendees", new[] { "Reception_ReceptionID" });
            DropTable("dbo.ReceptionAttendees");
            CreateIndex("dbo.Receptions", "Attendee_AttendeeID");
            AddForeignKey("dbo.Receptions", "Attendee_AttendeeID", "dbo.Attendees", "AttendeeID");
        }
    }
}
