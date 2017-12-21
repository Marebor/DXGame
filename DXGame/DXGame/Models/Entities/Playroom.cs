using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DXGame.Models.Entities
{
    public class Playroom
    {
        [Key]
        public string Name { get; set; }
        public ICollection<Player> Players { get; set; }
    }
}