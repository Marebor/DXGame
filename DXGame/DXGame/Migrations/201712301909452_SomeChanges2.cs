namespace DXGame.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SomeChanges2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Players", "Playroom_Name", c => c.String(maxLength: 128));
            CreateIndex("dbo.Players", "Playroom_Name");
            AddForeignKey("dbo.Players", "Playroom_Name", "dbo.Playrooms", "Name");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Players", "Playroom_Name", "dbo.Playrooms");
            DropIndex("dbo.Players", new[] { "Playroom_Name" });
            DropColumn("dbo.Players", "Playroom_Name");
        }
    }
}
