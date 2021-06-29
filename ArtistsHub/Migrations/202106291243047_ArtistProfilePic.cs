namespace ArtistsHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ArtistProfilePic : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Artists", "ArtistHasProfilePic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Artists", "ProfilePicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Artists", "ProfilePicExtension");
            DropColumn("dbo.Artists", "ArtistHasProfilePic");
        }
    }
}
