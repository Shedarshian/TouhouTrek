using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Effect
{
    public class TestChoice:EffectBase
    {
        public override async void DoEnable(Game game, ActionBase target)
        {
            var result = await game.WaitAnswer();
            Log.Debug("errrrrrrrrrrr");
        }
    }
}
