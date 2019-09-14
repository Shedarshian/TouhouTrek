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
    public class SimpleRequest: Request
    {
        public readonly static SimpleRequest Instance = new SimpleRequest();
    }
    public class SimpleResponse : Response
    {
        public int CardId;
        public override void HandleAction(Game game)
        {
            Player player = game.Players.Find(x => x.Id == playerId);
            player.UseCard(game, CardId, this);
        }
    }
}
