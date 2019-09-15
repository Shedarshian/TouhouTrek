using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Effect
{
    public class GoUsedDeck:EffectBase<PlayerAction.UseOneCard>
    {
        public override void Enable(Game game, UseOneCard response)
        {
            Player player = game.Players.Find(x => x.Id == response.playerId);
            Card card = Parent as Card;
            switch (card)
            {
                case ActionCard actionCard:
                    player.DropActionCard(game, new List<ActionCard>() { actionCard });
                    break;
                case EventCard eventCard:
                    player.DropEventCard(game, eventCard);
                    break;
            }

        }
    }
}
