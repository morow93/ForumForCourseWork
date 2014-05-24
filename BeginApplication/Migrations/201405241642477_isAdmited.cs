namespace BeginApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class isAdmited : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.File", "CommentId", "dbo.Comment");
            //DropForeignKey("dbo.Message", "UserIdFrom", "dbo.UserProfile");
            //DropForeignKey("dbo.Message", "UserIdTo", "dbo.UserProfile");
            //DropForeignKey("dbo.Message", "UserProfile_UserId", "dbo.UserProfile");
            //DropForeignKey("dbo.Message", "UserProfile_UserId1", "dbo.UserProfile");
            //DropIndex("dbo.File", new[] { "CommentId" });
            //DropIndex("dbo.Message", new[] { "UserIdFrom" });
            //DropIndex("dbo.Message", new[] { "UserIdTo" });
            //DropIndex("dbo.Message", new[] { "UserProfile_UserId" });
            //DropIndex("dbo.Message", new[] { "UserProfile_UserId1" });
            //DropTable("dbo.File");
            //DropTable("dbo.Message");

            AddColumn("Comment", "IsAdmitted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        IdMessage = c.Int(nullable: false, identity: true),
                        UserIdFrom = c.Int(nullable: false),
                        UserIdTo = c.Int(nullable: false),
                        MessageTitle = c.String(),
                        MessageText = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        UserProfile_UserId = c.Int(),
                        UserProfile_UserId1 = c.Int(),
                    })
                .PrimaryKey(t => t.IdMessage);
            
            CreateTable(
                "dbo.File",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        CommentId = c.Int(nullable: false),
                        FileContent = c.Binary(),
                    })
                .PrimaryKey(t => t.FileId);
            
            CreateIndex("dbo.Message", "UserProfile_UserId1");
            CreateIndex("dbo.Message", "UserProfile_UserId");
            CreateIndex("dbo.Message", "UserIdTo");
            CreateIndex("dbo.Message", "UserIdFrom");
            CreateIndex("dbo.File", "CommentId");
            AddForeignKey("dbo.Message", "UserProfile_UserId1", "dbo.UserProfile", "UserId");
            AddForeignKey("dbo.Message", "UserProfile_UserId", "dbo.UserProfile", "UserId");
            AddForeignKey("dbo.Message", "UserIdTo", "dbo.UserProfile", "UserId");
            AddForeignKey("dbo.Message", "UserIdFrom", "dbo.UserProfile", "UserId");
            AddForeignKey("dbo.File", "CommentId", "dbo.Comment", "CommentId");
        }
    }
}
