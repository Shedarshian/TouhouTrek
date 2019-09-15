using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    public class ChooseSomeCardRequest :Request
    {
        public int Count;
    }
    public class ChooseSomeCardResponse : Response
    {
        public List<ActionCard> Cards;
        public override void HandleAction(Game game)
        {
            Log.Debug("234");
            Player player = game.Players.Find(x => x.Id == playerId);
            player.DropActionCard(game, Cards);
        }
    }
}
