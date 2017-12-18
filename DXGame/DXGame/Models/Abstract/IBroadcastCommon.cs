using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXGame.Models.Abstract
{
    public interface IBroadcastCommon
    {
        void PlayroomCreated(string name);
        void PlayroomDeleted(string name);
        void PlayerJoined(string playername, string playroom);
        void PlayerLeft(string playername, string playroom);
    }
}
