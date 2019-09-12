using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Effect
{
    public class MoreSizeChange:EffectBase
    {
        public int Need;
        public int Change;
        public EventEnum disable;
        public override void DoEnable(Game game, ActionBase target)
        {
            game.EventSystem.Register(EventEnum.OnGameSizeChange, MoreChange, 0);
            game.EventSystem.Register(disable, registerDisable, 0);
        }

        public override void Disable(Game game)
        {
            game.EventSystem.Remove(EventEnum.OnGameSizeChange, MoreChange);
        }

        void registerDisable(object[] data)
        {
            Game game = data[0] as Game;
            Disable(game);
            game.EventSystem.Remove(disable, registerDisable);
        }

        void MoreChange(object[] data)
        {
            EventData<int> value = data[0] as EventData<int>;
            if (value.data >= Need)
            {
                value.data += Change;
                Log.Debug($"繁荣度变化多了{value.data}");
            }
        }
    }
}
