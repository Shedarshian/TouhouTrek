using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FairyGUI;

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
                selectedSkill = null;
                flushSkills();
                game.Answer(useinfo);
            });
            m_Endturn.onClick.Add(() =>
            {
                game.Answer(new EndFreeUseResponse()
                {
                    PlayerId = self.Id,
                });
            });
            m_Hand.OnCardClick.Add(freeUse_CardClick);
            m_skills.onClickItem.Add(freeUse_SkillClick);
            for (int i = 0; i < 8; i++)
            {
                playersSimpleInfo[i].onClick.Add(freeUse_HeroClick);
            }
        }

        //[BattleUI(nameof(flush))]
        //private void freeUseFlush()
        //{

        //}

        [BattleUI(nameof(onRequest))]
        private void freeUseRequestHandle()
        {
            //自由出牌
            if (nowRequest.PlayerId == self.Id && nowRequest is FreeUseRequest freeUseRequest)
            {
                m_Request.selectedIndex = 1;
                selectedCards.Clear();
                m_Hand.SetCards(self.ActionCards, selectedCards);
            }
        }

        void flushSkills()
        {
            foreach (GButton ui_skill in m_skills.GetChildren())
            {
                ui_skill.selected = selectedSkill == ui_skill.data;
            }
        }

        void freeUse_HeroClick(EventContext evt)
        {
            UI_PlayerSimpleInfo playerSimpleInfo = evt.sender as UI_PlayerSimpleInfo;
            Log.Debug(playerSimpleInfo.Player.Name);
            if (selectedPlayers.Contains(playerSimpleInfo.Player))
            {
                selectedPlayers.Remove(playerSimpleInfo.Player);
            }
            else
            {
                selectedPlayers.Add(playerSimpleInfo.Player);
            }
            flushSelectPlayer();
            checkUseAble();
        }

        void freeUse_SkillClick(FairyGUI.EventContext evt)
        {
            var skill = (evt.data as GObject).data as Skill;
            if (selectedSkill == skill)
            {
                //再点一下表示取消选择
                selectedSkill = null;
            }
            else
            {
                selectedSkill = skill;
            }
            checkUseAble();
            flushSkills();
        }

        void freeUse_CardClick(FairyGUI.EventContext evt)
        {
            checkUseAble();
        }

        private void checkUseAble()
        {
            if (nowRequest == null|| nowRequest.PlayerId != self.Id) return;
            if (selectedSkill != null)
            {
                NextRequest nextRequest;
                if (selectedSkill.CanUse(game, nowRequest, getFreeUseInfo(), out nextRequest))
                {
                    m_useCard.enabled = true;
                    m_UseTip.text = "";//应该是确认是否使用
                }
                else
                {
                    m_useCard.enabled = false;
                    m_UseTip.text = nextRequest.RequestInfo;
                }
            }
            else if (selectedCards.Count == 1)
            {
                NextRequest nextRequest;
                if (selectedCards[0].CanUse(game, nowRequest, getFreeUseInfo(), out nextRequest))
                {
                    m_useCard.enabled = true;
                    m_UseTip.text = "";//应该是确认是否使用
                }
                else
                {
                    m_useCard.enabled = false;
                    m_UseTip.text = nextRequest.RequestInfo;
                }
            }
            else
            {
                m_useCard.enabled = false;
                m_UseTip.text = "";
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
