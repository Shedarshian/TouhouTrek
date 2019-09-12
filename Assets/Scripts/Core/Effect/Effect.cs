using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ
{
    public class EffectBase
    {
        public dynamic Parent;
        public virtual void DoEnable(Game game, PlayerAction.ActionBase target)
        {

        }

        public virtual void Disable(Game game)
        {

        }

        public virtual Type GetUseType()
        {
            return typeof(Simple);
        }
    }
    public class EffectBase<T>:EffectBase where T:ActionBase
    {
        public sealed override void DoEnable(Game game, ActionBase target)
        {
            if (target is T)
            {
                Enable(game, target as T);
            }
            else
            {
                Log.Error($"{GetType()}效果无法接受该类参数：{target.GetType()}");
            }
        }
        public virtual void Enable(Game game, T target)
        {

        }

        public override Type GetUseType()
        {
            return typeof(T);
        }
    }
}
