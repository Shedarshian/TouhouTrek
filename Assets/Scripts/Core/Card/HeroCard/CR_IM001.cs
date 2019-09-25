using System;
using System.Threading.Tasks;
using System.Collections.Generic;

using ZMDFQ.PlayerAction;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 冷门爱好者
    /// </summary>
    public class CR_IM001 : HeroCard
    {
        public override Camp camp => Camp.indivMinor;
        public override List<Skill> Skills => throw new NotImplementedException();
    }
    /// <summary>
    /// 当个人影响力为负时，可在自己的弃牌阶段后将角色正面朝上，并摸个人影响力绝对数量的手牌。
    /// </summary>
    public class CR_IM001_SK1 : PassiveSkill
    {
        public override void Enable(Game game)
        {
            game.EventSystem.Register(EventEnum.afterDiscardPhase, effect);
        }
        public override void Disable(Game game)
        {
            game.EventSystem.Remove(EventEnum.afterDiscardPhase, effect);
        }
        async Task effect(object[] args)
        {
            Game game = args[0] as Game;
            Player player = args[1] as Player;
            if (!AutoRequest)
                return;
            if (Hero.Player.Size < 0 && player == Hero.Player)
            {
                TakeChoiceResponse response = await game.WaitAnswer(new TakeChoiceRequest()
                {
                    PlayerId = Hero.Player.Id,
                    Infos = new List<string>() { "不发动技能", "发动技能" },
                    TimeOut = 10
                }) as TakeChoiceResponse;
                if (response.Index > 0)
                {
                    await Hero.FaceUp(game);
                    await Hero.Player.DrawActionCard(game, Math.Abs(Hero.Player.Size));
                }
            }
        }
    }
    /// <summary>
    /// 当角色正面朝上时，手牌上限固定为4。若个人影响力大于1，每回合摸牌阶段多摸一张牌。
    /// </summary>
    public class CR_IM001_SK2 : PassiveSkill, IPropertyModifier<int>
    {
        string IPropertyModifier<int>.propName => nameof(Player.HandMax);
        void IPropertyModifier<int>.modify(ref int value)
        {
            if (Hero.isFaceup)
                value = 4;
        }
        public override void Enable(Game game)
        {
            game.EventSystem.Register(EventEnum.BeforDrawActionCard, effect);
        }
        public override void Disable(Game game)
        {
            game.EventSystem.Remove(EventEnum.BeforDrawActionCard, effect);
        }
        Task effect(object[] args)
        {

            //Game game = args[0] as Game;
            Player player = args[0] as Player;
            EventData<int> count = args[1] as EventData<int>;
            if (Hero.Player.Size > 1 && player == Hero.Player)
            {
                count.data++;
            }
            return Task.CompletedTask;
        }
    }
    /// <summary>
    /// 最终结算时，你的得分是个人影响力的绝对值。
    /// </summary>
    public class CR_IM001_SK3 : PassiveSkill, IPropertyModifier<int>
    {
        string IPropertyModifier<int>.propName => nameof(Player.point);
        void IPropertyModifier<int>.modify(ref int value)
        {
            value = Math.Abs(Hero.Player.Size);
        }
        public override void Enable(Game game)
        {
        }
        public override void Disable(Game game)
        {
        }
    }
}
