using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    /// <summary>
    /// 选择手上的一些牌
    /// </summary>
    public class ChooseSomeCardRequest :Request
    {
        /// <summary>
        /// 需要选择的数量
        /// </summary>
        public int Count;
    }
    public class ChooseSomeCardResponse : Response
    {
        public List<ActionCard> Cards;
    }
}
