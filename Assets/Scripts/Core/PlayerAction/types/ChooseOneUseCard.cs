using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    public class ChooseSomeoneRequest : Request
    {
        public int Number;
    }
    public class ChooseSomeoneResponse: UseOneCard
    {
        public List<Player> Targets;
        public override void HandleAction(Game game)
        {
            Player player = game.Players.Find(x => x.Id == playerId);
            player.UseActionCard(game, CardId, this);
        }
    }
}
