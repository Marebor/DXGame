using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXGame.Models.Entities
{
    public class Player
    {
        public string Name { get; set; }
        public ICollection<Playroom> Playrooms { get; set; }
    }
}