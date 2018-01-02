namespace DXGame.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using DXGame.Models;
    using DXGame.Models.Entities;

    // -ConfigurationTypeName Configuration_CardsDB

    internal sealed class Configuration_CardsDB : DbMigrationsConfiguration<DXGame.Models.CardsContext>
    {
        public Configuration_CardsDB()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DXGame.Models.CardsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            //var db = new CardsContext();
            //db.Cards.AddOrUpdate(new Card() { ID = 1, URL = "Content/Cards/Card_ID-0000000001.jpg" });
            //db.SaveChanges();
        }
    }
}
