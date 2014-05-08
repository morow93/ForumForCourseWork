namespace BeginApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AvatarMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("UserProfile", "ImageData", c => c.Binary(nullable: true, isMaxLength: true, fixedLength: false));
            AddColumn("UserProfile", "ImageMimeType", c => c.String(nullable: true, maxLength: 50, fixedLength: false)); 
        }
        
        public override void Down()
        {
        }
    }
}
