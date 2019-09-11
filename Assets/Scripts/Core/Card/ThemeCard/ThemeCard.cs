using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public class ThemeCard:Card
    {
        public List<EffectBase> Effects;
        public override Type GetUseType()
        {
            return typeof(Target.Simple);
        }
        internal  void Enable(Game game, Target.TargetBase target)
        {
            foreach (var effect in Effects)
            {
                effect.Enable(game, target);
            }
        }
        internal void Disable(Game game)
        {
            foreach (var effect in Effects)
            {
                effect.Disable(game);
            }
        }
    }
}
