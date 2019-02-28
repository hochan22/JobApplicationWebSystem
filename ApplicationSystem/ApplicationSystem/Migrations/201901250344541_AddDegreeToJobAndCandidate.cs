namespace ApplicationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDegreeToJobAndCandidate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Degrees",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Candidates", "DegreeId", c => c.Byte(nullable: false));
            AddColumn("dbo.Jobs", "DegreeId", c => c.Byte(nullable: false));
            CreateIndex("dbo.Candidates", "DegreeId");
            CreateIndex("dbo.Jobs", "DegreeId");
            AddForeignKey("dbo.Candidates", "DegreeId", "dbo.Degrees", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Jobs", "DegreeId", "dbo.Degrees", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Jobs", "DegreeId", "dbo.Degrees");
            DropForeignKey("dbo.Candidates", "DegreeId", "dbo.Degrees");
            DropIndex("dbo.Jobs", new[] { "DegreeId" });
            DropIndex("dbo.Candidates", new[] { "DegreeId" });
            DropColumn("dbo.Jobs", "DegreeId");
            DropColumn("dbo.Candidates", "DegreeId");
            DropTable("dbo.Degrees");
        }
    }
}
