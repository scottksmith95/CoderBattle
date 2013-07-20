using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace CoderBattle.Hubs
{
    public class FightHub : Hub
    {
        public void Start()
        {
            Clients.Caller.win();
        }
    }
}