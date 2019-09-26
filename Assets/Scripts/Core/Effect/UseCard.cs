using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.Effects
{
    public class UseCard
    {
        /// <summary>
        /// 正常的结算流程：结算牌放结算区，结算完毕后进入弃牌堆
        /// </summary>
        /// <param name="game"></param>
        /// <param name="useOneCard"></param>
        /// <param name="card"></param>
        /// <param name="effect"></param>
        public static async Task NormalUse(Game game, PlayerAction.FreeUse useOneCard, Card card, Func<Game, PlayerAction.FreeUse, Task> effect)
        {
            await NormalUse<PlayerAction.FreeUse>(game, useOneCard, card, effect);
            //Player player = game.GetPlayer(useOneCard.PlayerId);
            //if (card is ActionCard actionCard)
            //    await player.DropActionCard(game, new List<int>() { actionCard.Id });
            ////结算前把牌放入结算区
            //game.AddUsingCard(card);
            //game.AddUsingInfo(new UsingInfo()
            //{
            //    Card = card,
            //    Info = $"{player.Hero.Name}使用了{card.Name}",
            //});
            //await effect(game, useOneCard);
            ////结算完毕进入弃牌堆
            //if (card is ActionCard actionCard1)
            //    game.UsedActionDeck.Add(actionCard1);
        }
        public static async Task NormalUse<T>(Game game, T useOneCard, Card card, Func<Game, T, Task> effect) where T : PlayerAction.Response
        {
            Player player = game.GetPlayer(useOneCard.PlayerId);
            if (card is ActionCard actionCard)
                await player.DropActionCard(game, new List<int>() { actionCard.Id });
            else if (card is EventCard eventCard)
                await player.DropEventCard(game, eventCard);
            //结算前把牌放入结算区
            game.AddUsingCard(card);
            game.AddUsingInfo(new UsingInfo()
            {
                Card = card,
                Info = $"{player.Hero.Name}使用了{card.Name}",
            });
            await effect(game, useOneCard);
            //结算完毕进入弃牌堆
            if (card is ActionCard actionCard1)
                game.UsedActionDeck.Add(actionCard1);
            else if (card is EventCard eventCard1 && !game.ChainEventDeck.Contains(eventCard1))
                game.UsedEventDeck.Add(eventCard1);
        }
    }
}
