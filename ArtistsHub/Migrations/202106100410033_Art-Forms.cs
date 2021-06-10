namespace ArtistsHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArtForms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ArtForms",
                c => new
                    {
                        ArtFormID = c.Int(nullable: false, identity: true),
                        ArtFormName = c.String(),
                        DisciplineID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ArtFormID)
                .ForeignKey("dbo.Disciplines", t => t.DisciplineID, cascadeDelete: true)
                .Index(t => t.DisciplineID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArtForms", "DisciplineID", "dbo.Disciplines");
            DropIndex("dbo.ArtForms", new[] { "DisciplineID" });
            DropTable("dbo.ArtForms");
        }
    }
}
