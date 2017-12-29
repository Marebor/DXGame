using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Newtonsoft.Json;

namespace DXGame.Models.Entities
{
    public class DXEvent
    {
        public int ID { get; set; }
        public string PerformedBy { get; set; }
        public string PlayroomName { get; set; }
        public DateTime DatePerformed { get; set; }
        public string Content { get; set; }

        public DXEvent(object @event)
        {
            Content = JsonConvert.SerializeObject(@event);
        }

        public DXEvent(string content)
        {
            Content = content;
        }

        public static bool ValidateContent(string content)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject(content);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}