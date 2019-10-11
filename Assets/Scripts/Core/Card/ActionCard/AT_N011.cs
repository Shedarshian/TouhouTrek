using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 商业互吹：指定一名玩家，由你分配以下两个效果分别对你和该玩家生效：
    /// a.个人影响力+1
    /// b.抽一张行动牌
    /// </summary>
    public class AT_007 : ActionCard
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
            await Effects.UseCard.UseActionCard(game, useWay, this, async (g, r) =>
            {
                Player target = g.GetPlayer(r.PlayersId[0]);
                TakeChoiceResponse response = await g.WaitAnswer(new TakeChoiceRequest() { PlayerId = r.PlayerId, Infos = new List<string>() { "你个人影响力+1，对方抽一张牌", "你抽一张牌，对方个人影响力+1" } }) as TakeChoiceResponse;
                if (response.Index == 0)
                {
                    await Owner.ChangeSize(g, 1, this);
                    await target.DrawActionCard(g, 1);
                }
                else
                {
                    await Owner.DrawActionCard(g, 1);
                    await target.ChangeSize(g, 1, this);
                }
            });
        }
    }
}