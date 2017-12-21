namespace DXGame.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using DXGame.Models;
    using DXGame.Models.Entities;

    internal sealed class Configuration_GameDB : DbMigrationsConfiguration<DXGame.Models.GameContext>
    {
        public Configuration_GameDB()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DXGame.Models.GameContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            
        }
    }
}
