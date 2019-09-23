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
        public abstract bool CanUse(Game game, Request nowRequest, FreeUse useInfo, out UseRequest nextRequest);

        public abstract Task DoEffect(Game game, FreeUse useWay);
    }
}
