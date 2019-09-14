using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Effect
{
    public class ChangeSelfSize : EffectBase<PlayerAction.ChooseSomeoneResponse>
    {
        public int Size;
        public override void Enable(Game game, ChooseSomeoneResponse response)
        {
            foreach (var player in response.Targets)
                player.ChangeSize(Size);
        }
    }
}
