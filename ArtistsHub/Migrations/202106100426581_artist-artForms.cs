namespace ArtistsHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class artistartForms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Artists",
                c => new
                    {
                        ArtistID = c.Int(nullable: false, identity: true),
                        ArtistFirstName = c.String(),
                        ArtistLastName = c.String(),
                        ArtistOccupation = c.String(),
                        ArtistPhoneNumber = c.String(),
                        ArtistEmail = c.String(),
                        ArtistDescription = c.String(),
                    })
                .PrimaryKey(t => t.ArtistID);
            
            CreateTable(
                "dbo.ArtistArtForms",
                c => new
                    {
                        Artist_ArtistID = c.Int(nullable: false),
                        ArtForm_ArtFormID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Artist_ArtistID, t.ArtForm_ArtFormID })
                .ForeignKey("dbo.Artists", t => t.Artist_ArtistID, cascadeDelete: true)
                .ForeignKey("dbo.ArtForms", t => t.ArtForm_ArtFormID, cascadeDelete: true)
                .Index(t => t.Artist_ArtistID)
                .Index(t => t.ArtForm_ArtFormID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ArtistArtForms", "ArtForm_ArtFormID", "dbo.ArtForms");
            DropForeignKey("dbo.ArtistArtForms", "Artist_ArtistID", "dbo.Artists");
            DropIndex("dbo.ArtistArtForms", new[] { "ArtForm_ArtFormID" });
            DropIndex("dbo.ArtistArtForms", new[] { "Artist_ArtistID" });
            DropTable("dbo.ArtistArtForms");
            DropTable("dbo.Artists");
        }
    }
}
