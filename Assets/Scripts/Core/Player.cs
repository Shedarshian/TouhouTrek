using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    using PlayerAction;
    public class Player
    {
        public int Id;
        public int Size;
        /// <summary>
        /// 手牌
        /// </summary>
        public List<ActionCard> Cards = new List<ActionCard>();
        /// <summary>
        /// 被玩家扣住的事件
        /// </summary>
        public EventCard SaveEvent;

        public Hero Hero;

        internal void DrawActionCard(Game game,int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (game.Deck.Count == 0)
                {
                    game.Deck.AddRange(game.UsedDeck);
                    game.UsedDeck.Clear();
                    game.Reshuffle(game.Deck);
                }
                Cards.Add(game.Deck[0]);
                game.Deck.RemoveAt(0);
            }
        }

        internal void UseCard(Game game, int cardId, Response cardTarget)
        {
            ActionCard card = Cards.Find(x => x.Id == cardId);
            if (card == null) return;
            card.DoEffect(game, cardTarget);
            DropActionCard(game,new List<ActionCard>() { card });
        }

        internal void DropActionCard(Game game, List<ActionCard> cards)
        {
            foreach (var card in cards)
            {
                Cards.Remove(card);
                game.UsedDeck.Add(card);
            }
        }

        internal void UseSkill()
        {

        }

        internal void ChangeSize(int Size)
        {

        }

        internal int HandMax()
        {
            int result=1;
            if (Size > 1 && Size < 4)
            {
                result = Size;
            }
            else if (Size >= 4)
                result = 4;
            return result;
        }
    }
}
