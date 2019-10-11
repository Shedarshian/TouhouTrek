using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 造谣：你的个人影响力-1，指定一名玩家的个人影响力-2。
    /// </summary>
    public class AT_008 : ActionCard
    {
        protected override bool canUse(Game game, Request nowRequest, FreeUse useInfo, out NextRequest nextRequest)
        {
            nextRequest = null;
            switch (nowRequest)
            {
                case UseLimitCard useLimitCard:
                    return Effects.UseWayResponse.CheckLimit(game, useLimitCard, useInfo, ref nextRequest, this);
                case FreeUseRequest freeUse:
                    if (useInfo.PlayersId.Count < 1)
                    {
                        nextRequest = new HeroChooseRequest() { };
                        return false;
                    }
                    else
                    {
                        return true;
                    }
            }
            return false;
        }

        public override async Task DoEffect(Game game, FreeUse useWay)
        {
            await Owner.ChangeSize(game, -1, this);
            await game.GetPlayer(useWay.PlayersId[0]).ChangeSize(game, -2, this);
        }
    }
}