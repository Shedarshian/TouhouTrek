using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 东方警察
    /// </summary>
    public class CR_CM001 : HeroCard
    {
        public override Camp camp => Camp.commuMinor;
        public override List<Skill> Skills => new List<Skill>() { new CR_CM001_SK1() };
    }
    /// <summary>
    /// 出警：你可以将任意手牌当做“出警”打出，每回合最多两次。
    /// </summary>
    public class CR_CM001_SK1 : Skill
    {
        public override string Name => "出警";
        Effects.TurnLimit turnLimit { get; } = new Effects.TurnLimit() { MaxUseTime = 2 };
        public override void Enable(Game game)
        {
            turnLimit.Enable(game, game.Players.IndexOf(Hero.Player));
        }
        public override void Disable(Game game)
        {
            turnLimit.Disable(game);
        }
        protected override bool canUse(Game game, Request nowRequest, FreeUse useInfo, out NextRequest nextRequest)
        {
            nextRequest = null;
            if (!turnLimit.CanUse())
                return false;
            if (useInfo.Source.Count < 1)
            {
                nextRequest = new CardChooseRequest() { };
                return false;
            }
            else
                return new AT_N012().CanUse(game, nowRequest, useInfo, out nextRequest);
        }
        public override Task DoEffect(Game game, FreeUse useInfo)
        {
            turnLimit.Use();
            return new AT_N012().DoEffect(game, useInfo);
        }
    }
}