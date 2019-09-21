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
        public override Camp camp
        {
            get { return Camp.commuMajor; }
        }
        public override List<Skill> Skills => skills;
        List<Skill> skills = new List<Skill>() { new CR_CP001_SK1() { Name = "传教" } };
    }

    public class CR_CP001_SK1 : Skill,ITreatAs
    {
        Effects.TurnLimit turnLimit = new Effects.TurnLimit() { MaxUseTime = 2 };


        public override bool CanUse(Game game, Request nowRequest, FreeUse useInfo, out UseRequest nextRequest)
        {
            nextRequest = null;
            if (!turnLimit.CanUse())
            {
                return false;
            }
            return new AT_N001().CanUse(game, nowRequest, useInfo, out nextRequest);
        }

        //protected override UseRequest useWay()
        //{
        //    if (turnLimit.CanUse())
        //        return SimpleRequest.Instance;
        //    else
        //        return null;
        //}

        public override Task DoEffect(Game game, FreeUse useInfo)
        {
            turnLimit.Use();
            //SimpleResponse simpleResponse = useInfo as SimpleResponse;
            AT_N001 card = new AT_N001();
            return card.DoEffect(game, useInfo);
        }

        public override void Enable(Game game)
        {
            turnLimit.Enable(game);
        }
        public override void Disable(Game game)
        {
            turnLimit.Disable(game);
        }

        public Card TreatTo(Game game, FreeUse useOneCard)
        {
            return new AT_N001();
        }

        public bool CanTreat(Game game, FreeUse useOneCard)
        {
            return (useOneCard.Source.Count == 1);
        }
    }
}
