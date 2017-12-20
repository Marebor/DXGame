using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

using DXGame.Models.Entities;

namespace DXGame.Models
{
    public class GameContext : DbContext
    {
        public GameContext() : base("name=GameContext")
        {
            Database.CreateIfNotExists();
        }

        public DbSet<Player> Players { get; set; }
        public DbSet<Playroom> Playrooms { get; set; }
        public DbSet<DXEvent> Events { get; set; }
    }
}