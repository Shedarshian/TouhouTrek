using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ.Effects
{
    /// <summary>
    /// 用于 每回合限x次的技能
    /// </summary>
    public class TurnLimit
    {
        public int MaxUseTime;
        public int UseTime;

        public void Enable(Game game,int seat)
        {
            game.EventSystem.Register(EventEnum.TurnStart, seat, resetUseTime, 0);
        }

        public void Disable(Game game)
        {
            game.EventSystem.Remove(EventEnum.TurnStart, resetUseTime);
        }

        public void Use()
        {
            UseTime--;
        }

        private Task resetUseTime(object[] param)
        {
            UseTime = MaxUseTime;
            return Task.CompletedTask;
        } 

        public bool CanUse()
        {
            return UseTime > 0;
        }
    }
}
