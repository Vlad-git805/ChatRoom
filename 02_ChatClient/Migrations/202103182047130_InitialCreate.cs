namespace _02_ChatClient.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Contacts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        Tag = c.String(nullable: false),
                        EndPoint = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserContacts",
                c => new
                    {
                        User_Id = c.Int(nullable: false),
                        Contact_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.User_Id, t.Contact_Id })
                .ForeignKey("dbo.Users", t => t.User_Id, cascadeDelete: true)
                .ForeignKey("dbo.Contacts", t => t.Contact_Id, cascadeDelete: true)
                .Index(t => t.User_Id)
                .Index(t => t.Contact_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserContacts", "Contact_Id", "dbo.Contacts");
            DropForeignKey("dbo.UserContacts", "User_Id", "dbo.Users");
            DropIndex("dbo.UserContacts", new[] { "Contact_Id" });
            DropIndex("dbo.UserContacts", new[] { "User_Id" });
            DropTable("dbo.UserContacts");
            DropTable("dbo.Users");
            DropTable("dbo.Contacts");
        }
    }
}
