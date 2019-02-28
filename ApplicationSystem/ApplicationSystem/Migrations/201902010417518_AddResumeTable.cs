namespace ApplicationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResumeTable : DbMigration
    {
        public override void Up()
        {
            Sql("CREATE TABLE [dbo].[FileDetails]( [Id][int] IDENTITY(1, 1) NOT NULL,[FileName][varchar](60) NULL,[FileContent][varbinary](max) NULL) ON[PRIMARY] TEXTIMAGE_ON[PRIMARY] ");
        }
        
        public override void Down()
        {
        }
    }
}
