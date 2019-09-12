using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    public class UseCardBase:ActionBase
    {
        public int CardId;
        public override void HandleAction(Game game, Player player)
        {
            player.UseCard(game, CardId, this);
        }
    }
}
