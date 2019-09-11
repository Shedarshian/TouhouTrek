using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.Target;

namespace ZMDFQ.Effect
{
    public class ChangeSelfSize : EffectBase<Target.ChooseOne>
    {
        public int Size;
        public override void DoEffect(Game game, ChooseOne target)
        {
            target.Target.ChangeSize(Size);
        }
    }
}
