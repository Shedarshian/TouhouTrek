using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.Target;

namespace ZMDFQ.Effect
{
    public class MoreSizeChange:EffectBase
    {
        public int Need;
        public int Change;
        public override void Enable(Game game, TargetBase target)
        {
            game.EventSystem.Register(EventEnum.OnGameSizeChange, MoreChange, 0);
        }

        public override void Disable(Game game)
        {
            game.EventSystem.Remove(EventEnum.OnGameSizeChange, MoreChange);
        }

        void MoreChange(object[] data)
        {
            EventData<int> value = data[0] as EventData<int>;
            if (value.data > Need)
            {
                value.data += Change;
                Log.Debug($"繁荣度变化多了{value.data}");
            }
        }
    }
}
