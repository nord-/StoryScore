namespace StoryScore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class teamandplayers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        PlayerNumber = c.Int(nullable: false),
                        Position = c.String(),
                        PicturePath = c.String(),
                        PresentationVideoPath = c.String(),
                        GoalVideoPath = c.String(),
                        Team_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Teams", t => t.Team_Id)
                .Index(t => t.Team_Id);

            CreateTable(
                "dbo.Teams",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Coach = c.String(),
                        ShortName = c.String(),
                        LogoPath = c.String(),
                    })
                .PrimaryKey(t => t.Id);

        }

        public override void Down()
        {
            DropForeignKey("dbo.Players", "Team_Id", "dbo.Teams");
            DropIndex("dbo.Players", new[] { "Team_Id" });
            DropTable("dbo.Teams");
            DropTable("dbo.Players");
        }
    }
}
