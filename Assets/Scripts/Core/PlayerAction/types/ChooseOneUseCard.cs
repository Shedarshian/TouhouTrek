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
    public class ChooseSomeoneResponse: Response
    {
        public List<Player> Targets;
        public int CardId;
        public override void HandleAction(Game game)
        {
            Log.Debug("123");
            Player player = game.Players.Find(x => x.Id == playerId);
            player.UseCard(game, CardId, this);
        }
    }
}
