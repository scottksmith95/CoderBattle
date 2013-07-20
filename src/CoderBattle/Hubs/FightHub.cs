using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using Battler;

namespace CoderBattle.Hubs
{
    public class FightHub : Hub
    {
        public async Task Start(string fighter1, string fighter2)
        {
            var battle = new Battle();

            await battle.Init(fighter1, fighter2);

            var boutResult = await battle.FightLanguages();
            Clients.Caller.boutComplete(boutResult);

            boutResult = await battle.FightEnvironments();
            Clients.Caller.boutComplete(boutResult);

            boutResult = await battle.FightFrameworks();
            Clients.Caller.boutComplete(boutResult);

            boutResult = await battle.FightTools();
            Clients.Caller.boutComplete(boutResult);

            boutResult = await battle.FightInterests();
            Clients.Caller.boutComplete(boutResult);

            boutResult = await battle.FightTraits();
            Clients.Caller.boutComplete(boutResult);

            boutResult = await battle.FightAreas();
            Clients.Caller.boutComplete(boutResult);
        }
    }
}