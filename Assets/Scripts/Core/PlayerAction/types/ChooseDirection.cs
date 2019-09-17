using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    /// <summary>
    /// 选择事件卡的出牌方向
    /// </summary>
    public class ChooseDirectionRequest : Request
    {
        //public EventCard EventCard;
    }
    public class ChooseDirectionResponse : UseOneCard
    {
        public bool IfForward;
        public bool IfSet;
    }
}
