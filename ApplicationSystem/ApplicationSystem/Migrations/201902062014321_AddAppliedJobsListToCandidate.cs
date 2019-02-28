namespace ApplicationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAppliedJobsListToCandidate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidates", "AppliedJobsList", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidates", "AppliedJobsList");
        }
    }
}
