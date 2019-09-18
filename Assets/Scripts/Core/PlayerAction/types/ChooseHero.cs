using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.PlayerAction
{
    /// <summary>
    /// 游戏开始时选择英雄
    /// </summary>
    public class ChooseHeroRequest:Request
    {
        public List<int> HeroIds;
    }
    public class ChooseHeroResponse : Response
    {
        public int HeroId;
    }
}
