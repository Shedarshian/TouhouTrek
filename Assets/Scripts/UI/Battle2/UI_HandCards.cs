using FairyGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.UI.Battle
{
    public partial class UI_HandCards
    {
        GObjectPool CardPool;

        List<UI_Card> Cards = new List<UI_Card>();

        public EventListener OnCardClick;

        partial void Init()
        {
            CardPool = new GObjectPool(this.displayObject.cachedTransform);
            OnCardClick = new EventListener(this, "OnCardClick");
        }

        public override void Dispose()
        {
            CardPool.Clear();
            base.Dispose();
        }

        public void SetCards(List<ActionCard> cards, List<ActionCard> selected)
        {
            foreach (var ui_card in Cards.ToArray())
            {
                ReturnPool(ui_card);
            }
            foreach (var card in cards)
            {
                var ui_card = getCardFromPool();
                AddChild(ui_card);
                ui_card.SetCard(card);
                Cards.Add(ui_card);
                ui_card.onClick.Set(() =>
                {
                    OnCardClick.Call(ui_card);
                });
            }
            for (int i = 0; i < Cards.Count; i++)
            {
                Cards[i].x = 185 * i;
                Cards[i].y = selected.Contains(Cards[i].Card) ? 5 : 20;
            }
        }

        private UI_Card getCardFromPool()
        {
            var result = CardPool.GetObject(UI_Card.URL) as UI_Card;
            result.visible = true;
            return result;
        }
        private void ReturnPool(UI_Card card)
        {
            Cards.Remove(card);
            RemoveChild(card);
            CardPool.ReturnObject(card);
            card.visible = false;
        }
    }
}
