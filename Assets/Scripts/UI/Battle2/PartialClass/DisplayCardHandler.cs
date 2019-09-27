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
        [BattleUI(nameof(Init))]
        private void DisplayCard_Init()
        {
            m_useCard.enabled = false;
            m_Hand.OnCardClick.Add((evt) =>
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
                m_useCard.enabled = false;
                if (nowRequest.PlayerId == self.Id)
                {
                    if (selectedSkill != null)
                    {
                        PlayerAction.UseRequest nextRequest;
                        if (selectedSkill.CanUse(game, nowRequest,getFreeUseInfo(), out nextRequest))
                        {
                            m_useCard.enabled = true;
                        }
                        else
                        {
                            m_UseTip.text = nextRequest.RequestInfo;
                        }
                    }
                    else if (selectedCards.Count == 1)
                    {
                        PlayerAction.UseRequest nextRequest;
                        if (selectedCards[0].CanUse(game, nowRequest, getFreeUseInfo(),out nextRequest))
                        {
                            m_useCard.enabled = true;
                        }
                        else
                        {
                            m_UseTip.text = nextRequest.RequestInfo;
                        }
                    }
                }
            });
            m_useCard.onClick.Add(() =>
            {
                var useinfo = getFreeUseInfo();
                selectedCards.Clear();
                game.Answer(useinfo);
            });
            m_Endturn.onClick.Add(() =>
            {
                game.Answer(new PlayerAction.EndFreeUseResponse()
                {
                    PlayerId = self.Id,
                });
            });
        }

        PlayerAction.FreeUse getFreeUseInfo()
        {
            return new PlayerAction.FreeUse()
            {
                PlayerId = self.Id,
                Source = selectedCards.Select(x => x.Id).ToList(),
                CardId = selectedSkill != null || selectedCards.Count != 1 ? -1 : selectedCards[0].Id,
                SkillId = selectedSkill == null ? -1 : SkillHelper.getId(selectedSkill),
                PlayersId = selectedPlayers.Select(x => x.Id).ToList(),
            };
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
