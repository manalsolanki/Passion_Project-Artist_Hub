namespace ArtistsHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Discipline : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Disciplines",
                c => new
                    {
                        DisciplineID = c.Int(nullable: false, identity: true),
                        DisciplineName = c.String(),
                        DisciplineDescription = c.String(),
                    })
                .PrimaryKey(t => t.DisciplineID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Disciplines");
        }
    }
}
