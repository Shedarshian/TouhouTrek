using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Effect
{
    /// <summary>
    /// 游戏繁荣度变化为x以上时，额外偏移y，持续到z
    /// </summary>
    public class MoreSizeChange:EffectBase
    {
        /// <summary>
        /// 需要多少效果触发额外值
        /// </summary>
        public int Need;
        /// <summary>
        /// 额外值的效果量
        /// </summary>
        public int Change;
        /// <summary>
        /// 失效的时机
        /// </summary>
        public EventEnum disable;
        public override void DoEnable(Game game, Response response)
        {
            game.EventSystem.Register(EventEnum.OnGameSizeChange, MoreChange, 0);
            game.EventSystem.Register(disable, registerDisable, 0);
        }

        void registerDisable(object[] data)
        {
            Game game = data[0] as Game;
            game.EventSystem.Remove(EventEnum.OnGameSizeChange, MoreChange);
            game.EventSystem.Remove(disable, registerDisable);
        }

        void MoreChange(object[] data)
        {
            EventData<int> value = data[0] as EventData<int>;
            if (value.data >= Need)
            {
                value.data += Change;
                Log.Debug($"繁荣度变化多了{Change},最终为{value.data}");
            }
        }
    }
}
