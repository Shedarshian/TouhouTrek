using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZMDFQ
{
    using PlayerAction;
    public class AI : Player
    {
        public void Init(Game game)
        {
            game.EventSystem.Register(EventEnum.ActionStart, async (x) =>
            {
                if (game.ActivePlayer == this)
                {
                    await Task.Delay(500);
                    game.DoAction(new EndTurn() { playerId = Id });
                }
            }, 0);
            game.OnRequest += doResponse;
        }

        async void doResponse(Game game, Request request)
        {
            if (request.playerId != Id) return;
            await Task.Delay(500);//假装思考0.1s
            switch (request)
            {
                case ChooseSomeCardRequest  dropCard:
                    List<ActionCard> cards = new List<ActionCard>();
                    for (int i = 0; i < dropCard.Count; i++)
                    {
                        cards.Add(ActionCards[i]);
                    }
                    game.Answer(new ChooseSomeCardResponse()
                    {
                        playerId = Id,
                        Cards = cards
                    });
                    break;
                case ChooseHeroRequest chooseHero:
                    game.Answer(new ChooseHeroResponse()
                    {
                        playerId = Id,
                        HeroId = chooseHero.HeroIds[0],
                    });
                    break;
            }
        }
    }
}
