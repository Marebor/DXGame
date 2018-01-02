namespace DXGame.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SomeChanges : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PlayroomPlayers", "Playroom_Name", "dbo.Playrooms");
            DropForeignKey("dbo.PlayroomPlayers", "Player_Name", "dbo.Players");
            DropIndex("dbo.PlayroomPlayers", new[] { "Playroom_Name" });
            DropIndex("dbo.PlayroomPlayers", new[] { "Player_Name" });
            DropTable("dbo.PlayroomPlayers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PlayroomPlayers",
                c => new
                    {
                        Playroom_Name = c.String(nullable: false, maxLength: 128),
                        Player_Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Playroom_Name, t.Player_Name });
            
            CreateIndex("dbo.PlayroomPlayers", "Player_Name");
            CreateIndex("dbo.PlayroomPlayers", "Playroom_Name");
            AddForeignKey("dbo.PlayroomPlayers", "Player_Name", "dbo.Players", "Name", cascadeDelete: true);
            AddForeignKey("dbo.PlayroomPlayers", "Playroom_Name", "dbo.Playrooms", "Name", cascadeDelete: true);
        }
    }
}
