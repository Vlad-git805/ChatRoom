namespace _02_ChatClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class secnd_migrtion : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Port", c => c.String());
            DropColumn("dbo.Users", "EndPoint");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "EndPoint", c => c.String());
            DropColumn("dbo.Users", "Port");
        }
    }
}
