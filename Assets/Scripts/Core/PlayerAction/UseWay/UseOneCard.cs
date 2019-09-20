using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    /// <summary>
    /// 使用一张牌，这张牌能由其他卡转换过来
    /// </summary>
    public abstract class UseOneCard : UseInfo
    {
        public int CardId = -1;

        public List<int> Source;

        public int TreatAs = -1;
    }
}
