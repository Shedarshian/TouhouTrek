using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    using PlayerAction;
    using Target;
    public class Player
    {
        public int Size;
        /// <summary>
        /// 手牌
        /// </summary>
        public List<Card> Cards = new List<Card>();
        /// <summary>
        /// 被玩家扣住的事件
        /// </summary>
        public Card SaveEvent;
        internal void UseCard( Game game, int cardId, TargetBase cardTarget)
        {
            Card card = Cards.Find(x => x.Id == cardId);
            if (card == null) return;
            card.DoEffect(game, cardTarget); 
        }
        internal void UseSkill()
        {

        }

        internal void ChangeSize(int Size)
        {

        }
    }
}
