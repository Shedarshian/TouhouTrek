using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    public class DropCardRequest:Request
    {
        public int Count;
    }
    public class DropCardResponse : Response
    {
        public List<ActionCard> Cards;
        public override void HandleAction(Game game)
        {
            Player player = game.Players.Find(x => x.Id == playerId);
            player.DropActionCard(game, Cards);
        }
    }
}
