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

        [BattleUI(nameof(Init),true)]
        private void SimpleInfo_Init()
        {
            playersSimpleInfo = new UI_PlayerSimpleInfo[8];
            for (int i = 0; i < 8; i++)
            {
                int k = i;
                UI_PlayerSimpleInfo uI_PlayerSimpleInfo = GetChild("PlayerInfo" + i) as UI_PlayerSimpleInfo;
                playersSimpleInfo[i] = uI_PlayerSimpleInfo;
                playersSimpleInfo[i].changeStateOnClick = false;
            }
        }

        [BattleUI(nameof(SetGame))]
        private void SimpleInfo_SetGame()
        {
            for (int i = 0; i < 8; i++)
            {               
                if (i < game.Players.Count)
                {
                    playersSimpleInfo[i].visible = true;
                  
                }
                else
                {
                    playersSimpleInfo[i].visible = false;
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

        private void flushSelectPlayer()
        {
            for (int i = 0; i < playersSimpleInfo.Length; i++)
            {
                playersSimpleInfo[i].selected = selectedPlayers.Contains(playersSimpleInfo[i].Player);
            }
        }
    }
}
