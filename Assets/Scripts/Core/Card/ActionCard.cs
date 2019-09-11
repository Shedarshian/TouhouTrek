using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public class ActionCard:Card
    {
        public List<EffectBase> Effects;
        public override Type GetUseType()
        {
            if (Effects.Count > 0)
            {
                return Effects[0].GetUseType();
            }
            else
            {
                return typeof(Target.Simple);
            }
        }
        internal override void DoEffect(Game game, Target.TargetBase target)
        {
            foreach (var effect in Effects)
            {
                effect.Enable(game, target);
            }
        }
    }
}
