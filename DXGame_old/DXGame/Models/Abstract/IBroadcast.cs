using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DXGame.Models.Entities;

namespace DXGame.Models.Abstract
{
    public interface IBroadcast
    {
        //void PlayroomCreated(string name);
        //void PlayroomDeleted(string name);
        //void PlayerJoined(string playername, string playroom);
        //void PlayerLeft(string playername, string playroom);
        void Broadcast(string message);
        void BroadcastGroup(string message, string groupName);
    }
}
