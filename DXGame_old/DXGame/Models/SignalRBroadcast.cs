using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.SignalR;

using DXGame.Models.Abstract;
using DXGame.Models.Realtime;
using DXGame.Models.Entities;

namespace DXGame.Models
{
    public class SignalRBroadcast : IBroadcast
    {
        private readonly static IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<GameHub>();

        public void Broadcast(string message)
        {
            _context.Clients.All.onDXCommonEvent(message);
        }

        public void BroadcastGroup(string message, string groupName)
        {
            _context.Clients.Group(groupName).onDXPlayroomEvent(message);
        }
    }
}