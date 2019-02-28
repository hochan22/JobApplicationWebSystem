namespace ApplicationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEmailAndPasswordToCandidate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Candidates", "Email", c => c.String());
            AddColumn("dbo.Candidates", "Password", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Candidates", "Password");
            DropColumn("dbo.Candidates", "Email");
        }
    }
}
