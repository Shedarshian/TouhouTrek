using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.Effect
{
    public class ChangeMainSize:EffectBase<Target.Simple>
    {
        public int Size;
        public override void Enable(Game game, Target.Simple target)
        {
            game.ChangeSize(Size);
        }
    }
}
