using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.SignalR;

using DXGame.Models.Abstract;
using DXGame.Models.Realtime;

namespace DXGame.Models
{
    public class BroadcastCommon : IBroadcastCommon
    {
        private readonly static IHubContext _context = GlobalHost.ConnectionManager.GetHubContext<GameHub>();

        public void PlayroomCreated(string name)
        {
            _context.Clients.All.playroomCreated(name);
        }

        public void PlayroomDeleted(string name)
        {
            _context.Clients.All.playroomDeleted(name);
        }

        public void PlayerJoined(string playername, string playroom)
        {
            _context.Clients.All.playerJoined(playername, playroom);
        }

        public void PlayerLeft(string playername, string playroom)
        {
            _context.Clients.All.playerLeft(playername, playroom);
        }
    }
}