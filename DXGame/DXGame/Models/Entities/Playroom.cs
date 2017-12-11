using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXGame.Models.Entities
{
    public class Playroom
    {
        public string Name { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}