using System.Collections.Generic;

namespace ZMDFQ
{
    using PlayerAction;
    using System.Threading.Tasks;

    public class Player
    {
        public int Id;
        public int Size;
        /// <summary>
        /// 手牌
        /// </summary>
        public List<ActionCard> ActionCards = new List<ActionCard>();

        /// <summary>
        /// 玩家手上的事件牌
        /// </summary>
        public List<EventCard> EventCards = new List<EventCard>();

        /// <summary>
        /// 被玩家扣住的事件
        /// </summary>
        public EventCard SaveEvent;

        public HeroCard Hero;

        internal void DrawActionCard(Game game,int count)
        {
            List<ActionCard> data = new List<ActionCard>();
            for (int i = 0; i < count; i++)
            {
                if (game.Deck.Count == 0)
                {
                    game.Deck.AddRange(game.UsedDeck);
                    game.UsedDeck.Clear();
                    game.Reshuffle(game.Deck);
                }
                ActionCards.Add(game.Deck[0]);
                data.Add(game.Deck[0]);
                game.Deck.RemoveAt(0);
            }
            game.EventSystem.Call(EventEnum.DrawActionCard, this, data);
        }

        internal void DrawEventCard(Game game)
        {
            EventCard card = game.EventDeck[0];
            EventCards.Add(card);
            game.EventDeck.Remove(card);
            game.EventSystem.Call(EventEnum.DrawEventCard, this, card);
        }

        internal Task UseEventCard(Game game, ChooseDirectionResponse response)
        {
            if (response.IfSet)
            {
                return SetEventCard(game, response);
            }
            else
            {
                //默认玩家手上一定是一张事件卡，有其他情况再改
                if (response.IfForward)
                    return EventCards[0].UseForward(game, this);
                else
                    return EventCards[0].UseBackward(game, this);
            }
        }

        private async Task SetEventCard(Game game, ChooseDirectionResponse response)
        {
            if (SaveEvent != null)
            {
                await SaveEvent.UseForward(game, this);
            }
            SaveEvent = EventCards[0];
            EventCards.RemoveAt(0);
        }

        internal void DropEventCard(Game game,EventCard card)
        {
            if (card == SaveEvent)
            {
                game.UsedEventDeck.Add(card);
                SaveEvent = null;
            }
            else
            {
                game.UsedEventDeck.Add(card);
                EventCards.Remove(card);
            }
        }

        internal Task UseActionCard(Game game, int cardId, UseOneCard cardTarget)
        {
            ActionCard card = ActionCards.Find(x => x.Id == cardId);
            if (card == null) return Task.CompletedTask;
            return card.DoEffect(game, cardTarget);
        }

        internal void DropActionCard(Game game, List<ActionCard> cards)
        {
            List<ActionCard> data = new List<ActionCard>();
            foreach (var card in cards)
            {
                ActionCards.Remove(card);
                game.UsedDeck.Add(card);
                data.Add(card);
            }
            game.EventSystem.Call(EventEnum.DropActionCard, this, data);
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
