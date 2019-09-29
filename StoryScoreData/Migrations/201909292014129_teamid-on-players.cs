namespace StoryScore.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class teamidonplayers : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Players", "Team_Id", "dbo.Teams");
            //DropIndex("dbo.Players", new[] { "Team_Id" });

            ////AlterColumn("dbo.Players", "Team_Id", c => c.Int(nullable: false));
            //CreateIndex("dbo.Players", "Team_Id");
            //AddForeignKey("dbo.Players", "Team_Id", "dbo.Teams", "Id", cascadeDelete: true);
        }

        public override void Down()
        {
            //DropForeignKey("dbo.Players", "Team_Id", "dbo.Teams");
            //DropIndex("dbo.Players", new[] { "Team_Id" });
            ////AlterColumn("dbo.Players", "Team_Id", c => c.Int());
            //CreateIndex("dbo.Players", "Team_Id");
            //AddForeignKey("dbo.Players", "Team_Id", "dbo.Teams", "Id");
        }
    }
}
