using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.UI.Battle
{
    using PlayerAction;
    public partial class UI_Main2
    {
        [BattleUI(nameof(Init))]
        private void freeUseInit()
        {
            m_useCard.enabled = false;
            m_useCard.onClick.Add(() =>
            {
                var useinfo = getFreeUseInfo();
                selectedCards.Clear();
                game.Answer(useinfo);
            });
            m_Endturn.onClick.Add(() =>
            {
                game.Answer(new EndFreeUseResponse()
                {
                    PlayerId = self.Id,
                });
            });
            m_Hand.OnCardClick.Add((evt) =>
            {
                if (!(nowRequest is FreeUseRequest)) return;
                m_useCard.enabled = false;
                if (nowRequest.PlayerId == self.Id)
                {
                    if (selectedSkill != null)
                    {
                        UseRequest nextRequest;
                        if (selectedSkill.CanUse(game, nowRequest, getFreeUseInfo(), out nextRequest))
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
                        UseRequest nextRequest;
                        if (selectedCards[0].CanUse(game, nowRequest, getFreeUseInfo(), out nextRequest))
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
        }

        [BattleUI(nameof(onRequest))]
        private void freeUseRequestHandle()
        {
            if (nowRequest.PlayerId == self.Id && nowRequest is FreeUseRequest freeUseRequest)
            {
                m_Request.selectedIndex = 1;
                selectedCards.Clear();
                m_Hand.SetCards(self.ActionCards, selectedCards);
            }
        }

        FreeUse getFreeUseInfo()
        {
            return new FreeUse()
            {
                PlayerId = self.Id,
                Source = selectedCards.Select(x => x.Id).ToList(),
                CardId = selectedSkill != null || selectedCards.Count != 1 ? -1 : selectedCards[0].Id,
                SkillId = selectedSkill == null ? -1 : SkillHelper.getId(selectedSkill),
                PlayersId = selectedPlayers.Select(x => x.Id).ToList(),
            };
        }
    }
}
