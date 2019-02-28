namespace ApplicationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCodeToHiringManager : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.HiringManagers", "Code", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.HiringManagers", "Code");
        }
    }
}
