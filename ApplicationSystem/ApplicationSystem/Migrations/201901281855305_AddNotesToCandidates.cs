namespace ApplicationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNotesToCandidates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidates", "Notes", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidates", "Notes");
        }
    }
}
