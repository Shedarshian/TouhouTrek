using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    /// <summary>
    /// 自由出牌时，卡或者技能可以用什么形式使用
    /// </summary>
    public class NextRequest
    {
        public int PlayerId;
        public string RequestInfo;
    }
    /// <summary>
    /// 需要选择一张卡
    /// </summary>
    public class CardChooseRequest : NextRequest
    {

    }
    /// <summary>
    /// 需要选择一个英雄
    /// </summary>
    public class HeroChooseRequest : NextRequest
    {
        public int Number = 1;
    }
}
