using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    /// <summary>
    /// 自由行动阶段，卡或者技能可以用什么形式使用
    /// </summary>
    public class UseWay
    {
        public int PlayerId;
    }
    /// <summary>
    /// 使用一张卡的信息
    /// </summary>
    public abstract class UseInfo : Response
    {
        public abstract Task HandleAction(Game game);
    }
}
