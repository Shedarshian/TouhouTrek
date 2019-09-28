using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.UI.Battle
{
    //用于处理场上能看见的卡
    partial class UI_Main2
    {
        [BattleUI(nameof(Init),true)]
        private void DisplayCard_Init()
        {
            //这里必须先行其他注册事件，否则其他位置的手牌会出错
            //点选卡片
            m_Hand.OnCardClick.Add(evt =>
            {
                UI_Card uI_Card = evt.data as UI_Card;
                ActionCard card = uI_Card.Card as ActionCard;
                if (selectedCards.Contains(card))
                {
                    selectedCards.Remove(card);
                }
                else
                {
                    selectedCards.Add(card);
                }
                m_Hand.SetCards(self.ActionCards, selectedCards);
            });
        }

        [BattleUI(nameof(flush))]
        private void DisplayCardflush()
        {
            m_MainSize.m_size.selectedIndex = game.Size + 10;
            m_EventDeckCount.text = game.EventDeck.Count.ToString();
            m_ActionDeckCount.text = game.ActionDeck.Count.ToString();
            m_ThemeDeckCount.text = game.UsedThemeDeck.Count.ToString();
            m_EventDropDeckCount.text = game.UsedEventDeck.Count.ToString();
            m_ActionDeckCount.text = game.UsedActionDeck.Count.ToString();
            //m_NowTheme game.ActiveTheme
        }

        [BattleUI(nameof(onResponse))]
        private void DisplayCard_UseCard()
        {
            m_Hand.SetCards(self.ActionCards, selectedCards);
            flush();
        }

        [BattleUI(nameof(SetGame))]
        private void DisplayCard_NewGame()
        {
            game.EventSystem.Register(EventEnum.DrawActionCard,100, (x) =>
            {
                Player player = x[0] as Player;
                if (player.Id == self.Id)
                {
                    selectedCards.Clear();
                    m_Hand.SetCards(player.ActionCards, selectedCards);
                }
                return Task.CompletedTask;
            });
        }
    }
}
