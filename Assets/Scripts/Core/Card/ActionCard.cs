using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ
{
    public abstract class ActionCard : Card
    {
        public bool CanUse(Game game, Request nowRequest, FreeUse useInfo, out UseRequest nextRequest)
        {
            UseRequest request;
            bool result = canUse(game, nowRequest, useInfo, out request);
            EventData<bool> boolData = new EventData<bool>() { data = result };
            EventData<UseRequest> nextRequestData = new EventData<UseRequest>() { data = request };
            game.EventSystem.Call(EventEnum.onCheckCanUse, this, boolData, nextRequestData).GetAwaiter().GetResult();
            nextRequest = nextRequestData.data;
            return boolData.data;
        }

        protected abstract bool canUse(Game game, Request nowRequest, FreeUse useInfo, out UseRequest nextRequest);

        public abstract Task DoEffect(Game game, FreeUse useWay);
    }
}
