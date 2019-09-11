using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public class EventCard:Card
    {
        public List<EffectBase> ForwardEffects, BackwardEffects;
        public override Type GetUseType()
        {
            return typeof(Target.ChooseDirection);
        }
    }
}
