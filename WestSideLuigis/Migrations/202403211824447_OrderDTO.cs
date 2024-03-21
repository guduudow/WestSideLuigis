namespace WestSideLuigis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OrderDTO : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Orders", "MenuItem_ItemID", "dbo.MenuItems");
            DropIndex("dbo.Orders", new[] { "MenuItem_ItemID" });
            CreateTable(
                "dbo.OrderMenuItems",
                c => new
                    {
                        Order_OrderId = c.Int(nullable: false),
                        MenuItem_ItemID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Order_OrderId, t.MenuItem_ItemID })
                .ForeignKey("dbo.Orders", t => t.Order_OrderId, cascadeDelete: true)
                .ForeignKey("dbo.MenuItems", t => t.MenuItem_ItemID, cascadeDelete: true)
                .Index(t => t.Order_OrderId)
                .Index(t => t.MenuItem_ItemID);
            
            DropColumn("dbo.Orders", "MenuItem_ItemID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "MenuItem_ItemID", c => c.Int());
            DropForeignKey("dbo.OrderMenuItems", "MenuItem_ItemID", "dbo.MenuItems");
            DropForeignKey("dbo.OrderMenuItems", "Order_OrderId", "dbo.Orders");
            DropIndex("dbo.OrderMenuItems", new[] { "MenuItem_ItemID" });
            DropIndex("dbo.OrderMenuItems", new[] { "Order_OrderId" });
            DropTable("dbo.OrderMenuItems");
            CreateIndex("dbo.Orders", "MenuItem_ItemID");
            AddForeignKey("dbo.Orders", "MenuItem_ItemID", "dbo.MenuItems", "ItemID");
        }
    }
}
