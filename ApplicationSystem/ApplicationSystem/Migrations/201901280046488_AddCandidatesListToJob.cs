namespace ApplicationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCandidatesListToJob : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Jobs", "CandidatesList", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Jobs", "CandidatesList");
        }
    }
}
