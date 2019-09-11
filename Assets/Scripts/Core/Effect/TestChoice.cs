using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.Target;

namespace ZMDFQ.Effect
{
    public class TestChoice:EffectBase
    {
        public override async void DoEffect(Game game, TargetBase target)
        {
            var result = await game.WaitAnswer();
            Log.Debug("errrrrrrrrrrr");
        }
    }
}
