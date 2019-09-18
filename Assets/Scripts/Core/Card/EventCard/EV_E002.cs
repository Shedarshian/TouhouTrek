using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 全国性活动。 正：社群规模+2；逆：社群规模-1，个人影响力-1
    /// </summary>
    public class EV_E002 : EventCard
    {
        public override bool ForwardOnly => false;

        public override Task UseBackward(Game game, Player user)
        {
            return Task.CompletedTask;
        }

        public override Task UseForward(Game game, Player user)
        {
            return Task.CompletedTask;
        }
    }
}
