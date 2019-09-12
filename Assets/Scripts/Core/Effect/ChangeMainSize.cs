using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.Effect
{
    public class ChangeMainSize:EffectBase<PlayerAction.Simple>
    {
        public int Size;
        public override void Enable(Game game, PlayerAction.Simple target)
        {
            game.ChangeSize(Size);
        }
    }
}
