using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZMDFQ.PlayerAction;

namespace ZMDFQ.Cards
{
    /// <summary>
    /// 传教：社区规模±2
    /// </summary>
    public class AT_N001 : ActionCard<SimpleRequest, SimpleResponse>
    {
        protected override async void doEffect(Game game, SimpleResponse useWay)
        {
            Effects.GoUsedDeck(game, this, useWay);
            TakeChoiceResponse response = (TakeChoiceResponse)await game.WaitAnswer(new TakeChoiceRequest()
            {
                PlayerId = useWay.PlayerId,
                Infos = new List<string>()
                {
                    "+2",
                    "-2",
                }
            });
            if (response.Index == 0)
            {
                game.ChangeSize(2);
            }
            else
            {
                game.ChangeSize(-2);
            }
        }

        protected override SimpleRequest useWay()
        {
            return SimpleRequest.Instance;
        }
    }
}
