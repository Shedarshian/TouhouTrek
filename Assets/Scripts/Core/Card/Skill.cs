using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    using MongoDB.Bson.Serialization.Attributes;
    using PlayerAction;
    public abstract class Skill
    {
        [BsonIgnore]
        /// <summary>
        /// 持有此技能的英雄
        /// </summary>
        public HeroCard Hero;

        public string Name;

        public int ConfigId;

        [BsonIgnore]
        /// <summary>
        /// 未翻开时 是否询问响应技能
        /// </summary>
        public bool AutoRequest = false;
        /// <summary>
        /// 这是不是一个需要骰子的技能？
        /// </summary>
        public virtual bool NeedDice { get; } = false;
        /// <summary>
        /// 根据输入信息决定技能是否能使用，如果否，那么提供技能还需要的参数
        /// </summary>
        /// <param name="game"></param>
        /// <param name="nowRequest">当前处于什么询问中</param>
        /// <param name="useInfo"></param>
        /// <param name="nextRequest">要使用技能还需要什么参数</param>
        /// <returns></returns>
        public bool CanUse(Game game, Request nowRequest, FreeUse useInfo, out NextRequest nextRequest)
        {
            NextRequest request;
            bool result = canUse(game, nowRequest, useInfo, out request);
            EventData<bool> boolData = new EventData<bool>() { data = result };
            EventData<NextRequest> nextRequestData = new EventData<NextRequest>() { data = request };
            Task task = game.EventSystem.Call(EventEnum.onCheckCanUse, game.ActivePlayerSeat(), this, boolData, nextRequestData);
            if (!task.GetAwaiter().IsCompleted)
            {
                Log.Error($"EventEnum.onCheckCanUse必须同步运行");
                nextRequest = request;
                return result;
            }
            task.GetAwaiter().GetResult();
            nextRequest = nextRequestData.data;
            return boolData.data;
        }

        protected abstract bool canUse(Game game, Request nowRequest, FreeUse useInfo, out NextRequest nextRequest);

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

    /// <summary>
    /// 被动技能，无法使用，也没有主动效果
    /// </summary>
    public abstract class PassiveSkill : Skill
    {
        protected override bool canUse(Game game, Request nowRequest, FreeUse useInfo, out NextRequest nextRequest)
        {
            nextRequest = null;
            return false;
        }

        public override Task DoEffect(Game game, FreeUse useInfo)
        {
            return Task.CompletedTask;
        }
    }
}
