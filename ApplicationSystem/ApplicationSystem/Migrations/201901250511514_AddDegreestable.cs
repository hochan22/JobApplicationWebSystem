namespace ApplicationSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDegreestable : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Degrees (Id, Name) VALUES (1,'High-School')");
            Sql("INSERT INTO Degrees (Id, Name) VALUES (2, 'Bachelor')");
            Sql("INSERT INTO Degrees (Id, Name) VALUES (3, 'Master')");
            Sql("INSERT INTO Degrees (Id, Name) VALUES (4, 'Doctor')");
        }
        
        public override void Down()
        {
        }
    }
}
