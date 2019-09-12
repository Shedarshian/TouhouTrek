using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Effect
{
    public class ChangeSelfSize : EffectBase<PlayerAction.ChooseOneUseCard>
    {
        public int Size;
        public override void Enable(Game game, ChooseOneUseCard target)
        {
            target.Target.ChangeSize(Size);
        }
    }
}
