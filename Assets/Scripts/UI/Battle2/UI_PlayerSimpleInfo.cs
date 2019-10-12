using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.UI.Battle
{
    public partial class UI_PlayerSimpleInfo
    {
        public Player Player;
        partial void Init()
        {

        }
        public void SetPlayer(Player player)
        {
            this.Player = player;
            m_Name.text = player.Name;
            m_ActionCards.text = player.ActionCards.Count.ToString();
            m_EventCards.text = (player.EventCards.Count + (player.SaveEvent == null ? 0 : 1)).ToString();
            m_Size.text = player.Size.ToString();
            m_Group.m_type.selectedIndex = 0;//TODO
            m_Hero.visible = player.Hero != null;
            if (player.Hero != null) m_Hero.m_Name.text = player.Hero.Name;
            //TODO 累计积分
            //TODO BUFF
        }
    }
}
