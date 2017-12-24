using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using DXGame.Models.Events;
using Newtonsoft.Json;

namespace DXGame.Models.Entities
{
    public class DXEvent : EventContentBase
    {
        public int ID { get; set; }
        public string Content { get; set; }
    }
}