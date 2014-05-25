namespace BeginApplication.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinallyMessageMigration : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "Message",
            //    c => new
            //    {
            //        MessageId = c.Int(nullable: false, identity: true),
            //        UserIdFrom = c.Int(nullable: false),
            //        UserIdTo = c.Int(nullable: false),
            //        MessageTitle = c.String(nullable: false),
            //        MessageText = c.String(nullable: false),
            //        CreationDate = c.DateTime(nullable: false)
            //    })
            //    .PrimaryKey(t => t.MessageId)
            //    .ForeignKey("UserProfile", t => t.UserIdTo, cascadeDelete: false)
            //    .Index(t => t.UserIdTo)
            //    .ForeignKey("UserProfile", t => t.UserIdFrom, cascadeDelete: false)
            //    .Index(t => t.UserIdFrom);        
        }
        
        public override void Down()
        {

        }
    }
}
