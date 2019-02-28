namespace ApplicationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPrivilegeToCandidate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidates", "Privilege", c => c.Byte(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidates", "Privilege");
        }
    }
}
