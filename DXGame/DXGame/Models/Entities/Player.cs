using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DXGame.Models.Entities
{
    public class Player
    {
        [Key]
        public string Name { get; set; }
        public ICollection<Playroom> Playrooms { get; set; }
    }
}