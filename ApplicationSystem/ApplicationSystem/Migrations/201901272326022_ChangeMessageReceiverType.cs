namespace ApplicationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeMessageReceiverType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Messages", "Receiver", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Messages", "Receiver", c => c.Int(nullable: false));
        }
    }
}
