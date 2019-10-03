using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.UI.Battle
{
    /// <summary>
    /// 
    /// </summary>
    partial class UI_Main2
    {
        UI_PlayerSimpleInfo[] playersSimpleInfo;

        [BattleUI(nameof(SetGame))]
        private void SimpleInfo_SetGame()
        {
            playersSimpleInfo = new UI_PlayerSimpleInfo[game.Players.Count];
            for (int i = 0; i < 8; i++)
            {
                UI_PlayerSimpleInfo uI_PlayerSimpleInfo = GetChild("PlayerInfo" + i) as UI_PlayerSimpleInfo;
                if (i < game.Players.Count)
                {
                    uI_PlayerSimpleInfo.visible = true;
                    playersSimpleInfo[i] = uI_PlayerSimpleInfo;
                }
                else
                {
                    uI_PlayerSimpleInfo.visible = false;
                }
            }
        }

        [BattleUI(nameof(flush))]
        private void SimpleInfo_flush()
        {
            for (int i = 0; i < game.Players.Count; i++)
            {
                playersSimpleInfo[i].SetPlayer(game.Players[i]);
            }
        }
    }
}
