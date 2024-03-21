namespace WestSideLuigis.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MenuItemDTO : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuItems", "CategoryId", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "MenuItem_ItemID", c => c.Int());
            CreateIndex("dbo.MenuItems", "CategoryId");
            CreateIndex("dbo.Orders", "MenuItem_ItemID");
            AddForeignKey("dbo.MenuItems", "CategoryId", "dbo.Categories", "CategoryId", cascadeDelete: true);
            AddForeignKey("dbo.Orders", "MenuItem_ItemID", "dbo.MenuItems", "ItemID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "MenuItem_ItemID", "dbo.MenuItems");
            DropForeignKey("dbo.MenuItems", "CategoryId", "dbo.Categories");
            DropIndex("dbo.Orders", new[] { "MenuItem_ItemID" });
            DropIndex("dbo.MenuItems", new[] { "CategoryId" });
            DropColumn("dbo.Orders", "MenuItem_ItemID");
            DropColumn("dbo.MenuItems", "CategoryId");
        }
    }
}
