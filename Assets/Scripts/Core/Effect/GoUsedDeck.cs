using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ
{
    public partial class Effects
    {
        public static void GoUsedDeck(Game game, Card card, UseOneCard response)
        {
            Player player = game.Players.Find(x => x.Id == response.PlayerId);
            switch (card)
            {
                case ActionCard actionCard:
                    //这里要和弃牌分开来
                    List<ActionCard> data = new List<ActionCard>();
                    player.ActionCards.Remove(actionCard);
                    game.UsedDeck.Add(actionCard);
                    data.Add(actionCard);
                    break;
                case EventCard eventCard:
                    player.DropEventCard(game, eventCard);
                    break;
            }
        }
    }
}
