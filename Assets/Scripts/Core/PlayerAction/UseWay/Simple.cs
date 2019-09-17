using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    /// <summary>
    /// 直接使用牌
    /// </summary>
    public class SimpleRequest: UseWay
    {
        public readonly static SimpleRequest Instance = new SimpleRequest();
    }
    public class SimpleResponse : UseOneCard
    {
        public override void HandleAction(Game game)
        {
            Player player = game.Players.Find(x => x.Id == PlayerId);
            player.UseActionCard(game, CardId, this);
        }
    }
}
