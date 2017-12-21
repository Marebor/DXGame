using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DXGame.Models.Events
{
    public abstract class EventContentBase
    {
        public int ID { get; set; }
        public string PerformedBy { get; set; }
        public string PlayroomName { get; set; }
        public DateTime DatePerformed { get; set; }
    }
}