using System;
using ZMDFQ.PlayerAction;

namespace ZMDFQ
{
    public class EffectBase
    {
        public object Parent;
        public virtual void DoEnable(Game game, PlayerAction.Response response)
        {

        }

        internal void Disable(Game game)
        {
            
        }
    }
    /// <summary>
    /// 用泛型简单封装一层，表示子类希望接受什么类型的参数
    /// 如果需要接受复数类型的参数的情况下，使用基类泛型然后在子类做判断
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EffectBase<T>:EffectBase where T:Response
    {
        public sealed override void DoEnable(Game game, Response response)
        {
            if (response is T)
            {
                Enable(game, response as T);
            }
            else
            {
                Log.Error($"{GetType()}效果无法接受该类参数：{response.GetType()}");
            }
        }
        public virtual void Enable(Game game, T response)
        {

        }
    }
}
