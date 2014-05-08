namespace BeginApplication.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteTheme : DbMigration
    {
        public override void Up()
        {
            //modelBuilder.Entity<User>()
            //.HasOptional(a => a.UserDetail)
            //.WithOptionalDependent()
            //.WillCascadeOnDelete(true);    
        }

        public override void Down()
        {
            
        }
    }
}
