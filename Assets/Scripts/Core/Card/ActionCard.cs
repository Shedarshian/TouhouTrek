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

        public PlayerAction.UseWay UseWay;

        internal override void DoEffect(Game game, PlayerAction.Response target)
        {
            foreach (var effect in Effects)
            {
                effect.DoEnable(game, target);
            }
        }
    }
}
