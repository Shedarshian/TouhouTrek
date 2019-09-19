using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 传教爱好者：你可以把任意手牌当传教打出，每回合最多两次
    /// </summary>
    public class CR_CP001 : HeroCard
    {

    }

    public class CR_CP001_SK1 : Skill
    {
        protected override UseWay useWay()
        {
            return SimpleRequest.Instance;
        }

        internal override Task DoEffect(Game game, UseInfo useInfo)
        {
            SimpleResponse simpleResponse = useInfo as SimpleResponse;
            AT_N001 card = new AT_N001();
            return card.DoEffect(game, simpleResponse);
        }

        internal override void Enable(Game game)
        {
            
        }
    }
}
