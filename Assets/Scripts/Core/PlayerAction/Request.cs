using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    /// <summary>
    /// 向一名玩家请求完成一个操作,玩家需要返回对应的Response
    /// </summary>
    public class Request
    {
        /// <summary>
        /// 超时时间
        /// </summary>
        public float TimeOut = 5;
        /// <summary>
        /// 询问的目标玩家
        /// </summary>
        public int PlayerId = -1;
    }
}
