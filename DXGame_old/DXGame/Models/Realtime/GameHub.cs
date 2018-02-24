using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using Microsoft.AspNet.SignalR;

namespace DXGame.Models.Realtime
{
    public class GameHub : Hub
    {
        public void SubscribePlayroom(string name)
        {
            Groups.Add(Context.ConnectionId, name);
        }

        public void UnsubscribePlayroom(string name)
        {
            Groups.Remove(Context.ConnectionId, name);
        }
    }
}