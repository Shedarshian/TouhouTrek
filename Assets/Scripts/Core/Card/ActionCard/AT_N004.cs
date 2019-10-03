using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 约稿
    /// 社群规模±1，选择一名玩家并给x张牌，你与其个人影响力+x
    /// </summary>
    public class AT_N004 : ActionCard
    {
        protected override bool canUse(Game game, Request nowRequest, FreeUse useInfo, out UseRequest nextRequest)
        {
            throw new NotImplementedException();
        }

        public override Task DoEffect(Game game, FreeUse useWay)
        {
            throw new NotImplementedException();
        }
    }
}
