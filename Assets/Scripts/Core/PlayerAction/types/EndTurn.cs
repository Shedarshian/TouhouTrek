using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    public class EndTurn:Response
    {
        public override void HandleAction(Game game)
        {
            Player player = game.Players.Find(x => x.Id == playerId);
            game.EndTurn(player);
        }
    }
}
