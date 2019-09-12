using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;
using FairyGUI.Utils;

namespace ZMDFQ.UI
{
    public class UI_Hands : GComponent
    {
        public const string URL = "ui://oacz4rtmqfxq4";

        GObjectPool CardPool;

        List<UI_ActionCard> Cards=new List<UI_ActionCard>();

        public EventListener OnCardClick;

        public static UI_Hands CreateInstance()
        {
            return (UI_Hands)UIPackage.CreateObject("Battle", "HandCards");
        }
        public override void ConstructFromXML(XML xml)
        {
            base.ConstructFromXML(xml);
            OnCardClick = new EventListener(this, "OnCardClick");
            CardPool = new GObjectPool(this.displayObject.cachedTransform);
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
                Cards[i].y = selected.Contains(Cards[i].Card) ? -20 : 0;
            }
        }

        private UI_ActionCard getCardFromPool()
        {
            var result= CardPool.GetObject(UI_ActionCard.URL) as UI_ActionCard;
            result.visible = true;
            return result;
        }
        private void ReturnPool(UI_ActionCard card)
        {
            Cards.Remove(card);
            RemoveChild(card);
            CardPool.ReturnObject(card);
            card.visible = false;
        }
    }
}
