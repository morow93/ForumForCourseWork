namespace BeginApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropSomeClumns : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.UserProfile", "IsDeleted");
            DropColumn("dbo.UserProperty", "AllowSendMessage");
        }
        
        public override void Down()
        {
            
        }
    }
}
