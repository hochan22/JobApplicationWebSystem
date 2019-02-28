namespace ApplicationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeMessageSenderType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "SenderId", c => c.Int(nullable: false));
            AddColumn("dbo.Messages", "ReceiverId", c => c.Int(nullable: false));
            AlterColumn("dbo.Messages", "Sender", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Messages", "Sender", c => c.Int(nullable: false));
            DropColumn("dbo.Messages", "ReceiverId");
            DropColumn("dbo.Messages", "SenderId");
        }
    }
}
