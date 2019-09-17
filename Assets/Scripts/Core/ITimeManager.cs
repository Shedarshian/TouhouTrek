using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ZMDFQ
{
    using PlayerAction;
    public interface ITimeManager
    {
        Game Game { set; }
        /// <summary>
        /// 注册一个询问，超时后自动回应
        /// </summary>
        /// <param name="request"></param>
        void Register(Request request);

        /// <summary>
        /// 取消一个询问
        /// </summary>
        /// <param name="request"></param>
        void Cancel(Request request);
    }
}
