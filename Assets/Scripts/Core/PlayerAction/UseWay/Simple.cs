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
        public override Task HandleAction(Game game)
        {
            return game.GetPlayer(PlayerId).UseActionCard(game, CardId, this);
        }
    }
}
