using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.Target;

namespace ZMDFQ
{
    public class EffectBase
    {
        public virtual void Enable(Game game, Target.TargetBase target)
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
    public class EffectBase<T>:EffectBase where T:TargetBase
    {
        public override void Enable(Game game, TargetBase target)
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
