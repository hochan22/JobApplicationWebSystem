namespace ApplicationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddJobAttributes : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "Location", c => c.String());
            AddColumn("dbo.Jobs", "DurationInMonths", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "DurationInMonths");
            DropColumn("dbo.Jobs", "Location");
        }
    }
}
