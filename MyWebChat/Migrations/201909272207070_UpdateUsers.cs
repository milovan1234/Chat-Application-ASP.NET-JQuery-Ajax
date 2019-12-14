namespace MyWebChat.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUsers : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Users", "UserRank", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Users", "UserRank", c => c.String(nullable: false));
        }
    }
}
