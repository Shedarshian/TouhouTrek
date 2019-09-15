using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    public class EventCard:Card
    {
        public bool ForwardOnly;
        public List<EffectBase> ForwardEffects, BackwardEffects;
        internal override void DoEffect(Game game,PlayerAction.Response response)
        {
            bool forward = (response as PlayerAction.ChooseDirectionResponse).IfForward;
            if (forward)
            {
                foreach (var effect in ForwardEffects)
                {
                    effect.DoEnable(game, response);
                }
            }
            else if (!ForwardOnly)
            {
                foreach (var effect in BackwardEffects)
                {
                    effect.DoEnable(game, response);
                }
            }
        }
    }
}
