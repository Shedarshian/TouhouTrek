using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.UI.Battle
{
    //用于处理倒计时
    partial class UI_Main2
    {
        [BattleUI(nameof(OnUpdate))]
        private void timeUpdate()
        {
            if (nowRequest != null)
            {
                m_TimeBar.max = nowRequest.TimeOut;
                m_TimeBar.value = nowRequest.RemainTime;
            }
        }
    }
}
