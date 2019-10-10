using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 盈利
    /// 抽两张行动牌
    /// </summary>
    public class AT_N003 : ActionCard
    {
        public override string Name => "盈利";
        protected override bool canUse(Game game, Request nowRequest, FreeUse useInfo, out NextRequest nextRequest)
        {
            nextRequest = null;
            return true;
        }
        public override async Task DoEffect(Game game, FreeUse useWay)
        {
            await Effects.UseCard.UseActionCard(game, useWay, this, async (g, r) =>
            {
                await g.GetPlayer(r.PlayerId).DrawActionCard(game, 2);
            });
        }
    }
}
