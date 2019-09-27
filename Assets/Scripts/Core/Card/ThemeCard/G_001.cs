using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 东方红魔乡：无法使用或者发动效果中包含判定动作的行动牌和技能。
    /// </summary>
    public class G_002 : ThemeCard
    {
        public override void Enable(Game game)
        {
            throw new NotImplementedException();
        }
        public override void Disable(Game game)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 旧作：无特殊效果
    /// </summary>
    public class G_001 : ThemeCard
    {
        public override void Disable(Game game)
        {

        }

        public override void Enable(Game game)
        {

        }
    }
}
