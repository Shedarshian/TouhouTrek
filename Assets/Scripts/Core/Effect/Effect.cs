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
        public virtual void DoEffect(Game game, Target.TargetBase target)
        {

        }

        public virtual void Dispose(Game game)
        {

        }
    }
    public class EffectBase<T>:EffectBase where T:TargetBase
    {
        public override void DoEffect(Game game, TargetBase target)
        {
            if (target is T)
            {
                DoEffect(game, target as T);
            }
            else
            {
                Log.Error($"{GetType()}效果无法接受该类参数：{target.GetType()}");
            }
        }
        public virtual void DoEffect(Game game, T target)
        {

        }
    }
}
