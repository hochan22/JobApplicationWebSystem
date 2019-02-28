namespace ApplicationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddResumeProcedure : DbMigration
    {
        public override void Up()
        {
            Sql("Create Procedure [dbo].[AddFileDetails]  (@FileName varchar(60),@FileContent varBinary(Max))as begin Set NoCount on Insert into FileDetails values(@FileName, @FileContent) End  ");
        }
        
        public override void Down()
        {
        }
    }
}
