namespace WestSideLuigis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AttendeeDTO : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Receptions", "Attendee_AttendeeID", c => c.Int());
            CreateIndex("dbo.Receptions", "Attendee_AttendeeID");
            AddForeignKey("dbo.Receptions", "Attendee_AttendeeID", "dbo.Attendees", "AttendeeID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Receptions", "Attendee_AttendeeID", "dbo.Attendees");
            DropIndex("dbo.Receptions", new[] { "Attendee_AttendeeID" });
            DropColumn("dbo.Receptions", "Attendee_AttendeeID");
        }
    }
}
