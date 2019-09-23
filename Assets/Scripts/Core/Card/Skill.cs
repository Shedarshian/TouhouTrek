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
        /// 根据输入信息决定技能是否能使用，如果否，那么提供技能还需要的参数
        /// </summary>
        /// <param name="game"></param>
        /// <param name="nowRequest">当前处于什么询问中</param>
        /// <param name="useInfo"></param>
        /// <param name="nextRequest">要使用技能还需要什么参数</param>
        /// <returns></returns>
        public abstract bool CanUse(Game game, Request nowRequest, FreeUse useInfo, out UseRequest nextRequest);

        /// <summary>
        /// 选择角色牌后就生效
        /// </summary>
        public abstract void Enable(Game game);

        /// <summary>
        /// 某些原因导致技能失效
        /// </summary>
        /// <param name="game"></param>
        public abstract void Disable(Game game);

        /// <summary>
        /// 主动使用时效果
        /// </summary>
        /// <param name="game"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public abstract Task DoEffect(Game game, FreeUse useInfo);
    }
}
