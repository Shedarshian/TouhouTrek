using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 创作
    /// 个人影响力+1
    /// </summary>
    public class AT_N002 : ActionCard
    {
        protected override bool canUse(Game game, Request nowRequest, FreeUse useInfo, out UseRequest nextRequest)
        {
            nextRequest = null;
            return true;
        }
        public override async Task DoEffect(Game game, FreeUse useWay)
        {
            await Effects.UseCard.UseActionCard(game, useWay, this, (g, r) =>
            {
                g.GetPlayer(r.PlayerId).Size += 1;
                return Task.CompletedTask;
            });
        }
    }
}
