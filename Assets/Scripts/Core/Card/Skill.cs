using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    using PlayerAction;
    public abstract class Skill
    {
        public string Name;
        /// <summary>
        /// 未翻开时 是否询问响应技能
        /// </summary>
        public bool AutoRequest = false;

        /// <summary>
        /// 主动使用时应输入的参数类型，不可主动使用则为空
        /// </summary>
        public UseWay UseWay => useWay();

        protected abstract UseWay useWay();
        /// <summary>
        /// 选择角色牌后就生效
        /// </summary>
        internal abstract void Enable(Game game);

        /// <summary>
        /// 某些原因导致技能失效
        /// </summary>
        /// <param name="game"></param>
        internal abstract void Disable(Game game);

        /// <summary>
        /// 主动使用时效果
        /// </summary>
        /// <param name="game"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        internal abstract Task DoEffect(Game game, UseInfo useInfo);
    }
}
