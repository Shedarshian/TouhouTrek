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
        internal void Enable(Game game, PlayerAction.Response target)
        {
            foreach (var effect in Effects)
            {
                effect.DoEnable(game, target);
            }
        }
    }
}
