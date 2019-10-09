using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 墨菲定理：任意玩家决定事件牌发生方向时使用，你的个人影响力-1，事件牌的发生方向逆转(不影响扣置发生的事件)
    /// </summary>
    public class AT_N020 : ActionCard
    {
        public override Task DoEffect(Game game, FreeUse useWay)
        {
            throw new NotImplementedException();
        }

        protected override bool canUse(Game game, Request nowRequest, FreeUse useInfo, out UseRequest nextRequest)
        {
            throw new NotImplementedException();
        }
    }
}
