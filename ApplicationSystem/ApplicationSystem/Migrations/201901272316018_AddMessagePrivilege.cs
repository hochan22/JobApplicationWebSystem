namespace ApplicationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessagePrivilege : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "SenderPrivilege", c => c.Int(nullable: false));
            AddColumn("dbo.Messages", "ReceiverPrivilege", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "ReceiverPrivilege");
            DropColumn("dbo.Messages", "SenderPrivilege");
        }
    }
}
