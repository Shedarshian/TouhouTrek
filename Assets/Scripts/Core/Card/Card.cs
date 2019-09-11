using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public class Card
    {
        public int Id;
        public CardTypeEnum CardType;
        public List<EffectBase> Effects;
        public virtual void DoEffect(Game game, Target.TargetBase target)
        {
            foreach (var effect in Effects)
            {
                effect.DoEffect(game, target);
            }
        }

    }

    public enum CardTypeEnum
    {
        Charactor,
        Theme,
        Event,
        Action,
    }
}
