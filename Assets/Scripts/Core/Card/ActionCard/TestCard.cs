using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.Target;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 测试卡，社区影响力+1
    /// </summary>
    public class TestCard:ActionCard
    {
        public override void DoEffect(Game game, TargetBase target)
        {
            game.ChangeSize(1);
        }
    }
}
