using System.Collections.Generic;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 出警：指定一名玩家并进行一次两点点数判定，判定结果为X，该玩家影响力-X，社群规模±X。
    /// </summary>
    public class AT_N012 : ActionCard
    {
        protected override bool canUse(Game game, Request nowRequest, FreeUse useInfo, out NextRequest nextRequest)
        {
            if (useInfo.PlayersId.Count < 1)
            {
                nextRequest = new HeroChooseRequest();
                return false;
            }
            else
            {
                nextRequest = null;
                return true;
            }
        }
        public override async Task DoEffect(Game game, FreeUse useWay)
        {
            await Effects.UseCard.UseActionCard(game, useWay, this, async (g, r) =>
            {
                int x = g.twoPointCheck();
                g.GetPlayer(r.PlayersId[0]).ChangeSize(g, -x, this);
                TakeChoiceResponse response = await g.WaitAnswer(new TakeChoiceRequest() { PlayerId = r.PlayerId, Infos = new List<string>() { "-2", "+2" } }) as TakeChoiceResponse;
                if (response.Index == 0)
                    g.Size -= x;
                else
                    g.Size += x;
            }
        }
    }
}
