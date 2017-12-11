using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using Microsoft.AspNet.SignalR;

namespace DXGame.Models.Realtime
{
    public class CommonHub : Hub
    {
        public CommonHub() : base()
        {

        }

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public void JoinPlayroom(string name)
        {

        }
    }
}