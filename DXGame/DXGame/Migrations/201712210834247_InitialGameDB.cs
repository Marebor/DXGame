namespace DXGame.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialGameDB : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DXEvents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        PerformedBy = c.String(),
                        DatePerformed = c.DateTime(nullable: false),
                        PlayroomName = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Players",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.Playrooms",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.PlayroomPlayers",
                c => new
                    {
                        Playroom_Name = c.String(nullable: false, maxLength: 128),
                        Player_Name = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.Playroom_Name, t.Player_Name })
                .ForeignKey("dbo.Playrooms", t => t.Playroom_Name, cascadeDelete: true)
                .ForeignKey("dbo.Players", t => t.Player_Name, cascadeDelete: true)
                .Index(t => t.Playroom_Name)
                .Index(t => t.Player_Name);
            
            DropTable("dbo.Cards");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Cards",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        URL = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            DropForeignKey("dbo.PlayroomPlayers", "Player_Name", "dbo.Players");
            DropForeignKey("dbo.PlayroomPlayers", "Playroom_Name", "dbo.Playrooms");
            DropIndex("dbo.PlayroomPlayers", new[] { "Player_Name" });
            DropIndex("dbo.PlayroomPlayers", new[] { "Playroom_Name" });
            DropTable("dbo.PlayroomPlayers");
            DropTable("dbo.Playrooms");
            DropTable("dbo.Players");
            DropTable("dbo.DXEvents");
        }
    }
}
