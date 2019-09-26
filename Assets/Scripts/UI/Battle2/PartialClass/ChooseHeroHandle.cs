using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.UI.Battle
{
    /// <summary>
    /// 处理选英雄逻辑
    /// </summary>
    public partial class UI_Main2
    {
        PlayerAction.ChooseHeroRequest chooseHeroRequset;

        [BattleUI(nameof(Init))]
        private void chooseHeroInit()
        {
            for (int i = 0; i < 3; i++)
            {
                var card = (m_HeroChooseWindow.GetChild("p" + i) as UI_Card);
                card.onClick.Add(() =>
                {
                    game.Answer(new PlayerAction.ChooseHeroResponse()
                    {
                        HeroId = card.Card.Id,
                        PlayerId = self.Id,
                    });
                });
            }
        }

        [BattleUI(nameof(OnUpdate))]
        private void updateHeroChooseTime()
        {
            if (chooseHeroRequset != null)
            {
                m_HeroChooseWindow.m_time.max = chooseHeroRequset.TimeOut;
                m_HeroChooseWindow.m_time.value = chooseHeroRequset.RemainTime;
            }
        }

        [BattleUI(nameof(onRequest))]
        private void chooseHeroRequest()
        {
            if ((nowRequest.PlayerId == self.Id) && nowRequest is PlayerAction.ChooseHeroRequest chooseHeroRequset)
            {
                this.chooseHeroRequset = chooseHeroRequset;
                m_ChooseHero.selectedIndex = 1;
                for (int i = 0; i < chooseHeroRequset.HeroIds.Count; i++)
                {
                    int id = chooseHeroRequset.HeroIds[i];
                    (m_HeroChooseWindow.GetChild("p" + i) as UI_Card).SetCard(game.GetCard(id));
                }
            }
        }

        [BattleUI(nameof(onResponse))]
        private void chooseHeroReponse()
        {
            if (nowResponse.PlayerId==self.Id&&nowResponse is PlayerAction.ChooseHeroResponse chooseHeroResponse)
            {
                this.chooseHeroRequset = null;
                m_ChooseHero.selectedIndex = 0;
            }
        }
    }
}
