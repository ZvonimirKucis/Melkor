namespace Melkor_core_dbhandler.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mig1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NotificationContexts",
                c => new
                    {
                        NotificationId = c.Guid(nullable: false),
                        Title = c.String(),
                        Message = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        PublishedBy = c.String(),
                    })
                .PrimaryKey(t => t.NotificationId);
            
            CreateTable(
                "dbo.TestContexts",
                c => new
                    {
                        TestId = c.Guid(nullable: false),
                        Name = c.String(),
                        Result = c.Boolean(nullable: false),
                        RunDateTime = c.DateTime(nullable: false),
                        UserId = c.Guid(nullable: false),
                        Dir = c.String(),
                    })
                .PrimaryKey(t => t.TestId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TestContexts");
            DropTable("dbo.NotificationContexts");
        }
    }
}
